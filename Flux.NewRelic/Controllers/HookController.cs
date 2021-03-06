using System.Threading.Tasks;
using Flux.NewRelic.DeploymentReporter.Clients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Flux.NewRelic.DeploymentReporter.Models;
using Microsoft.AspNetCore.Authorization;

namespace Flux.NewRelic.DeploymentReporter.Controllers
{
	[ApiController]
	[Route("[controller]")]
	[Authorize]
	public class HookController : ControllerBase
	{
		private readonly ILogger<HookController> _logger;
		private readonly INewRelicClient _newRelicClient;

		public HookController(ILogger<HookController> logger, INewRelicClient newRelicClient)
		{
			_logger = logger;
			_newRelicClient = newRelicClient;
		}

		[HttpPut]
		[HttpPost]
		public async Task<IActionResult> PutDataAsync([FromBody] dynamic hookContent, [FromQuery] HookType type)
		{
			return await ManageHookContentAsync(hookContent, type);
		}


		private async Task<IActionResult> ManageHookContentAsync(dynamic hookContent, HookType type)
		{
			// TODO some stuff here... heh (business logic)
			var data = System.Text.Json.JsonSerializer.Serialize(hookContent);

			_logger.LogDebug($"{Request.Method}: {data}");
			await _newRelicClient.CreateDeploymentAsync(new NewRelicDeployment());

			return NoContent();
		}
	}
}
