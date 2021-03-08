using System.Threading.Tasks;
using Flux.NewRelic.DeploymentReporter.Models.NewRelic;

namespace Flux.NewRelic.DeploymentReporter.Clients
{
    public interface INewRelicClient
    {
        Task CreateDeploymentAsync(string applicationId, Deployment deployment);
    }
}
