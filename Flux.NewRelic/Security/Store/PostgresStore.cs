using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flux.NewRelic.DeploymentReporter.Security.Store
{
	public class PostgresStore : IApiKeyStore
	{
		public PostgresStore()
		{
			// TODO Probably need to run a migration to deploy itself then add the keys if none were existing.
		}

		public Task<ApiKey> ExecuteAsync(string key)
		{
			throw new NotImplementedException();
		}

        public void RefreshKeys(Dictionary<string, ApiKey> keys)
        {
			// Should do nothing, since we use a database
        }
    }
}
