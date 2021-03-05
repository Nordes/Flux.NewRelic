using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flux.NewRelic.DeploymentReporter.Configurations;
using Microsoft.Extensions.Options;

namespace Flux.NewRelic.DeploymentReporter.Security.Store
{
	public class InMemory : IGetApiKeyQuery
	{
		private readonly IDictionary<string, ApiKey> _apiKeys;

		public InMemory(ApplicationConfig config)
		{
			if (config.ApiKeys.Any())
			{
				_apiKeys = config.ApiKeys.ToDictionary(apiKey => apiKey.Key, apiKey => apiKey);
				// Improvement: Look at changes in the file (background job) and refresh the keys. ConfigMap can update without restarting the pod.
			}

			// [Example below]
			// var existingApiKeys = new List<ApiKey>
			// {
			// 	new ApiKey(1, "Flux-Dev", "8f1e9594-55cc-44dc-b76a-e084cdd57d83", DateTime.UtcNow,
			// 		new List<string>
			// 		{
			// 			Roles.Hook,
			// 		}),
			// };
			//
			// _apiKeys = existingApiKeys.ToDictionary(x => x.Key, x => x);
		}

		public Task<ApiKey> Execute(string providedApiKey)
		{
			_apiKeys.TryGetValue(providedApiKey, out var key);
			return Task.FromResult(key);
		}
	}
}