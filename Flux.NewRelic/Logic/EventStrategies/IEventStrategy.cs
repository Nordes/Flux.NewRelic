using Flux.NewRelic.DeploymentReporter.Models.Flux;
using System.Threading.Tasks;

namespace Flux.NewRelic.DeploymentReporter.Logic.EventStrategies
{
    public interface IEventStrategy
    {
        Kind Kind { get; }

        Task ExecuteAsync(Event @event);
    }
}

