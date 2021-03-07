using Flux.NewRelic.DeploymentReporter.Logic.EventStrategies;
using System.Threading.Tasks;

namespace Flux.NewRelic.DeploymentReporter.Logic
{
    public interface IFluxEventFactory
    {
        IEventStrategy Get(Models.Flux.Kind kind);
    }
}