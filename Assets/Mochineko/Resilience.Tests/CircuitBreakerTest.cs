#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Mochineko.Result;
using Mochineko.UncertainResult;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Mochineko.Resilience.Tests
{
    [TestFixture]
    internal sealed class CircuitBreakerTest
    {
        [Test]
        [RequiresPlayMode(false)]
        public async Task PrimitiveCircuitBreakerTest()
        {
            var breakTime = TimeSpan.FromSeconds(0.1f);
            ICircuitBreakerPolicy<bool> policy = CircuitBreakerFactory
                .CircuitBreaker<bool>(
                    failureThreshold: 3,
                    breakTime);

            IResult<bool> result;

            policy.State.Should().Be(CircuitState.Closed,
                because: "Default state of circuit is closed.");

            result = await ExecuteAsSuccess(policy, true);

            result.Success.Should().BeTrue();
            policy.State.Should().Be(CircuitState.Closed,
                because: "Success result does not open circuit.");

            result = await ExecuteAsRetryable(policy);
            result = await ExecuteAsRetryable(policy);
            result.Failure.Should().BeTrue();
            policy.State.Should().Be(CircuitState.Closed,
                because: "Circuit count is less than failureThreshold then circuit holds closed.");

            result = await ExecuteAsRetryable(policy);
            result.Failure.Should().BeTrue();
            policy.State.Should().Be(CircuitState.Open,
                because: "Circuit count equals to failureThreshold then circuit is open.");

            result = await ExecuteAsSuccess(policy, true);
            result.Failure.Should().BeTrue(because: "Circuit is open.");
            policy.State.Should().Be(CircuitState.Open);

            // Wait to change state to half-open.
            await Task.Delay(breakTime * 2);

            result = await ExecuteAsRetryable(policy);
            result.Failure.Should().BeTrue();
            policy.State.Should().Be(CircuitState.Open,
                "If result is failure when is half-open then circuit is reopen.");

            // Wait to change state to half-open.
            await Task.Delay(breakTime * 2);

            result = await ExecuteAsSuccess(policy, true);
            result.Success.Should().BeTrue();
            policy.State.Should().Be(CircuitState.Closed,
                because: "If result is success when is half-open then circuit is closed.");

            policy.Isolate();
            policy.State.Should().Be(CircuitState.Isolated);

            result = await ExecuteAsSuccess(policy, true);
            result.Failure.Should().BeTrue(
                because: "Once circuit is isolated, circuit does not execute action.");
            
            result = await ExecuteAsSuccess(policy, true);
            result.Failure.Should().BeTrue();
            result = await ExecuteAsSuccess(policy, true);
            result.Failure.Should().BeTrue();
            result = await ExecuteAsSuccess(policy, true);
            result.Failure.Should().BeTrue();
            result = await ExecuteAsSuccess(policy, true);
            result.Failure.Should().BeTrue();
        }

        private static async Task<IResult<TResult>> ExecuteAsRetryable<TResult>(
            ICircuitBreakerPolicy<TResult> policy)
        {
            return await policy.ExecuteAsync(
                execute: _ => Task.FromResult<IUncertainResult<TResult>>(
                    UncertainResultFactory.Retry<TResult>("Force retry")),
                cancellationToken: CancellationToken.None);
        }

        private static async Task<IResult<TResult>> ExecuteAsSuccess<TResult>(
            ICircuitBreakerPolicy<TResult> policy,
            TResult resultValue)
        {
            return await policy.ExecuteAsync(
                execute: _ => Task.FromResult<IUncertainResult<TResult>>(
                    UncertainResultFactory.Succeed<TResult>(resultValue)),
                cancellationToken: CancellationToken.None);
        }
    }
}