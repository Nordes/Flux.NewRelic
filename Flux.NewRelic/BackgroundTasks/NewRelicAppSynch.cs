using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Flux.NewRelic.DeploymentReporter.BackgroundTasks
{
	public class NewRelicAppSynch: BackgroundService
	{
		private readonly ILogger<NewRelicAppSynch> _logger;

		public NewRelicAppSynch(ILogger<NewRelicAppSynch> logger)
		{
			_logger = logger;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{

				await Task.Delay(TimeSpan.FromMinutes(60), stoppingToken);
			}
		}
	}
}
