namespace Flux.NewRelic.DeploymentReporter.Configurations
{
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
