#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Mochineko.Resilience.Retry;
using Mochineko.UncertainResult;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Mochineko.Resilience.Tests
{
    [TestFixture]
    internal sealed class RetryTest
    {
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(5)]
        [TestCase(10)]
        [TestCase(20)]
        [RequiresPlayMode(false)]
        public async Task PrimitiveRetryTest(int maxRetryCount)
        {
            IRetryPolicy<bool> policy = RetryFactory.Retry<bool>(maxRetryCount);

            Task<IUncertainResult<bool>> ForceRetry(CancellationToken cancellationToken)
                => Task.FromResult<IUncertainResult<bool>>(UncertainResultFactory.Retry<bool>("Force retry."));

            var result = await policy.ExecuteAsync(
                execute: ForceRetry,
                CancellationToken.None);

            result.Retryable.Should().BeTrue();
            policy.RetryCount.Should().Be(maxRetryCount);
        }
        
        [TestCase(0, 0.1f)]
        [TestCase(1, 0.2f)]
        [TestCase(2, 0.2f)]
        [TestCase(5, 0.1f)]
        [TestCase(10, 0.05f)]
        [TestCase(20, 0.01f)]
        [RequiresPlayMode(false)]
        public async Task LinearTimeRetryTest(int maxRetryCount, float intervalSeconds)
        {
            IRetryPolicy<bool> policy = RetryFactory.RetryWithWait<bool>(
                maxRetryCount,
                interval:TimeSpan.FromSeconds(intervalSeconds));

            Task<IUncertainResult<bool>> ForceRetry(CancellationToken cancellationToken)
                => Task.FromResult<IUncertainResult<bool>>(UncertainResultFactory.Retry<bool>("Force retry."));

            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();
            
            var result = await policy.ExecuteAsync(
                execute: ForceRetry,
                CancellationToken.None);
            
            stopWatch.Stop();

            result.Retryable.Should().BeTrue();
            policy.RetryCount.Should().Be(maxRetryCount);
            stopWatch.ElapsedMilliseconds.Should().BeGreaterOrEqualTo(
                (long)(intervalSeconds * 1000 * maxRetryCount));
        }
        
        [TestCase(0, 2d)]
        [TestCase(1, 2d)]
        [TestCase(2, 2d)]
        [TestCase(5, 1.1d)]
        [RequiresPlayMode(false)]
        public async Task RetryWithExponentialBackoffTest(int maxRetryCount, double baseNumber)
        {
            IRetryPolicy<bool> policy = RetryFactory.RetryWithExponentialBackoff<bool>(maxRetryCount, baseNumber);

            Task<IUncertainResult<bool>> ForceRetry(CancellationToken cancellationToken)
                => Task.FromResult<IUncertainResult<bool>>(UncertainResultFactory.Retry<bool>("Force retry."));

            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();
            
            var result = await policy.ExecuteAsync(
                execute: ForceRetry,
                CancellationToken.None);
            
            stopWatch.Stop();

            result.Retryable.Should().BeTrue();
            policy.RetryCount.Should().Be(maxRetryCount);
            stopWatch.ElapsedMilliseconds.Should().BeGreaterOrEqualTo(
                (long)(1000 * IntegrateExponentialBackoffDuration(maxRetryCount, baseNumber)));
        }
        
        private static double IntegrateExponentialBackoffDuration(int retryCount, double baseNumber)
        {
            var duration = 0d;
            for (var i = 0; i < retryCount; i++)
            {
                duration += Math.Pow(baseNumber, i);
            }

            return duration;
        }
        
        [TestCase(0, 0.1d, 1d)]
        [TestCase(1, 0.1d, 2d)]
        [TestCase(2, 0.1d, 1d)]
        [TestCase(5, 0.01d, 0.5d)]
        [RequiresPlayMode(false)]
        public async Task RetryWithJitterTest(int maxRetryCount, double minimum, double maximum)
        {
            IRetryPolicy<bool> policy = RetryFactory.RetryWithJitter<bool>(maxRetryCount, minimum, maximum);

            Task<IUncertainResult<bool>> ForceRetry(CancellationToken cancellationToken)
                => Task.FromResult<IUncertainResult<bool>>(UncertainResultFactory.Retry<bool>("Force retry."));

            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();
            
            var result = await policy.ExecuteAsync(
                execute: ForceRetry,
                CancellationToken.None);
            
            stopWatch.Stop();

            result.Retryable.Should().BeTrue();
            policy.RetryCount.Should().Be(maxRetryCount);
            stopWatch.ElapsedMilliseconds.Should().BeGreaterOrEqualTo(
                (long)(1000 * minimum * maxRetryCount));
        }
    }
}