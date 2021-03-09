using Flux.NewRelic.DeploymentReporter.Models.Flux;
using System;
using System.Threading.Tasks;

namespace Flux.NewRelic.DeploymentReporter.Logic.EventStrategies
{
    public class HelmChartStrategy : IEventStrategy
    {
        public Kind Kind => Kind.HelmChart;

        public Task ExecuteAsync(Event @event)
        {
            throw new NotImplementedException();
        }
    }
}
