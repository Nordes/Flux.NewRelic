using Flux.NewRelic.DeploymentReporter.Models.Flux;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flux.NewRelic.DeploymentReporter.Logic.EventStrategies
{
    public class BucketStrategy : IEventStrategy
    {
        public Kind Kind => Kind.Bucket;

        public Task ExecuteAsync(Event @event)
        {
            throw new NotImplementedException();
        }
    }
}
