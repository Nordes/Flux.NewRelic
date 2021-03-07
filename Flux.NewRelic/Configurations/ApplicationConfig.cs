using System.Collections.Generic;
using Flux.NewRelic.DeploymentReporter.Models.Flux;
using Flux.NewRelic.DeploymentReporter.Security;

namespace Flux.NewRelic.DeploymentReporter.Configurations
{
    public class ApplicationConfig
	{
		/// <summary>
		/// ApiKey to access the API's
		/// </summary>
		public List<ApiKey> ApiKeys { get; set; }

		/// <summary>
		/// Mappings between Flux Policies and NewRelic entities
		/// </summary>
		public List<Mapping> Mappings { get; set; }

		/// <summary>
		/// NewRelic configuration
		/// </summary>
		public NewRelicConfig NewRelic { get; set; }

		public class Mapping
		{
			/// <summary>
			/// Flux object name
			/// </summary>
			public string FluxPolicyName { get; set; }

            public Kind Kind { get; set; }
            public string NewRelicAppId { get; set; }

        }

		public class NewRelicConfig
		{
			public string LicenseKey { get; set; }
			public NewRelicDeploymentConfig Deployment { get; set; }

			// Inner class since it will never be outside this.
			public class NewRelicDeploymentConfig
			{
				public string User { get; set; }
				public string DefaultDescription { get; set; }
			}
		}
	}
}
