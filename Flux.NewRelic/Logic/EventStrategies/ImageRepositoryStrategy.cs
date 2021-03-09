using Flux.NewRelic.DeploymentReporter.Models.Flux;
using System;
using System.Threading.Tasks;

namespace Flux.NewRelic.DeploymentReporter.Logic.EventStrategies
{
    public class ImageRepositoryStrategy : IEventStrategy
    {
        public Kind Kind { get; } = Kind.ImageRepository;

        public Task ExecuteAsync(Event @event)
        {
            throw new NotImplementedException();
        }
    }
}

