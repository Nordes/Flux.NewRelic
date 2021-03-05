using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Flux.NewRelic.DeploymentReporter.Models;
using Flux.NewRelic.DeploymentReporter.Security;
using Flux.NewRelic.DeploymentReporter.Security.Store;

namespace Flux.NewRelic.DeploymentReporter
{
	public class Startup
	{
		private IConfiguration _configuration;

		public Startup(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();
			services.AddMemoryCache();

			// Debug purpose, should not go to prod like this? (maybe yes... who knows).
			services.AddSingleton<IGetApiKeyQuery, InMemory>();

			services.AddAuthentication(options =>
		   {
			   options.DefaultAuthenticateScheme = ApiKeyAuthenticationOptions.DefaultScheme;
			   options.DefaultChallengeScheme = ApiKeyAuthenticationOptions.DefaultScheme;
		   })
		   .AddApiKeySupport(options => { });
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseRouting();
			//if (!env.("....something..._IN_DOCKER")){
			//	// As we run in DOCKER, this is not necessary. Our gateway or sidecar should do the https before us
			//	//app.UseHttpsRedirection();
			//}

			// API Key management
			app.UseAuthentication();
			app.UseAuthorization();

			// Controller stuff
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
