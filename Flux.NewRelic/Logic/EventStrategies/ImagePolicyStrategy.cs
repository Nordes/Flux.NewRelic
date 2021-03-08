using Flux.NewRelic.DeploymentReporter.Clients;
using Flux.NewRelic.DeploymentReporter.Configurations;
using Flux.NewRelic.DeploymentReporter.Exceptions;
using Flux.NewRelic.DeploymentReporter.Models.Flux;
using Flux.NewRelic.DeploymentReporter.Models.NewRelic;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Flux.NewRelic.DeploymentReporter.Configurations.ApplicationConfig;

namespace Flux.NewRelic.DeploymentReporter.Logic.EventStrategies
{
    public class ImagePolicyStrategy : IEventStrategy
    {
        private readonly ILogger<ImagePolicyStrategy> _logger;
        private readonly INewRelicClient _newRelicClient;
        private readonly ApplicationConfig _appConfig;
        private readonly Dictionary<string, ImagePolicyInfo> _cache = new Dictionary<string, ImagePolicyInfo>();

        public ImagePolicyStrategy(ILogger<ImagePolicyStrategy> logger, INewRelicClient newRelicClient, ApplicationConfig appConfig)
        {
            _logger = logger;
            _newRelicClient = newRelicClient;
            _appConfig = appConfig;
        }

        public Kind Kind { get; } = Kind.ImagePolicy;

        public async Task ExecuteAsync(Event @event)
        {
            switch (@event.Severity)
            {
                case Severity.Info:
                    await ManageInfoLevelAsync(@event);
                    break;
                default:
                    throw new NotImplementedException($"{GetType().Name}: Severity type '{@event.Severity}' not implemented.");
            }
        }

        private Task ManageInfoLevelAsync(Event @event)
        {
            var (ImageName, ImageTag) = ParseMessage(@event.Message);
            _logger.LogDebug($"ImageName: {ImageName}, Tag: {ImageTag}");

            // * Look for mapping
            var mapping = _appConfig.Mappings.FirstOrDefault(f => f.Kind == @event.InvolvedObject.Kind &&f.FluxPolicyName.Equals(@event.InvolvedObject.Name, StringComparison.OrdinalIgnoreCase));
            if (mapping == default(Mapping))
                return Task.CompletedTask;

            // * Todo: look if changes
            if (_cache.TryGetValue(mapping.NewRelicAppId, out ImagePolicyInfo imagePolicyInfo))
            {
                _logger.LogDebug($"{GetType().Name}: Cache hit, now looking for changes");
                // Exists...
                imagePolicyInfo.RelatedEvent = @event;

                if (ImageTag.Equals(imagePolicyInfo.ImageTag, StringComparison.OrdinalIgnoreCase) && ImageName.Equals(imagePolicyInfo.ImageName, StringComparison.OrdinalIgnoreCase))
                    return Task.CompletedTask; // No Changes

                imagePolicyInfo.ImageName = ImageName;
                imagePolicyInfo.ImageTag = ImageTag;
            }
            else
            {
                // Not exists
                imagePolicyInfo = new ImagePolicyInfo
                {
                    ImageName = ImageName,
                    ImageTag = ImageTag,
                    RelatedEvent = @event,
                };
                _cache.Add(mapping.NewRelicAppId, imagePolicyInfo);
                // TODO - Avoid sending if already part of the list of deployments (NewRelic). This is not implemented yet.
            }

            // * Create the new deployment
            var deployment = new Deployment()
            {
                Changelog = ImageTag,
                Timestamp = @event.Timestamp,
                Description = _appConfig.NewRelic.Deployment.DefaultDescription,
                User = _appConfig.NewRelic.Deployment.User
            };

            return _newRelicClient.CreateDeploymentAsync(mapping.NewRelicAppId, deployment);
        }

        private (string ImageName, string ImageTag) ParseMessage(string message)
        {
            // Message: Latest image tag for 'some-acr.azurecr.io/container-name' resolved to: 1.2.3-develop.1023
            var infoMessageRegex = new Regex(@"^.*(?<ImageName>'.*').*: (?<ImageTag>.*)$");

            var matches = infoMessageRegex.Match(message);
            if (matches.Groups.Count != 3)
                throw new UnreconizedMessageException($"Kind: {Kind}, Message: {message}");

            return (matches.Groups["ImageName"].Value, matches.Groups["ImageTag"].Value);
        }

        private class ImagePolicyInfo
        {
            public string ImageName { get; set; }
            public string ImageTag { get; set; }
            public Event RelatedEvent { get; set; }
        }
    }
}

