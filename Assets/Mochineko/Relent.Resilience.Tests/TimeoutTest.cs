#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Mochineko.Relent.Resilience.Timeout;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Mochineko.Relent.Resilience.Tests
{
    [TestFixture]
    internal sealed class TimeoutTest
    {
        [TestCase(0f)]
        [TestCase(0.01f)]
        [TestCase(0.05f)]
        [TestCase(0.1f)]
        [RequiresPlayMode(false)]
        public async Task PrimitiveTimeoutWithNoValueTest(float timeout)
        {
            ITimeoutPolicy policy = TimeoutFactory.Timeout(TimeSpan.FromSeconds(timeout));

            var result = await policy.ExecuteAsync(
                execute: cancellationToken => WaitUtility.WaitAndSucceed(
                    TimeSpan.FromSeconds(timeout + 10), // wait over timeout
                    cancellationToken),
                cancellationToken: CancellationToken.None);

            result.Retryable.Should().BeTrue();
        }

        [TestCase(0f)]
        [TestCase(0.01f)]
        [TestCase(0.05f)]
        [TestCase(0.1f)]
        [RequiresPlayMode(false)]
        public async Task PrimitiveTimeoutTest(float timeout)
        {
            ITimeoutPolicy<int> policy = TimeoutFactory.Timeout<int>(TimeSpan.FromSeconds(timeout));

            var result = await policy.ExecuteAsync(
                execute: cancellationToken => WaitUtility.WaitAndSucceed(
                    TimeSpan.FromSeconds(timeout + 10), // wait over timeout
                    cancellationToken,
                    1),
                cancellationToken: CancellationToken.None);

            result.Retryable.Should().BeTrue();
        }
    }
}