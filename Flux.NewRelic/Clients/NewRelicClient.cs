using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Flux.NewRelic.DeploymentReporter.Configurations;
using Flux.NewRelic.DeploymentReporter.Models.NewRelic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Flux.NewRelic.DeploymentReporter.Clients
{
    public class NewRelicClient : INewRelicClient
    {
        private readonly ILogger<NewRelicClient> _logger;
        private readonly HttpClient _httpClient;

        public NewRelicClient(ILogger<NewRelicClient> logger, HttpClient httpClient, AppSettings appConfig)
        {
            _logger = logger;
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Add("X-Api-Key", appConfig.NewRelic.LicenseKey);
            // System.Text.JSON https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-ignore-properties?pivots=dotnet-5-0
        }

        public async Task CreateDeploymentAsync(string applicationId, Deployment deployment)
        {
            var serializerOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var httpContent = new StringContent(JsonSerializer.Serialize(deployment, serializerOptions), Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            // Documentation at: https://rpm.newrelic.com/api/explore/application_deployments/create
            var result = await _httpClient.PostAsync($"{applicationId}/deployments.json", httpContent);

			if (result.IsSuccessStatusCode)
				return;

            // Maybe the message is also part of the response. We never know.
			var msg = ((int)result.StatusCode) switch {
				StatusCodes.Status401Unauthorized => "Invalid API key. Contact your account admin to generate a new one, or see our API docs.",
                StatusCodes.Status403Forbidden => "Your New Relic API access isn't enabled. Contact your account admin, or see our API docs.",
                StatusCodes.Status500InternalServerError => "We hit a server error. Try again, or visit Our Support Center.",
                StatusCodes.Status400BadRequest => "The timestamp must be in UTC ISO8601 format. For example, '2019-10-08T00:15:36Z' and it must be after the most recent deployment timestamp. The application must also be reporting to record a deployment.",
                StatusCodes.Status404NotFound => "We didn't find an application with the given ID.",
				_ => $"Unknown issue. Return code was {(int)result.StatusCode}."
            };
			
            /*
			 Potential return code
				~~~~~~~~~~~~~~~~~~~~~~~~~~~
				401	Invalid API key. Contact your account admin to generate a new one, or see our API docs.
				401	API key required.
				403	Your New Relic API access isn't enabled. Contact your account admin, or see our API docs.
				403	You are not authorized to delete deployments.
				500	We hit a server error. Try again, or visit Our Support Center.
				400	Revision parameter required.
				400	The timestamp must be in ISO8601 format. For example, '2019-10-08T00:15:36Z'
				400	The timestamp must be in UTC. For example, '2019-10-08T00:15:36Z'
				400	The timestamp can’t be in the future.
				400	The timestamp must be after the most recent deployment timestamp.
				400	The application must be reporting to record a deployment.
				404	We didn't find an application with the given ID.
				~~~~~~~~~~~~~~~~~~~~~~~~~~~
			 */
			_logger.LogWarning($"Issue while sending to NewRelic. Message: {msg}");

            return;
        }
    }
}
