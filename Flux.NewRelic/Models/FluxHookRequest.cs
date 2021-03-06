using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flux.NewRelic.DeploymentReporter.Models
{
	public class FluxHookRequest
	{
		public InvolvedObject InvolvedObject { get; set; }

		public string Severity { get; set; } // TODO <== Put as enum, we know for a fact, that it will be INFO or ERROR and nothing else (see documentation)

		/// <summary>
		/// ISO-8601 Format
		/// </summary>
		public string Timestamp { get; set; }

		public string Message { get; set; }
		public string Reason { get; set; }
		public string ReportingController { get; set; }
		public string ReportingInstance { get; set; }
	}

	public class InvolvedObject
	{
		public string Kind { get; set; } // TODO <== Put as enum, we know the kind (GitRepository, ImagePolicy, ...)
		public string Namespace { get; set; }
		public string Name { get; set; }
		public string Uid { get; set; }
		public string ApiVersion { get; set; }
		public string ResourceVersion { get; set; }
	}
}
