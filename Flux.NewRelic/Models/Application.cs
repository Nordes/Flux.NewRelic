using System;

namespace Flux.NewRelic.DeploymentReporter.Models
{
	public class Application
	{
		public int id { get; set; }
		public string name { get; set; }
		public string language { get; set; }
		public DateTime last_reported_at { get; set; }
	}
}
