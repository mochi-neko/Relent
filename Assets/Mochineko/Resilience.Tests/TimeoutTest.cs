#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Mochineko.UncertainResult;
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
            IPolicy<int> policy = TimeoutFactory.Timeout<int>(TimeSpan.FromSeconds(timeout));

            var result = await policy.ExecuteAsync(
                execute: cancellationToken => WaitAsUncertain(
                    TimeSpan.FromSeconds(timeout + 10), // wait over timeout
                    cancellationToken,
                    1),
                cancellationToken: CancellationToken.None);

            result.Success.Should().BeFalse();
            result.Failure.Should().BeTrue();
        }

        private static async Task<IUncertainResult<TResult>> WaitAsUncertain<TResult>(
            TimeSpan waitTime,
            CancellationToken cancellationToken,
            TResult successResult)
        {
            try
            {
                await Task.Delay(waitTime, cancellationToken);

                return UncertainResultFactory.Succeed(successResult);
            }
            catch (OperationCanceledException exception)
            {
                return UncertainResultFactory.Retry<TResult>(
                    $"Cancelled to wait because operation was cancelled with exception:{exception}.");
            }
            catch (Exception exception)
            {
                return UncertainResultFactory.Fail<TResult>(
                    $"Cancelled to wait because of unhandled exception:{exception}.");
            }
        }
    }
}