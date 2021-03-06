using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flux.NewRelic.DeploymentReporter.Configurations;

namespace Flux.NewRelic.DeploymentReporter.Security.Store
{
	public class InMemory : IGetApiKeyQuery
	{
		private readonly IDictionary<string, ApiKey> _apiKeys;

		public InMemory(ApplicationConfig config)
		{
			if (config.ApiKeys.Any())
			{
				// Improvement: Look at changes in the file (background job) and refresh the keys. ConfigMap can update without restarting the pod.
				_apiKeys = config.ApiKeys.ToDictionary(apiKey => apiKey.Key, apiKey => apiKey);
			}
		}

		public Task<ApiKey> Execute(string providedApiKey)
		{
			_apiKeys.TryGetValue(providedApiKey, out var key);
			return Task.FromResult(key);
		}
	}
}