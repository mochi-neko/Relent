#nullable enable
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Mochineko.HttpResult.Samples
{
    [TestFixture]
    internal class HttpClientSample
    {
        private static readonly HttpClient httpClient
            = new HttpClient();

        [Test]
        [RequiresPlayMode(false)]
        public async Task GetSample()
        {
            var url = "https://www.google.com/";
            using var cancellationTokenSource = new CancellationTokenSource();
            var result = await CallGetAPIAsync(httpClient, url, cancellationTokenSource.Token);
            if (result is IHttpSuccessResult<string> success)
            {
                // Happy path
                Debug.Log(success.Result);
            }
            else if (result is IHttpRetryableResult<string> retryable)
            {
                // Can retry
                Debug.LogWarning(retryable.Message);
            }
            else if (result is IHttpFailureResult<string> failure)
            {
                // Handle error
                Debug.LogError(failure.Message);
            }
            else
            {
                // Unexpected
                throw new Exception();
            }
        }
      
        private static async Task<IHttpResult<string>> CallGetAPIAsync(
            HttpClient httpClient,
            string url,
            CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return HttpResult.Fail<string>(
                    $"Failed because task has been already cancelled when called the API.");
            }

            using var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            try
            {
                using var responseMessage = await httpClient
                    .SendAsync(requestMessage, cancellationToken);
                if (responseMessage == null)
                {
                    return HttpResult.Fail<string>(
                        $"Failed because {nameof(HttpResponseMessage)} was null.");
                }
                
                if (responseMessage.IsSuccessStatusCode)
                {
                    if (responseMessage.Content == null)
                    {
                        return HttpResult.Fail<string>(
                            $"Failed because {nameof(HttpResponseMessage.Content)} was null.");
                    }

                    var responseText = await responseMessage.Content.ReadAsStringAsync();
                    if (responseText == null)
                    {
                        return HttpResult.Fail<string>(
                            $"Failed because response text was null.");
                    }

                    // Success
                    return HttpResult.Succeed(responseText);
                }
                // Retryable
                else if (responseMessage.StatusCode is HttpStatusCode.TooManyRequests
                         || (int)responseMessage.StatusCode is >= 500 and <= 599)
                {
                    return HttpResult.Retry<string>(
                        $"Retryable due to [{(int)responseMessage.StatusCode}:{responseMessage.StatusCode}].");
                }
                // Response error
                else
                {
                    return HttpResult.Fail<string>(
                        responseMessage.StatusCode == HttpStatusCode.NotFound
                            ? $"Failed because [{(int)responseMessage.StatusCode}:{responseMessage.StatusCode}] the API was not found:{url}."
                            : $"Failed because [{(int)responseMessage.StatusCode}:{responseMessage.StatusCode}] server did not respond OK."
                    );
                }
            }
            // Request error
            catch (HttpRequestException exception)
            {
                return HttpResult.Retry<string>(
                    $"Retryable because {nameof(HttpRequestException)} was thrown during calling the API:{exception}.");
            }
            // User cancellation
            catch (TaskCanceledException exception)
                when (cancellationToken.IsCancellationRequested)
            {
                return HttpResult.Fail<string>(
                    $"Failed because task was canceled by user during call to the API:{exception}.");
            }
            // Timeout cancellation
            catch (TaskCanceledException exception)
                when (exception.InnerException is TimeoutException)
            {
                return HttpResult.Retry<string>(
                    $"Retryable because {nameof(TimeoutException)} was thrown during calling the API:{exception}.");
            }
            // Unhandled error
            catch (Exception exception)
            {
                return HttpResult.Fail<string>(
                    $"Failed because an unhandled exception was thrown when calling the API:{exception}.");
            }
        }
    }
}