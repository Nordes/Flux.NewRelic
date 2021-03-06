using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace Flux.NewRelic.DeploymentReporter.Security.Store
{
	public class RedisStore : IApiKeyStore
	{
		private readonly IDistributedCache _cache;

		public RedisStore(IDistributedCache cache)
		{
			_cache = cache;
		}

		public Task<ApiKey> ExecuteAsync(string key)
		{
			// Key format for cache: /flux/newrelic/apikeys/[key]
			//_cache.GetAsync()
			// _cache.GetAsync()
			throw new NotImplementedException();
		}

        public void RefreshKeys(Dictionary<string, ApiKey> keys)
        {
            throw new NotImplementedException();
        }
    }
}
