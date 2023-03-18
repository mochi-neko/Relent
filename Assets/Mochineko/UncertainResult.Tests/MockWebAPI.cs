#nullable enable
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Mochineko.UncertainResult.Tests
{
    internal static class MockWebAPI
    {
        public static async Task<IUncertainResult<string>> GetAsync(
            HttpClient httpClient,
            string url,
            CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                Debug.LogWarning(
                    $"Retryable because operation was cancelled before calling the API.");
                return UncertainResult.Retry<string>(
                    $"Retryable because operation was cancelled before calling the API.");
            }
            
            using var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            try
            {
                using var responseMessage = await httpClient
                    .SendAsync(requestMessage, cancellationToken);
                if (responseMessage == null)
                {
                    return UncertainResult.Fail<string>(
                        $"Failed because {nameof(HttpResponseMessage)} was null.");
                }

                if (responseMessage.IsSuccessStatusCode)
                {
                    if (responseMessage.Content == null)
                    {
                        return UncertainResult.Fail<string>(
                            $"Failed because {nameof(HttpResponseMessage.Content)} was null.");
                    }

                    var responseText = await responseMessage.Content.ReadAsStringAsync();
                    if (responseText == null)
                    {
                        return UncertainResult.Fail<string>(
                            $"Failed because response text was null.");
                    }

                    // Success
                    return UncertainResult.Succeed(responseText);
                }
                // Retryable
                else if (responseMessage.StatusCode is HttpStatusCode.TooManyRequests
                         || (int)responseMessage.StatusCode is >= 500 and <= 599)
                {
                    Debug.LogWarning(
                        $"Retryable because the API returned status code:({(int)responseMessage.StatusCode}){responseMessage.StatusCode}.");
                    return UncertainResult.Retry<string>(
                        $"Retryable because the API returned status code:({(int)responseMessage.StatusCode}){responseMessage.StatusCode}.");
                }
                // Response error
                else
                {
                    return UncertainResult.Fail<string>(
                        $"Failed because the API returned status code:({(int)responseMessage.StatusCode}){responseMessage.StatusCode}."
                    );
                }
            }
            // Request error
            catch (HttpRequestException exception)
            {
                Debug.LogWarning(
                    $"Retryable because {nameof(HttpRequestException)} was thrown during calling the API:{exception}.");
                return UncertainResult.Retry<string>(
                    $"Retryable because {nameof(HttpRequestException)} was thrown during calling the API:{exception}.");
            }
            // Task cancellation
            catch (TaskCanceledException exception)
                when (exception.CancellationToken == cancellationToken)
            {
                Debug.LogWarning(
                    $"Failed because task was canceled by user during call to the API:{exception}.");
                return UncertainResult.Retry<string>(
                    $"Failed because task was canceled by user during call to the API:{exception}.");
            }
            // Operation cancellation 
            catch (OperationCanceledException exception)
            {
                Debug.LogWarning(
                    $"Retryable because operation was cancelled during calling the API:{exception}.");
                return UncertainResult.Retry<string>(
                    $"Retryable because operation was cancelled during calling the API:{exception}.");
            }
            // Unhandled error
            catch (Exception exception)
            {
                Debug.LogError(
                    $"Failed because an unhandled exception was thrown when calling the API:{exception}.");
                return UncertainResult.Fail<string>(
                    $"Failed because an unhandled exception was thrown when calling the API:{exception}.");
            }
        }
    }
}