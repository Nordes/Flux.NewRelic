using Flux.NewRelic.DeploymentReporter.Logic;
using Flux.NewRelic.DeploymentReporter.Logic.EventStrategies;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace Flux.NewRelic.DeploymentReporter.Tests.Logic
{
    public class FluxEventFactoryTest
    {
        private List<IEventStrategy> _eventStrategies;

        public FluxEventFactoryTest()
        {
            _eventStrategies = new List<IEventStrategy>();
        }

        [Fact]
        public void Get_WhenKindStrategyExists_ExpectStrategyKind()
        {
            // Arrange
            var fakeImagePolicyStrategy = new Mock<IEventStrategy>();
            fakeImagePolicyStrategy.Setup(m => m.Kind).Returns(Models.Flux.Kind.ImagePolicy);
            _eventStrategies.Add(fakeImagePolicyStrategy.Object);
            // Act
            var fluxEventFactory = new FluxEventFactory(_eventStrategies);
            var result = fluxEventFactory.Get(Models.Flux.Kind.ImagePolicy);
            // Assert
            Assert.Equal(fakeImagePolicyStrategy.Object, result);
        }

        [Fact]
        public void Get_WhenKindStrategyIsUnknown_ExpectThrowNotImplementedException()
        {
            // Arrange
            // Act
            var fluxEventFactory = new FluxEventFactory(_eventStrategies);
            var exception = Assert.Throws<NotImplementedException>(() => { fluxEventFactory.Get(Models.Flux.Kind.Unknown); });
            // Assert
            Assert.Equal("The kind of event is unknown and then can't be proceeded.", exception.Message);
        }

        [Fact]
        public void Get_WhenKindStrategyIsNotImplemented_ExpectThrowNotImplementedException()
        {
            // Arrange
            // Act
            var fluxEventFactory = new FluxEventFactory(_eventStrategies);
            var exception = Assert.Throws<NotImplementedException>(() => { fluxEventFactory.Get(Models.Flux.Kind.ImagePolicy); });
            // Assert
            Assert.Equal($"The kind requested \"{Models.Flux.Kind.ImagePolicy}\" is not yet implemented.", exception.Message);
        }
    }
}
