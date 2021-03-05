using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using Flux.NewRelic.DeploymentReporter.Security.Exceptions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace Flux.NewRelic.DeploymentReporter.Security
{
	public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
	{
		private const string ProblemDetailsContentType = "application/problem+json";
		private readonly IGetApiKeyQuery _getApiKeyQuery;
		private const string ApiKeyHeaderName = "X-Api-Key";

		public ApiKeyAuthenticationHandler(
			IOptionsMonitor<ApiKeyAuthenticationOptions> options,
			ILoggerFactory logger,
			UrlEncoder encoder,
			ISystemClock clock,
			IGetApiKeyQuery getApiKeyQuery) : base(options, logger, encoder, clock)
		{
			_getApiKeyQuery = getApiKeyQuery ?? throw new ArgumentNullException(nameof(getApiKeyQuery));
		}

		protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
		{
			var apiKeyQueryValue = new StringValues(string.Empty);
			if (!Request.Headers.TryGetValue(ApiKeyHeaderName, out var apiKeyHeaderValues) && !Request.Query.TryGetValue("api_key", out apiKeyQueryValue))
			{
				return AuthenticateResult.NoResult();
			}

			var providedApiKey = apiKeyHeaderValues.FirstOrDefault() ?? apiKeyQueryValue.FirstOrDefault();

			if (apiKeyHeaderValues.Count == 0 && string.IsNullOrEmpty(apiKeyQueryValue) || string.IsNullOrWhiteSpace(providedApiKey))
			{
				return AuthenticateResult.NoResult();
			}

			var existingApiKey = await _getApiKeyQuery.Execute(providedApiKey);

			if (existingApiKey != null)
			{
				var claims = new List<Claim>
					{
						new Claim(ClaimTypes.Name, existingApiKey.Owner)
					};

				claims.AddRange(existingApiKey.Roles.Select(role => new Claim(ClaimTypes.Role, role)));

				var identity = new ClaimsIdentity(claims, Options.AuthenticationType);
				var identities = new List<ClaimsIdentity> { identity };
				var principal = new ClaimsPrincipal(identities);
				var ticket = new AuthenticationTicket(principal, Options.Scheme);

				return AuthenticateResult.Success(ticket);
			}

			return AuthenticateResult.NoResult();
		}

		protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
		{
			Response.StatusCode = 401;
			Response.ContentType = ProblemDetailsContentType;
			var problemDetails = new UnauthorizedProblemDetails();

			await Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
		}

		protected override async Task HandleForbiddenAsync(AuthenticationProperties properties)
		{
			Response.StatusCode = 403;
			Response.ContentType = ProblemDetailsContentType;
			var problemDetails = new ForbiddenProblemDetails();

			await Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
		}
	}
}
