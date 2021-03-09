using Flux.NewRelic.DeploymentReporter.Logic.EventStrategies;
using Flux.NewRelic.DeploymentReporter.Models.Flux;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Flux.NewRelic.DeploymentReporter.Logic
{
    public class FluxEventFactory: IFluxEventFactory
    {
        public FluxEventFactory(IEnumerable<IEventStrategy> strategies)
        {
            // Not sure it's the best approach to do this.
            _strategies = strategies.ToDictionary(f => f.Kind, f => f);
        }

        private readonly Dictionary<Kind, IEventStrategy> _strategies;

        public IEventStrategy Get(Kind kind)
        {
            IEventStrategy strategy;
            if (kind == Kind.Unknown)
                throw new NotImplementedException("The kind of event is unknown and then can't be proceeded.");
            else if (!_strategies.TryGetValue(kind, out strategy))
                throw new NotImplementedException($"The kind requested \"{kind}\" is not yet implemented.");

            return strategy;
        }
    }
}

