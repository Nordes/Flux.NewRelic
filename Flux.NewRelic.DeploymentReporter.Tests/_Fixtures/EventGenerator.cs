using Flux.NewRelic.DeploymentReporter.Models.Flux;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Flux.NewRelic.DeploymentReporter.Tests._Fixtures
{
    internal static class EventGenerator
    {
        private readonly static JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
        };

        public static Event Get(Kind kind, string name = null, string ns = "flux-system", string timestamp = "2021-03-06T00:52:41Z", string imageTag = "1.2.3-develop.1023")
        {
            switch (kind)
            {
                case Kind.ImagePolicy:
                    name ??= "the-policy-name";
                    return JsonSerializer.Deserialize<Event>(@"
                        {
                            ""involvedObject"": {
                                ""kind"": ""ImagePolicy"",
                                ""namespace"": """ + ns + @""",
                                ""name"": """ + name + @""",
                                ""uid"": ""fd62c2f8-fe04-4fe8-bfd8-b7a583ea0f67"",
                                ""apiVersion"": ""image.toolkit.fluxcd.io/v1alpha1"",
                                ""resourceVersion"": ""69855472""
                            },
                            ""severity"": ""info"",
                            ""timestamp"": """ + timestamp + @""",
                            ""message"": ""Latest image tag for \u0027some-acr.azurecr.io/container-name\u0027 resolved to: " + imageTag + @""",
                            ""reason"": ""info"",
                            ""reportingController"": ""image-reflector-controller"",
                            ""reportingInstance"": ""image-reflector-controller-85796d5c4d-78czj""
                        }
                        ",
                        _jsonSerializerOptions);
                case Kind.GitRepository:
                    name ??= "flux-system";
                    return JsonSerializer.Deserialize<Event>(@"
                        {
	                        ""involvedObject"": {

                                ""kind"": ""GitRepository"",
                                ""namespace"": """ + ns + @""",
                                ""name"": """ + name + @""",
                                ""uid"": ""f5f03733-8a9f-4b0f-b943-1bde4b944cb5"",
                                ""apiVersion"": ""source.toolkit.fluxcd.io/v1beta1"",
                                ""resourceVersion"": ""69855487""
                            },
	                        ""severity"": ""info"",
                            ""timestamp"": """ + timestamp + @""",
	                        ""message"": ""Fetched revision: main/49fc0e14ad37d5a4918c572280118e2fb12ac6e8"",
	                        ""reason"": ""info"",
	                        ""reportingController"": ""source-controller"",
	                        ""reportingInstance"": ""source-controller-c55db769d-gqvhr""
                        }
                    ",
                    _jsonSerializerOptions);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
