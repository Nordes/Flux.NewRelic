using Flux.NewRelic.DeploymentReporter.Models.Flux;
using System;
using System.Threading.Tasks;

namespace Flux.NewRelic.DeploymentReporter.Logic.EventStrategies
{
    public class GitRepositoryStrategy : IEventStrategy
    {
        public Kind Kind { get; } = Kind.GitRepository;

        public Task ExecuteAsync(Event @event)
        {
            throw new NotImplementedException();
        }
    }
}

