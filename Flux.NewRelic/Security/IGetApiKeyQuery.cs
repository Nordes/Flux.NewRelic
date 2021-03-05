using System.Threading.Tasks;

namespace Flux.NewRelic.DeploymentReporter.Security
{
	public interface IGetApiKeyQuery
	{
		Task<ApiKey> Execute(string providedApiKey);
	}
}