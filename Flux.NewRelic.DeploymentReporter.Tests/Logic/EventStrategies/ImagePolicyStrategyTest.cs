using Flux.NewRelic.DeploymentReporter.Clients;
using Flux.NewRelic.DeploymentReporter.Configurations;
using Flux.NewRelic.DeploymentReporter.Logic.EventStrategies;
using Flux.NewRelic.DeploymentReporter.Models.Flux;
using Flux.NewRelic.DeploymentReporter.Models.NewRelic;
using Flux.NewRelic.DeploymentReporter.Tests._Fixtures;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Flux.NewRelic.DeploymentReporter.Tests.Logic.EventStrategies
{
    public class ImagePolicyStrategyTest
    {
        private readonly ILogger<ImagePolicyStrategy> _logger;
        private readonly Mock<INewRelicClient> _newRelicClient;
        private readonly ApplicationConfig _applicationConfig;

        public ImagePolicyStrategyTest()
        {
            _logger = Mock.Of<ILogger<ImagePolicyStrategy>>();
            _newRelicClient = new Mock<INewRelicClient>(MockBehavior.Strict);
            _applicationConfig = new ApplicationConfig();
        }

        [Fact]
        public void Kind_WhenLookingForKind_ExpectImagePolicy()
        {
            // Arrange
            // Act
            var ctor = new ImagePolicyStrategy(_logger, _newRelicClient.Object, _applicationConfig);
            // Assert
            Assert.Equal(Kind.ImagePolicy, ctor.Kind);
        }

        [Fact]
        public async Task ExecuteAsync_WhenImagePolicyEventHasNoMapping_ShouldDoNothing()
        {
            // Arrange
            var ctor = new ImagePolicyStrategy(_logger, _newRelicClient.Object, _applicationConfig);
            var evt = EventGenerator.Get(ctor.Kind);
            // Act
            await ctor.ExecuteAsync(evt);
            // Assert
        }

        [Fact]
        public async Task ExecuteAsync_WhenImagePolicyEventHasNoCache_ShouldLookForChanges_ExpectNewDeploymentCreated()
        {
            // Arrange
            var ctor = new ImagePolicyStrategy(_logger, _newRelicClient.Object, _applicationConfig);
            var evt = EventGenerator.Get(ctor.Kind);
            _applicationConfig.Mappings.Add(new ApplicationConfig.Mapping() { FluxPolicyName = evt.InvolvedObject.Name, NewRelicAppId = "12345", Kind = Kind.ImagePolicy });
            _newRelicClient.Setup(m => m.CreateDeploymentAsync("12345", It.IsAny<Deployment>())).Returns(Task.CompletedTask);
            // Act
            await ctor.ExecuteAsync(evt);
            // Assert
            _newRelicClient.VerifyAll();
        }

        [Fact]
        public async Task ExecuteAsync_WhenUsingCache_ExpectNothingIfNoChanges()
        {
            // Arrange
            var ctor = new ImagePolicyStrategy(_logger, _newRelicClient.Object, _applicationConfig);
            var evt = EventGenerator.Get(ctor.Kind);
            _applicationConfig.Mappings.Add(new ApplicationConfig.Mapping() { FluxPolicyName = evt.InvolvedObject.Name, NewRelicAppId = "12345", Kind = Kind.ImagePolicy });
            _newRelicClient.Setup(m => m.CreateDeploymentAsync("12345", It.IsAny<Deployment>())).Returns(Task.CompletedTask);
            // Act
            await ctor.ExecuteAsync(evt);
            await ctor.ExecuteAsync(evt);

            // Assert
            _newRelicClient.Verify(m => m.CreateDeploymentAsync("12345", It.IsAny<Deployment>()), Times.Once);
            _newRelicClient.VerifyAll();
        }

        [Fact]
        public async Task ExecuteAsync_WhenUsingCache_ExpectNewDeploymentIfChanges()
        {
            // Arrange
            var ctor = new ImagePolicyStrategy(_logger, _newRelicClient.Object, _applicationConfig);
            var evt = EventGenerator.Get(ctor.Kind);
            var evt2 = EventGenerator.Get(ctor.Kind, imageTag: "1.2.4");
            _applicationConfig.Mappings.Add(new ApplicationConfig.Mapping() { FluxPolicyName = evt.InvolvedObject.Name, NewRelicAppId = "12345", Kind = Kind.ImagePolicy });
            _newRelicClient.Setup(m => m.CreateDeploymentAsync("12345", It.IsAny<Deployment>())).Returns(Task.CompletedTask);
            // Act
            await ctor.ExecuteAsync(evt);   // First deployment
            await ctor.ExecuteAsync(evt2);  // Second deployment
            // Assert
            _newRelicClient.Verify(m => m.CreateDeploymentAsync("12345", It.IsAny<Deployment>()), Times.Exactly(2));
            _newRelicClient.VerifyAll();
        }
    }
}
