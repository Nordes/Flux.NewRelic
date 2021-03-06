using System;
using System.Threading.Tasks;

namespace Flux.NewRelic.DeploymentReporter.Security.Store
{
	public class Redis : IGetApiKeyQuery
	{
		public Task<ApiKey> Execute(string providedApiKey)
		{
			throw new NotImplementedException();
		}
	}
}
