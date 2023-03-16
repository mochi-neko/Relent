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
        [Test]
        [RequiresPlayMode(false)]
        public async Task GetSample()
        {
            var url = "https://www.google.com/";
            using var cancellationTokenSource = new CancellationTokenSource();
            using var httpClient = new HttpClient();
            var result = await CallGetAPIAsync(httpClient, url, cancellationTokenSource.Token);
            if (result is IHttpSuccessResult<string> success)
            {
                // Happy path
                Debug.Log(success.Result);
            }
            else if (result is IHttpRetryableResult<string> retryable)
            {
                // Can retry
                throw new Exception();
            }
            else if (result is IHttpFailureResult<string> failure)
            {
                // Handle error
                throw new Exception();
            }
            else
            {
                // Unexpected
                throw new Exception();
            }
        }

        [Test]
        [RequiresPlayMode(false)]
        public async Task CancelByUser()
        {
            LogAssert.ignoreFailingMessages = true;
            
            using var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(0.1d));
            
            var result = await WaitAsync(cancellationTokenSource.Token, TimeSpan.FromSeconds(10d));
            if (result is IHttpSuccessResult<string> success)
            {
                // Happy path
                throw new Exception();
            }
            else if (result is IHttpRetryableResult<string> retryable)
            {
                // Can retry
                throw new Exception();
            }
            else if (result is IHttpFailureResult<string> failure)
            {
                // Handle error
            }
            else
            {
                // Unexpected
                throw new Exception();
            }
        }
        
        [Test]
        [RequiresPlayMode(false)]
        public async Task CancelByTimeout()
        {
            LogAssert.ignoreFailingMessages = true;
            
            using var httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromSeconds(0.1d);
            
            var url = "https://www.google.com/";
            
            var retryCount = 0;

            while (retryCount < 5)
            {
                var result = await CallGetAPIAsync(
                    httpClient,
                    url,
                    CancellationToken.None);
                if (result is IHttpSuccessResult<string> success)
                {
                    // Happy path
                    throw new Exception("Succeeded is invalid");
                }
                else if (result is IHttpRetryableResult<string> retryable)
                {
                    // Can retry
                    retryCount++;
                    Debug.Log($"Retry:{retryCount}");
                }
                else if (result is IHttpFailureResult<string> failure)
                {
                    // Handle error
                    throw new Exception(failure.Message);
                }
                else
                {
                    // Unexpected
                    throw new Exception();
                }
            }
            
            Debug.Log("Over retry count.");
            GC.KeepAlive(httpClient);
        }

        private static async Task<IHttpResult<string>> CallGetAPIAsync(
            HttpClient httpClient,
            string url,
            CancellationToken cancellationToken)
        {
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
                    Debug.LogWarning(
                        $"Retryable due to [{(int)responseMessage.StatusCode}:{responseMessage.StatusCode}].");
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
                Debug.LogWarning(
                    $"Retryable because {nameof(HttpRequestException)} was thrown during calling the API:{exception}.");
                return HttpResult.Retry<string>(
                    $"Retryable because {nameof(HttpRequestException)} was thrown during calling the API:{exception}.");
            }
            // User cancellation
            catch (TaskCanceledException exception)
                when (exception.CancellationToken == cancellationToken)
            {
                Debug.LogError(
                    $"Failed because task was canceled by user during call to the API:{exception}.");
                return HttpResult.Fail<string>(
                    $"Failed because task was canceled by user during call to the API:{exception}.");
            }
            // Timeout or other cancellation
            catch (TaskCanceledException exception)
            {
                Debug.LogWarning(
                    $"Retryable because task was cancelled during calling the API:{exception}.");
                return HttpResult.Retry<string>(
                    $"Retryable because task was cancelled during calling the API:{exception}.");
            }
            // Timeout or other cancellation 
            catch (OperationCanceledException exception)
            {
                Debug.LogWarning(
                    $"Retryable because operation was cancelled during calling the API:{exception}.");
                return HttpResult.Retry<string>(
                    $"Retryable because operation was cancelled during calling the API:{exception}.");
            }
            // Unhandled error
            catch (Exception exception)
            {
                Debug.LogError(
                    $"Failed because an unhandled exception was thrown when calling the API:{exception}.");
                return HttpResult.Fail<string>(
                    $"Failed because an unhandled exception was thrown when calling the API:{exception}.");
            }
        }

        private static async Task<IHttpResult<string>> WaitAsync(
            CancellationToken cancellationToken,
            TimeSpan time)
        {
            try
            {
                await Task.Delay(time, cancellationToken);

                Debug.Log("Finished");
                return HttpResult.Succeed("Finished");
            }
            // User cancellation
            catch (TaskCanceledException exception)
                when (cancellationToken.IsCancellationRequested)
            {
                Debug.LogError(exception);
                return HttpResult.Fail<string>(
                    $"Failed because task was canceled by user during waiting with exception:{exception}.");
            }
            // Timeout cancellation
            catch (TaskCanceledException exception)
                when (exception.InnerException is TimeoutException)
            {
                Debug.LogWarning(exception);
                return HttpResult.Retry<string>(
                    $"Retryable because timeout during waiting with exception:{exception}.");
            }
            // Unhandled error
            catch (Exception exception)
            {
                Debug.LogException(exception);
                return HttpResult.Fail<string>(
                    $"Failed because an unhandled exception was thrown when waiting:{exception}.");
            }
        }
    }
}