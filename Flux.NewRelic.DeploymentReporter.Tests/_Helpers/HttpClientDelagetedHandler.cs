using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Flux.NewRelic.DeploymentReporter.Tests._Helpers
{
	[ExcludeFromCodeCoverage]
    public class HttpClientDelagetedHandler : DelegatingHandler
    {
        private readonly Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> _handlerFunc;
        public HttpRequestMessage Request { get; private set; }

        public HttpClientDelagetedHandler()
        {
            _handlerFunc = (request, cancellationToken) =>
            {
                Request = request;

                return Task.FromResult(new HttpResponseMessage { StatusCode = HttpStatusCode.OK });
            };
        }

        public HttpClientDelagetedHandler(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> handlerFunc)
        {
            _handlerFunc = handlerFunc;
        }

        public static (HttpClient, HttpClientDelagetedHandler) GetFakeClient(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> handlerFunc = null, string baseAddress = "https://localhost:5001/fake/")
        {
            if (handlerFunc == null)
            {
                var handlerA = new HttpClientDelagetedHandler();
                return (new HttpClient(handlerA) { BaseAddress = new Uri(baseAddress) }, handlerA);
            }

            var handler = new HttpClientDelagetedHandler(handlerFunc);
            return (new HttpClient(handler) { BaseAddress = new Uri(baseAddress) }, handler);
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return _handlerFunc(request, cancellationToken);
        }
    }
}
