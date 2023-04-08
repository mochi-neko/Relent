#nullable enable
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Mochineko.Relent.UncertainResult.Tests
{
    public static class MockWebAPI
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
                return UncertainResults.RetryWithTrace<string>(
                    $"Retryable because operation was cancelled before calling the API.");
            }
            
            using var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            try
            {
                using var responseMessage = await httpClient
                    .SendAsync(requestMessage, cancellationToken);
                if (responseMessage == null)
                {
                    return UncertainResults.FailWithTrace<string>(
                        $"Failed because {nameof(HttpResponseMessage)} was null.");
                }

                if (responseMessage.IsSuccessStatusCode)
                {
                    if (responseMessage.Content == null)
                    {
                        return UncertainResults.FailWithTrace<string>(
                            $"Failed because {nameof(HttpResponseMessage.Content)} was null.");
                    }

                    var responseText = await responseMessage.Content.ReadAsStringAsync();
                    if (responseText == null)
                    {
                        return UncertainResults.FailWithTrace<string>(
                            $"Failed because response text was null.");
                    }

                    // Success
                    return UncertainResults.Succeed(responseText);
                }
                // Retryable
                else if (responseMessage.StatusCode is HttpStatusCode.TooManyRequests
                         || (int)responseMessage.StatusCode is >= 500 and <= 599)
                {
                    Debug.LogWarning(
                        $"Retryable because the API returned status code:({(int)responseMessage.StatusCode}){responseMessage.StatusCode}.");
                    return UncertainResults.RetryWithTrace<string>(
                        $"Retryable because the API returned status code:({(int)responseMessage.StatusCode}){responseMessage.StatusCode}.");
                }
                // Response error
                else
                {
                    return UncertainResults.FailWithTrace<string>(
                        $"Failed because the API returned status code:({(int)responseMessage.StatusCode}){responseMessage.StatusCode}."
                    );
                }
            }
            // Request error
            catch (HttpRequestException exception)
            {
                Debug.LogWarning(
                    $"Retryable because {nameof(HttpRequestException)} was thrown during calling the API:{exception}.");
                return UncertainResults.RetryWithTrace<string>(
                    $"Retryable because {nameof(HttpRequestException)} was thrown during calling the API:{exception}.");
            }
            // Task cancellation
            catch (TaskCanceledException exception)
                when (exception.CancellationToken == cancellationToken)
            {
                Debug.LogWarning(
                    $"Failed because task was canceled by user during call to the API:{exception}.");
                return UncertainResults.RetryWithTrace<string>(
                    $"Failed because task was canceled by user during call to the API:{exception}.");
            }
            // Operation cancellation 
            catch (OperationCanceledException exception)
            {
                Debug.LogWarning(
                    $"Retryable because operation was cancelled during calling the API:{exception}.");
                return UncertainResults.RetryWithTrace<string>(
                    $"Retryable because operation was cancelled during calling the API:{exception}.");
            }
            // Unhandled error
            catch (Exception exception)
            {
                Debug.LogError(
                    $"Failed because an unhandled exception was thrown when calling the API:{exception}.");
                return UncertainResults.FailWithTrace<string>(
                    $"Failed because an unhandled exception was thrown when calling the API:{exception}.");
            }
        }
    }
}