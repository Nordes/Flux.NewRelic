using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flux.NewRelic.DeploymentReporter.Security;

namespace Flux.NewRelic.DeploymentReporter.Configurations
{
	public class ApplicationConfig
	{
		public List<ApiKey> ApiKeys { get; set; }
		public List<Mapping> Mappings { get; set; }
		public NewRelicConfig NewRelic { get; set; }

		public class Mapping
		{
			public string FluxPolicyName { get; set; }
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
