#nullable enable
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Mochineko.Relent.Resilience.CircuitBreaker;
using Mochineko.Relent.Resilience.Retry;
using Mochineko.Relent.Resilience.Timeout;
using Mochineko.Relent.Resilience.Wrap;
using Mochineko.Relent.UncertainResult;
using Mochineko.Relent.UncertainResult.Tests;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Mochineko.Relent.Resilience.Tests
{
    [TestFixture]
    internal sealed class HttpClientResilienceSample
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

            var retryPolicy = RetryFactory
                .Retry<string>(maxRetryCount);

            var result = await retryPolicy.ExecuteAsync(
                async cancellationToken => await MockWebAPI.GetAsync(httpClient, DummyUrl, cancellationToken),
                CancellationToken.None);

            if (result is IUncertainFailureResult<string> failure)
            {
                Debug.LogError(failure.Message);
            }
            
            result.Retryable.Should().BeTrue();
            retryPolicy.RetryCount.Should().Be(maxRetryCount);
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

            var timeoutPolicy = TimeoutFactory
                .Timeout<string>(TimeSpan.FromSeconds(timeoutSeconds));

            var result = await timeoutPolicy.ExecuteAsync(
                async cancellationToken => await MockWebAPI.GetAsync(httpClient, DummyUrl, cancellationToken),
                userCancellationTokenSource.Token);
            
            if (result is IUncertainFailureResult<string> failure)
            {
                Debug.LogError(failure.Message);
            }
            
            result.Retryable.Should().BeTrue();
        }

        [Test]
        [RequiresPlayMode(false)]
        public async Task RetryWithMultiTimeoutAndWaitSample()
        {
            using var httpClient = new HttpClient(new MockedHttpMessageHandler(async cancellationToken =>
            {
                await Task.Delay(TimeSpan.FromSeconds(10f), cancellationToken);

                var response = new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)
                {
                    Content = new StringContent("You can retry this request.")
                };

                return response;
            }));
            
            var totalTimeoutPolicy = TimeoutFactory
                .Timeout<string>(TimeSpan.FromSeconds(3d));
            
            var retryPolicy = RetryFactory
                .RetryWithWait<string>(10, TimeSpan.FromSeconds(0.1f));
            
            var eachTimeoutPolicy = TimeoutFactory
                .Timeout<string>(TimeSpan.FromSeconds(0.5f));

            var policy = totalTimeoutPolicy
                .Wrap(retryPolicy)
                .Wrap(eachTimeoutPolicy);
            
            var result = await policy.ExecuteAsync(
                async cancellationToken => await MockWebAPI.GetAsync(httpClient, DummyUrl, cancellationToken),
                CancellationToken.None);

            if (result is IUncertainFailureResult<string> failure)
            {
                Debug.LogError(failure.Message);
            }
            
            result.Retryable.Should().BeTrue();
            // retryPolicy.RetryCount.Should().Be(5); // (0.5s(loop) * 0.1a(interval)) * 5 < 3s(total)
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
            
            var totalTimeoutPolicy = TimeoutFactory
                .Timeout<string>(TimeSpan.FromSeconds(5d));
            
            var retryPolicy = RetryFactory
                .Retry<string>(10);
            
            var circuitBreakerPolicy = CircuitBreakerFactory
                .CircuitBreaker<string>(3, TimeSpan.FromSeconds(3f));

            var policy = totalTimeoutPolicy
                .Wrap(retryPolicy)
                .Wrap(circuitBreakerPolicy);

            var result = await policy.ExecuteAsync(
                async cancellationToken => await MockWebAPI.GetAsync(httpClient, DummyUrl, cancellationToken),
                CancellationToken.None);

            if (result is IUncertainFailureResult<string> failure)
            {
                Debug.LogError(failure.Message);
            }
            
            result.Retryable.Should().BeTrue();
            retryPolicy.RetryCount.Should().Be(10,
                because: "Retry count is consumed immediately when circuit breaker is open.");
            circuitBreakerPolicy.State.Should().Be(CircuitState.Open);
        }
    }
}