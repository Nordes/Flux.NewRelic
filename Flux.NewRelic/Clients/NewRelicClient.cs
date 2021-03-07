using System.Net.Http;
using System.Threading.Tasks;
using Flux.NewRelic.DeploymentReporter.Models.NewRelic;

namespace Flux.NewRelic.DeploymentReporter.Clients
{
    public class NewRelicClient: INewRelicClient
	{

		private readonly HttpClient _httpClient;
		private readonly string _remoteServiceBaseUrl;

		public NewRelicClient(HttpClient httpClient)
		{
			_httpClient = httpClient;
			_remoteServiceBaseUrl = _httpClient.BaseAddress?.ToString();
			// Newtonsoft... but MS should also have something similar 
			//
			//var settings = new JsonSerializerSettings
			//{
			//	NullValueHandling = NullValueHandling.Ignore,
			//	MissingMemberHandling = MissingMemberHandling.Ignore
			//};

			// System.Text.JSON https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-ignore-properties?pivots=dotnet-5-0
		}

		public Task CreateDeploymentAsync(NewRelicDeployment deployment)
		{
			// Do the call here.
			// https://rpm.newrelic.com/api/explore/application_deployments/create

			/*
				curl -X POST 'https://api.newrelic.com/v2/applications/{application_id}/deployments.json' \
				     -H 'X-Api-Key:{api_key}' -i \
				     -H 'Content-Type: application/json' \
				     -d \
				'{
				  "deployment": {
				    "revision": "string",
				    "changelog": "string",
				    "description": "string",
				    "user": "string"
				  }
				}' 
			 */

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

			return Task.CompletedTask;
		}
	}

	public interface INewRelicClient
	{
		Task CreateDeploymentAsync(NewRelicDeployment deployment);
	}
}
