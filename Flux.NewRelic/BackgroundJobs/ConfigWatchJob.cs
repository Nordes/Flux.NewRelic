using Flux.NewRelic.DeploymentReporter.Security;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Flux.NewRelic.DeploymentReporter.BackgroundJobs
{
    public class ConfigWatchJob : BackgroundService
    {
        private readonly IApiKeyStore _apiKeyStore;

        public ConfigWatchJob(IApiKeyStore apiKeyStore)
        {
            _apiKeyStore = apiKeyStore;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Look at the appsettings.json (let's consider that one as the configuration. Maybe it will become a different one later.

                // If file changed, we need to update the security store.
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken); 
            }
        }
    }
}
