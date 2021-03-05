using System;

namespace Flux.NewRelic.DeploymentReporter.Security.Exceptions
{
	public class ForbiddenProblemDetails : Exception
	{
		public ForbiddenProblemDetails() : base("Forbidden")
		{
		}
	}
}