#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Mochineko.Resilience.Timeout;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Mochineko.Resilience.Tests
{
    [TestFixture]
    internal sealed class TimeoutTest
    {
        [TestCase(0f)]
        [TestCase(0.1f)]
        [TestCase(0.5f)]
        [TestCase(1f)]
        [RequiresPlayMode(false)]
        public async Task PrimitiveTimeoutTest(float timeout)
        {
            ITimeoutPolicy<int> policy = TimeoutFactory.Timeout<int>(TimeSpan.FromSeconds(timeout));

            var result = await policy.ExecuteAsync(
                execute: cancellationToken => WaitUtility.WaitAsUncertain(
                    TimeSpan.FromSeconds(timeout + 10), // wait over timeout
                    cancellationToken,
                    1),
                cancellationToken: CancellationToken.None);

            result.Success.Should().BeFalse();
            result.Failure.Should().BeTrue();
        }
    }
}