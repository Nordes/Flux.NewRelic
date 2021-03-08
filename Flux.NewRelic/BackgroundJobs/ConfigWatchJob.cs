using Flux.NewRelic.DeploymentReporter.Configurations;
using Flux.NewRelic.DeploymentReporter.Security;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Flux.NewRelic.DeploymentReporter.BackgroundJobs
{
    public class ConfigWatchJob : BackgroundService
    {
        private readonly ILogger<ConfigWatchJob> _logger;
        private readonly IApiKeyStore _apiKeyStore;
        private readonly ApplicationConfig _appConfig;
        private readonly TimeSpan _loopDelay = TimeSpan.FromSeconds(30);

        public ConfigWatchJob(ILogger<ConfigWatchJob> logger, IApiKeyStore apiKeyStore, ApplicationConfig appConfig)
        {
            _logger = logger;
            _apiKeyStore = apiKeyStore;
            _appConfig = appConfig;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Look at the appsettings.json (let's consider that one as the configuration. Maybe it will become a different one later.

                // If file changed, we need to
                // 1. update the security store.
                // 2. update mappings
                // 3. update other stuff... most likely
                
                _logger.LogTrace($"No changes, waiting {_loopDelay.TotalSeconds} seconds"); // We could use a real "System.IO.FileSystemWatcher" instead.
                await Task.Delay(_loopDelay, stoppingToken); 
            }
        }
    }
}
