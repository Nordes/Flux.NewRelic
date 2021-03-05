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

		public HookController(ILogger<HookController> logger)
		{
			_logger = logger;
		}

		[HttpPut]
		[HttpPost]
		public IActionResult PutData([FromBody]dynamic hookContent, [FromQuery] HookType type)
		{
			return ManageHookContent(hookContent, type);
		}


		private IActionResult ManageHookContent(dynamic hookContent, HookType type)
		{
			// TODO some stuff here... heh (business logic)
			var data = System.Text.Json.JsonSerializer.Serialize(hookContent);
			
			_logger.LogDebug($"{Request.Method}: {data}");

			return NoContent();
		}
	}
}
