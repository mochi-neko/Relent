#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Mochineko.Relent.Resilience.CircuitBreaker;
using Mochineko.Relent.Resilience.Retry;
using Mochineko.Relent.Resilience.Timeout;
using Mochineko.Relent.Resilience.Wrap;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Mochineko.Relent.Resilience.Tests
{
    [TestFixture]
    internal sealed class WrapTest
    {
        [Test]
        [RequiresPlayMode(false)]
        public async Task WrapRetryAndTimeoutWithNoValueTest()
        {
            var totalTimeout = TimeoutFactory.Timeout(TimeSpan.FromSeconds(0.35d));
            var retry = RetryFactory.Retry(10);
            var eachTimeout = TimeoutFactory.Timeout(TimeSpan.FromSeconds(0.1d));

            var policy = totalTimeout
                .Wrap(retry)
                .Wrap(eachTimeout);

            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            
            var result = await policy.ExecuteAsync(
                execute: cancellationToken => WaitUtility.WaitAndRetry(
                    TimeSpan.FromSeconds(10f), // wait over timeout
                    cancellationToken),
                cancellationToken: CancellationToken.None);

            stopwatch.Stop();
            
            result.Retryable.Should().BeTrue();
            stopwatch.ElapsedMilliseconds.Should().BeGreaterOrEqualTo((long)(1000 * 0.3d));
        }
        
        [Test]
        [RequiresPlayMode(false)]
        public async Task WrapRetryAndTimeoutTest()
        {
            var totalTimeout = TimeoutFactory.Timeout<string>(TimeSpan.FromSeconds(0.35d));
            var retry = RetryFactory.Retry<string>(10);
            var eachTimeout = TimeoutFactory.Timeout<string>(TimeSpan.FromSeconds(0.1d));

            var policy = totalTimeout
                .Wrap(retry)
                .Wrap(eachTimeout);

            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            
            var result = await policy.ExecuteAsync(
                execute: cancellationToken => WaitUtility.WaitAndRetry<string>(
                    TimeSpan.FromSeconds(10f), // wait over timeout
                    cancellationToken),
                cancellationToken: CancellationToken.None);

            stopwatch.Stop();
            
            result.Retryable.Should().BeTrue();
            stopwatch.ElapsedMilliseconds.Should().BeGreaterOrEqualTo((long)(1000 * 0.3d));
        }

        [Test]
        [RequiresPlayMode(false)]
        public async Task WrapStrategyWithNoValueTest()
        {
            var totalTimeout = TimeoutFactory.Timeout(TimeSpan.FromSeconds(0.35d));
            var retry = RetryFactory.Retry(10);
            var circuitBreaker = CircuitBreakerFactory.CircuitBreaker(3, TimeSpan.FromSeconds(0.1d));
            var eachTimeout = TimeoutFactory.Timeout(TimeSpan.FromSeconds(0.01d));

            var policy = totalTimeout
                .Wrap(retry)
                .Wrap(circuitBreaker)
                .Wrap(eachTimeout);

            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();

            var result = await policy.ExecuteAsync(
                execute: cancellationToken => WaitUtility.WaitAndSucceed(
                    TimeSpan.FromSeconds(10f), // wait over timeout
                    cancellationToken),
                cancellationToken: CancellationToken.None);

            stopWatch.Stop();

            result.Retryable.Should().BeTrue();
            retry.RetryCount.Should().Be(10,
                because: "CircuitBreaker is open by each timeout then retry count is consumed immediately.");
            circuitBreaker.State.Should().Be(CircuitState.Open,
                because: "Circuit is broken by each timeout.");
            stopWatch.ElapsedMilliseconds.Should().BeGreaterOrEqualTo((long)(1000 * 0.03d),
                because: "Circuit round equals to each timeout * circuit failure threshold.");
        }
        
        [Test]
        [RequiresPlayMode(false)]
        public async Task WrapStrategyTest()
        {
            var totalTimeout = TimeoutFactory.Timeout<string>(TimeSpan.FromSeconds(0.35d));
            var retry = RetryFactory.Retry<string>(10);
            var circuitBreaker = CircuitBreakerFactory.CircuitBreaker<string>(3, TimeSpan.FromSeconds(0.1d));
            var eachTimeout = TimeoutFactory.Timeout<string>(TimeSpan.FromSeconds(0.01d));

            var policy = totalTimeout
                .Wrap(retry)
                .Wrap(circuitBreaker)
                .Wrap(eachTimeout);

            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();

            var result = await policy.ExecuteAsync(
                execute: cancellationToken => WaitUtility.WaitAndSucceed<string>(
                    TimeSpan.FromSeconds(10f), // wait over timeout
                    cancellationToken,
                    "Success"),
                cancellationToken: CancellationToken.None);

            stopWatch.Stop();

            result.Retryable.Should().BeTrue();
            retry.RetryCount.Should().Be(10,
                because: "CircuitBreaker is open by each timeout then retry count is consumed immediately.");
            circuitBreaker.State.Should().Be(CircuitState.Open,
                because: "Circuit is broken by each timeout.");
            stopWatch.ElapsedMilliseconds.Should().BeGreaterOrEqualTo((long)(1000 * 0.03d),
                because: "Circuit round equals to each timeout * circuit failure threshold.");
        }
    }
}