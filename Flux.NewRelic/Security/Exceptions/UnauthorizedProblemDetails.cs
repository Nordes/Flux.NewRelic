using System;

namespace Flux.NewRelic.DeploymentReporter.Security.Exceptions
{
	public class UnauthorizedProblemDetails : Exception
	{
		public UnauthorizedProblemDetails() : base("Unauthorized")
		{
		}
	}
}