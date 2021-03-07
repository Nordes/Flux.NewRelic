using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flux.NewRelic.DeploymentReporter.Security.Store
{
	public class RedisStore : IApiKeyStore
	{
		public Task<ApiKey> Execute(string providedApiKey)
		{
			throw new NotImplementedException();
		}

        public void RefreshKeys(Dictionary<string, ApiKey> keys)
        {
            throw new NotImplementedException();
        }
    }
}
