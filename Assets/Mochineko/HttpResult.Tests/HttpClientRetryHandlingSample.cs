#nullable enable
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Mochineko.HttpResult.Tests
{
    [TestFixture]
    internal sealed class HttpClientRetryHandlingSample
    {
        private const string DummyUrl = "http://localhost:8080";

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(5)]
        [TestCase(10)]
        [RequiresPlayMode(false)]
        public async Task RetrySample(int maxRetryCount)
        {
            using var httpClient = new HttpClient(new MockedHttpMessageHandler(_ =>
            {
                var response = new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)
                {
                    Content = new StringContent("You can retry this request.")
                };

                return Task.FromResult(response);
            }));

            var retryCount = 0;

            while (retryCount < maxRetryCount)
            {
                var result = await MockWebAPI.GetAsync(httpClient, DummyUrl, CancellationToken.None);
                if (result is IHttpSuccessResult<string> success)
                {
                    // Should break the loop on your code
                    throw new Exception();
                }
                else if (result is IHttpRetryableResult<string> retryable)
                {
                    retryCount++;
                }
                else if (result is IHttpFailureResult<string> failure)
                {
                    // Should handle failure on your code
                    throw new Exception();
                }
                else
                {
                    // Unexpected
                    throw new HttpResultPatternMatchException(nameof(result));
                }
            }

            retryCount.Should().Be(maxRetryCount);
        }

        [TestCase(0f)]
        [TestCase(0.1f)]
        [TestCase(0.5f)]
        [TestCase(1f)]
        [RequiresPlayMode(false)]
        public async Task TimeoutWithUserCancellationSample(float timeoutSeconds)
        {
            using var httpClient = new HttpClient(
                new MockedHttpMessageHandler(async cancellationToken =>
                {
                    await Task.Delay(TimeSpan.FromSeconds(20d), cancellationToken);

                    throw new InvalidOperationException();
                }));

            using var userCancellationTokenSource = new CancellationTokenSource();

            using var timeoutCancellationTokenSource = new CancellationTokenSource();
            timeoutCancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(timeoutSeconds));

            using var linkedCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(
                userCancellationTokenSource.Token,
                timeoutCancellationTokenSource.Token);

            var result = await MockWebAPI.GetAsync(httpClient, DummyUrl, linkedCancellationTokenSource.Token);
            result.Retryable.Should().BeTrue();
        }

        [Test]
        [RequiresPlayMode(false)]
        public async Task RetryWithMultiTimeoutAndBreakSample()
        {
            using var httpClient = new HttpClient(new MockedHttpMessageHandler(async cancellationToken =>
            {
                await Task.Delay(TimeSpan.FromSeconds(0.3f), cancellationToken);

                var response = new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)
                {
                    Content = new StringContent("You can retry this request.")
                };

                return response;
            }));

            // Timeout of total
            using var totalTimeoutCancellationTokenSource = new CancellationTokenSource();
            totalTimeoutCancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(1d));

            var retryCount = 0;

            while (retryCount < 10)
            {
                if (totalTimeoutCancellationTokenSource.IsCancellationRequested)
                {
                    break;
                }

                // Timeout of loop
                using var loopTimeoutCancellationTokenSource = new CancellationTokenSource();
                loopTimeoutCancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(0.2f));

                using var linkedCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(
                    totalTimeoutCancellationTokenSource.Token,
                    loopTimeoutCancellationTokenSource.Token);

                var result = await MockWebAPI.GetAsync(httpClient, DummyUrl, linkedCancellationTokenSource.Token);
                if (result is IHttpSuccessResult<string> success)
                {
                    // Should break the loop on your code
                    throw new Exception();
                }
                else if (result is IHttpRetryableResult<string> retryable)
                {
                    retryCount++;

                    // Break time to retry
                    try
                    {
                        await Task.Delay(TimeSpan.FromSeconds(0.1f), totalTimeoutCancellationTokenSource.Token);
                    }
                    catch (OperationCanceledException)
                    {
                        Debug.Log($"Cancel to break at {retryCount}th retrying.");
                        break;
                    }
                }
                else if (result is IHttpFailureResult<string> failure)
                {
                    // Should handle failure on your code
                    throw new Exception();
                }
                else
                {
                    // Unexpected
                    throw new HttpResultPatternMatchException(nameof(result));
                }
            }

            retryCount.Should().Be(3); // 0.2s(loop) * 3 = 0.6s < 1s(total)
            totalTimeoutCancellationTokenSource.IsCancellationRequested.Should().BeTrue();
        }

        [Test]
        [RequiresPlayMode(false)]
        public async Task RetryWithCircuitBreakerAndTimeoutSample()
        {
            using var httpClient = new HttpClient(new MockedHttpMessageHandler(_ =>
            {
                var response = new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)
                {
                    Content = new StringContent("You can retry this request.")
                };

                return Task.FromResult(response);
            }));

            var retryCount = 0;
            var circuitCount = 0;

            var timeoutCancellationSource = new CancellationTokenSource();
            timeoutCancellationSource.CancelAfter(TimeSpan.FromSeconds(5f));

            while (retryCount < 10)
            {
                if (timeoutCancellationSource.IsCancellationRequested)
                {
                    break;
                }

                var result = await MockWebAPI.GetAsync(httpClient, DummyUrl, timeoutCancellationSource.Token);
                if (result is IHttpSuccessResult<string> success)
                {
                    // Should break the loop on your code
                    throw new Exception();
                }
                else if (result is IHttpRetryableResult<string> retryable)
                {
                    retryCount++;
                    circuitCount++;

                    if (circuitCount >= 3)
                    {
                        // Break time to retry
                        try
                        {
                            await Task.Delay(TimeSpan.FromSeconds(3f), timeoutCancellationSource.Token);
                            circuitCount = 0;
                        }
                        catch (TaskCanceledException)
                        {
                            Debug.Log($"Cancel to circuit break at {retryCount}th retrying.");
                            break;
                        }
                    }
                }
                else if (result is IHttpFailureResult<string> failure)
                {
                    // Should handle failure on your code
                    throw new Exception();
                }
                else
                {
                    // Unexpected
                    throw new HttpResultPatternMatchException(nameof(result));
                }
            }

            retryCount.Should().Be(6); // 6times = 3s(break) * 2 > 5s(timeout) 
        }
    }
}