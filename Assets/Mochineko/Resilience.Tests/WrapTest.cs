#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Mochineko.Resilience.CircuitBreaker;
using Mochineko.Resilience.Retry;
using Mochineko.Resilience.Timeout;
using Mochineko.Resilience.Wrap;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Mochineko.Resilience.Tests
{
    [TestFixture]
    internal sealed class WrapTest
    {
        [Test]
        [RequiresPlayMode(false)]
        public async Task WrapRetryAndTimeoutTest()
        {
            var totalTimeout = TimeoutFactory.Timeout<string>(TimeSpan.FromSeconds(0.5d));
            var retry = RetryFactory.Retry<string>(10);
            var eachTimeout = TimeoutFactory.Timeout<string>(TimeSpan.FromSeconds(0.1d));

            var policy = totalTimeout
                .Wrap(retry)
                .Wrap(eachTimeout);

            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            
            var result = await policy.ExecuteAsync(
                execute: cancellationToken => WaitUtility.WaitAndRetry<string>(
                    TimeSpan.FromSeconds(0.5f), // wait over timeout
                    cancellationToken),
                cancellationToken: CancellationToken.None);

            stopwatch.Stop();
            
            result.Retryable.Should().BeTrue();
            retry.RetryCount.Should().Be(5); // 5 = 0.5s(total) / 0.1s(each)
            stopwatch.ElapsedMilliseconds.Should().BeGreaterOrEqualTo((long)(1000 * 0.1d * 5));
        }

        [Test]
        [RequiresPlayMode(false)]
        public async Task WrapStrategyTest()
        {
            var totalTimeout = TimeoutFactory.Timeout<string>(TimeSpan.FromSeconds(0.5d));
            var retry = RetryFactory.Retry<string>(10);
            var circuitBreaker = CircuitBreakerFactory.CircuitBreaker<string>(3, TimeSpan.FromSeconds(1d));
            var eachTimeout = TimeoutFactory.Timeout<string>(TimeSpan.FromSeconds(0.1d));

            var policy = totalTimeout
                .Wrap(retry)
                .Wrap(circuitBreaker)
                .Wrap(eachTimeout);

            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();

            var result = await policy.ExecuteAsync(
                execute: cancellationToken => WaitUtility.WaitAndSucceed<string>(
                    TimeSpan.FromSeconds(0.5f), // wait over timeout
                    cancellationToken,
                    "Success"),
                cancellationToken: CancellationToken.None);

            stopWatch.Stop();

            result.Retryable.Should().BeTrue();
            retry.RetryCount.Should().Be(10,
                because: "CircuitBreaker is open by each timeout then retry count is consumed immediately.");
            circuitBreaker.State.Should().Be(CircuitState.Open,
                because: "Circuit is broken by each timeout.");
            stopWatch.ElapsedMilliseconds.Should().BeGreaterOrEqualTo((long)(1000 * 0.3d),
                because: "Circuit round equals to each timeout * circuit failure threshold.");
            stopWatch.ElapsedMilliseconds.Should().BeLessThan((long)(1000 * 0.5d),
                because: "Does not reach total timeout.");
        }
    }
}