using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Flux.NewRelic.DeploymentReporter.Logic;
using System;

namespace Flux.NewRelic.DeploymentReporter.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class HookController : ControllerBase
    {
        private readonly ILogger<HookController> _logger;
        private readonly IFluxEventFactory _fluxEventFactory;

        public HookController(ILogger<HookController> logger, IFluxEventFactory fluxEventFactory)
        {
            _logger = logger;
            _fluxEventFactory = fluxEventFactory;
        }

        [HttpPut]
        [HttpPost]
        public async Task<IActionResult> PutDataAsync([FromBody] Models.Flux.Event @event)
        {
            var data = System.Text.Json.JsonSerializer.Serialize(@event);
            _logger.LogDebug($"{Request?.Method}: {data}");

            try
            {
                var executionStrategy = _fluxEventFactory.Get(@event.InvolvedObject.Kind);
                await executionStrategy.ExecuteAsync(@event);
            }
            catch (Exception e)
            {
                // Swallow the error
                _logger.LogError(e, "Error while proceeding the requested content.", @event);
            }

            return NoContent();
        }
    }
}
