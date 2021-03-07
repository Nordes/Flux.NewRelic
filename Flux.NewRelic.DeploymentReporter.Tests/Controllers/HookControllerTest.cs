using Flux.NewRelic.DeploymentReporter.Controllers;
using Flux.NewRelic.DeploymentReporter.Logic;
using Flux.NewRelic.DeploymentReporter.Logic.EventStrategies;
using Flux.NewRelic.DeploymentReporter.Models.Flux;
using Flux.NewRelic.DeploymentReporter.Tests._Fixtures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Flux.NewRelic.DeploymentReporter.Tests.Controllers
{
    public class HookControllerTest
    {
        private readonly ILogger<HookController> _logger;
        private readonly Mock<IFluxEventFactory> _fluxEventFactory;
        private readonly Mock<IEventStrategy> _fluxEventStrategy;
        private readonly HookController _hookController;

        public HookControllerTest()
        {
            _logger = Mock.Of<ILogger<HookController>>();
            _fluxEventFactory = new Mock<IFluxEventFactory>();
            _fluxEventStrategy = new Mock<IEventStrategy>();
            _hookController = new HookController(_logger, _fluxEventFactory.Object);
        }

        [Fact]
        public async Task PutDataAsync_WhenExistingKind_ShouldReturnNoContent()
        {
            // Arrange
            _fluxEventFactory.Setup(m => m.Get(Kind.ImagePolicy)).Returns(_fluxEventStrategy.Object);
            _fluxEventStrategy.Setup(m => m.ExecuteAsync(It.IsAny<Event>()));
            var evt = EventGenerator.Get(Kind.ImagePolicy);
            // Act
            var result = (StatusCodeResult) await _hookController.PutDataAsync(evt);
            // Assert
            Assert.Equal(StatusCodes.Status204NoContent, result.StatusCode);
            _fluxEventFactory.VerifyAll();
            _fluxEventStrategy.VerifyAll();
        }

        [Fact]
        public async Task PutDataAsync_WhenError_ShouldReturnNoContent()
        {
            // Arrange
            _fluxEventFactory.Setup(m => m.Get(Kind.ImagePolicy)).Throws(new NotImplementedException("The kind of event is unknown and then can't be proceeded."));
            var evt = EventGenerator.Get(Kind.ImagePolicy);
            // Act
            var result = (StatusCodeResult)await _hookController.PutDataAsync(evt);
            // Assert
            Assert.Equal(StatusCodes.Status204NoContent, result.StatusCode);
            _fluxEventFactory.VerifyAll();
            _fluxEventStrategy.VerifyAll();
        }
    }
}
