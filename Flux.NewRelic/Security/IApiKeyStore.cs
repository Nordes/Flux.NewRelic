using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flux.NewRelic.DeploymentReporter.Security
{
	public interface IApiKeyStore
	{
		void RefreshKeys(Dictionary<string, ApiKey> keys);

		Task<ApiKey> ExecuteAsync(string key);
	}
}