using System;
using Flux.NewRelic.DeploymentReporter.Models;
using Microsoft.AspNetCore.Authentication;

namespace Flux.NewRelic.DeploymentReporter.Security
{
	// Implementation ref: https://josef.codes/asp-net-core-protect-your-api-with-api-keys/

	public static class AuthenticationBuilderExtensions
	{
		public static AuthenticationBuilder AddApiKeySupport(this AuthenticationBuilder authenticationBuilder, Action<ApiKeyAuthenticationOptions> options)
		{
			return authenticationBuilder.AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(ApiKeyAuthenticationOptions.DefaultScheme, options);
		}
	}
}
