using System;
using Flux.NewRelic.DeploymentReporter.Clients;
using Flux.NewRelic.DeploymentReporter.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Flux.NewRelic.DeploymentReporter.Security;
using Flux.NewRelic.DeploymentReporter.Security.Store;
using Flux.NewRelic.DeploymentReporter.Logic;
using Flux.NewRelic.DeploymentReporter.Logic.EventStrategies;
using Flux.NewRelic.DeploymentReporter.BackgroundJobs;
using System.Diagnostics.CodeAnalysis;

namespace Flux.NewRelic.DeploymentReporter
{
	[ExcludeFromCodeCoverage]
	public class Startup
	{
		private readonly IConfiguration _configuration;

		public Startup(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			// Look at : https://edi.wang/post/2019/1/5/auto-refresh-settings-changes-in-aspnet-core-runtime
			var applicationConfig = _configuration.GetSection("AppSettings").Get<AppSettings>();
            services.AddSingleton(applicationConfig);
			// interesting thing about auto-reload: https://edi.wang/post/2019/1/5/auto-refresh-settings-changes-in-aspnet-core-runtime
			//services.Configure<AppSettings>(_configuration.GetSection(nameof(AppSettings))); // (Could also do something like this)

			services.AddControllers();
			services.AddMemoryCache();
			services.AddHttpClient<INewRelicClient, NewRelicClient>(client =>
				{
					client.BaseAddress = new Uri("https://api.newrelic.com/v2/");
				});
			services.AddAuthentication(options =>
				{
					options.DefaultAuthenticateScheme = ApiKeyAuthenticationOptions.DefaultScheme;
					options.DefaultChallengeScheme = ApiKeyAuthenticationOptions.DefaultScheme;
				})
				.AddApiKeySupport(options => { });
			
			// Debug purpose, should not go to prod like this? (maybe yes... who knows).
			services.AddSingleton<IApiKeyStore, InMemoryStore>();

			services.AddStackExchangeRedisCache(o =>
				{
					o.Configuration = _configuration.GetConnectionString("redis");
				});

			// Add background job
			services.AddHostedService<ConfigWatchJob>();

			// Strategies for how to handle the message.
			services.AddSingleton<IFluxEventFactory, FluxEventFactory>();
			services.AddSingleton<IEventStrategy, BucketStrategy>();
			services.AddSingleton<IEventStrategy, GitRepositoryStrategy>();
			services.AddSingleton<IEventStrategy, KustomizationStrategy>();
			services.AddSingleton<IEventStrategy, HelmChartStrategy>();
			services.AddSingleton<IEventStrategy, HelmReleaseStrategy>();
			services.AddSingleton<IEventStrategy, HelmRepositoryStrategy>();
			services.AddSingleton<IEventStrategy, ImagePolicyStrategy>();
			services.AddSingleton<IEventStrategy, ImageRepositoryStrategy>();
			services.AddSingleton<IEventStrategy, ImageUpdateAutomationStrategy>();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseRouting();

			if (!bool.TryParse(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") ?? "", out var result) || !result)
			{
				// If you put a real SSL certificate in your docker image, then remove below line, otherwise no SSL
				app.UseHttpsRedirection();
			}

			// API Key management
			app.UseAuthentication();
			app.UseAuthorization();

			// Controllers
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
