using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Flux.NewRelic.DeploymentReporter.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class HookController : ControllerBase
	{
		private readonly ILogger<HookController> _logger;

		public HookController(ILogger<HookController> logger)
		{
			_logger = logger;
		}

		[HttpPut]
		public IActionResult PutData([FromBody]dynamic hookContent, [FromQuery] HookType type)
		{
			return ManageHookContent(hookContent, type);
		}

		[HttpPost]
		public IActionResult PostData([FromBody] dynamic hookContent, [FromQuery] HookType type)
		{
			return ManageHookContent(hookContent, type);
		}

		private IActionResult ManageHookContent(dynamic hookContent, HookType type)
		{
			var data = System.Text.Json.JsonSerializer.Serialize(hookContent);
			
			_logger.LogDebug($"{Request.Method}: {data}");

			return NoContent();
		}
	}

	public enum HookType
	{
		Unknown = 0,
		Flux = 1,
	}
}
