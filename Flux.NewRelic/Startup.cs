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

namespace Flux.NewRelic.DeploymentReporter
{
	public class Startup
	{
		private readonly IConfiguration _configuration;

		public Startup(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			var applicationConfig = _configuration.GetSection("Config").Get<ApplicationConfig>();

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
			services.AddSingleton(applicationConfig);
			services.AddSingleton<IGetApiKeyQuery, InMemory>();

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
