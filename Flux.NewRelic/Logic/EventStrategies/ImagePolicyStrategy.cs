using Flux.NewRelic.DeploymentReporter.Clients;
using Flux.NewRelic.DeploymentReporter.Models.Flux;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flux.NewRelic.DeploymentReporter.Logic.EventStrategies
{
    public class ImagePolicyStrategy : IEventStrategy
    {
        private readonly INewRelicClient _newRelicClient;
        private readonly Dictionary<string, string> _versionMapping = new Dictionary<string, string>();

        public ImagePolicyStrategy(INewRelicClient newRelicClient)
        {
            _newRelicClient = newRelicClient;
        }

        public Kind Kind { get; } = Kind.ImagePolicy;

        public Task ExecuteAsync(Event @event)
        {
            // await _newRelicClient.CreateDeploymentAsync(new NewRelicDeployment());
            throw new NotImplementedException();
        }
    }
}

