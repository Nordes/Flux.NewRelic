using System;

namespace Flux.NewRelic.DeploymentReporter.Models
{
	internal class ImagePolicyMapping
	{
		public string FluxPolicyName { get; set; }
		public long NewRelicAppId { get; set; }
	}
}
