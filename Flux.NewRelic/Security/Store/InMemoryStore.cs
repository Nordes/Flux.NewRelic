using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Flux.NewRelic.DeploymentReporter.Configurations;

namespace Flux.NewRelic.DeploymentReporter.Security.Store
{
	public class InMemoryStore : IApiKeyStore
	{
		private readonly IDictionary<string, ApiKey> _apiKeys;
		private static readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1);

		public InMemoryStore(AppSettings config)
		{
			if (config.ApiKeys.Any())
			{
				// Improvement: Look at changes in the file (background job) and refresh the keys. ConfigMap can update without restarting the pod.
				_apiKeys = config.ApiKeys.ToDictionary(apiKey => apiKey.Key, apiKey => apiKey);
			}
		}

		/// <inheritdoc cref="IApiKeyStore.ExecuteAsync"/>
		public Task<ApiKey> ExecuteAsync(string key)
		{
			_semaphoreSlim.Wait();
			_apiKeys.TryGetValue(key, out var keyFound);
			_semaphoreSlim.Release();
			return Task.FromResult(keyFound);
		}

		/// <summary>
		/// Refresh the keys in case the config file have changed.
		/// </summary>
		/// <param name="keys">New keys to take into consideration.</param>
		public void RefreshKeys(Dictionary<string, ApiKey> keys)
		{
			_semaphoreSlim.Wait();
			_apiKeys.Clear();
			foreach (var (key, apiKey) in keys)
			{
				_apiKeys.Add(key, apiKey);
			}
			_semaphoreSlim.Release();
		}
	}
}