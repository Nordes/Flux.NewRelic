using Flux.NewRelic.DeploymentReporter.Clients;
using Flux.NewRelic.DeploymentReporter.Configurations;
using Flux.NewRelic.DeploymentReporter.Tests._Helpers;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Flux.NewRelic.DeploymentReporter.Tests.Clients
{
    public class NewRelicClientTest
    {
        private readonly ILogger<NewRelicClient> _logger;
        private readonly AppSettings _appConfig;

        public NewRelicClientTest()
        {
            _logger = Mock.Of<ILogger<NewRelicClient>>();
            _appConfig = new AppSettings
            {
                NewRelic = new AppSettings.NewRelicConfig
                {
                    LicenseKey = "ABCDEF"
                },
                Mappings = new List<AppSettings.Mapping> { }
            };
        }

        [Fact]
        public async Task CreateDeployment_WhenSending_ShouldIgnoreJsonPropertiesWithNoValuesInRequests()
        {
            // Arrange
            var (httpClient, handler) = HttpClientDelagetedHandler.GetFakeClient();
            var expectedRawRequest = @"{""revision"":""Develop-1234"",""changelog"":""1.2.3"",""timestamp"":""2019-10-08T00:15:36Z""}";

            // Act
            var ctor = new NewRelicClient(_logger, httpClient, _appConfig);
            await ctor.CreateDeploymentAsync("12345", new Models.NewRelic.Deployment { Changelog = "1.2.3", Revision = "Develop-1234", Timestamp = "2019-10-08T00:15:36Z" });

            // Assert
            var rawRequestSent = await handler.Request.Content.ReadAsStringAsync();
            Assert.Equal(expectedRawRequest, rawRequestSent);
        }
    }
}
