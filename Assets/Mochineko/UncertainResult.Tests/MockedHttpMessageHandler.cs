#nullable enable
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Mochineko.UncertainResult.Tests
{
    public sealed class MockedHttpMessageHandler : HttpMessageHandler
    {
        private readonly Func<CancellationToken, Task<HttpResponseMessage>>? responseFactory;
        private readonly HttpResponseMessage? httpResponseMessage;

        public MockedHttpMessageHandler(HttpResponseMessage httpResponseMessage)
        {
            this.httpResponseMessage = httpResponseMessage;
        }

        public MockedHttpMessageHandler(Func<CancellationToken, Task<HttpResponseMessage>> responseFactory)
        {
            this.responseFactory = responseFactory ?? throw new ArgumentNullException(nameof(responseFactory));
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            if (responseFactory != null)
            {
                return await responseFactory.Invoke(cancellationToken);
            }
            else if (httpResponseMessage != null)
            {
                return httpResponseMessage;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }
}