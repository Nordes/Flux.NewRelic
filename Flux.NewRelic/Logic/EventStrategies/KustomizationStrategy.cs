using Flux.NewRelic.DeploymentReporter.Models.Flux;
using System;
using System.Threading.Tasks;

namespace Flux.NewRelic.DeploymentReporter.Logic.EventStrategies
{
    public class KustomizationStrategy : IEventStrategy
    {
        public Kind Kind => Kind.Kustomization;

        public Task ExecuteAsync(Event @event)
        {
            throw new NotImplementedException();
        }
    }
}
