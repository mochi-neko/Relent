#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Mochineko.Relent.Resilience.CircuitBreaker;
using Mochineko.Relent.UncertainResult;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Mochineko.Relent.Resilience.Tests
{
    [TestFixture]
    internal sealed class CircuitBreakerTest
    {
        [Test]
        [RequiresPlayMode(false)]
        public async Task PrimitiveCircuitBreakerWithNoValueTest()
        {
            var interval = TimeSpan.FromSeconds(0.1f);
            ICircuitBreakerPolicy policy = CircuitBreakerFactory
                .CircuitBreaker(
                    failureThreshold: 3,
                    interval);

            IUncertainResult result;

            policy.State.Should().Be(CircuitState.Closed,
                because: "Default state of circuit is closed.");

            result = await ExecuteAsSuccess(policy);

            result.Success.Should().BeTrue();
            policy.State.Should().Be(CircuitState.Closed,
                because: "Success result does not open circuit.");

            result = await ExecuteAsRetryable(policy);
            result = await ExecuteAsRetryable(policy);
            result.Retryable.Should().BeTrue();
            policy.State.Should().Be(CircuitState.Closed,
                because: "Circuit count is less than failureThreshold then circuit holds closed.");

            result = await ExecuteAsRetryable(policy);
            result.Retryable.Should().BeTrue();
            policy.State.Should().Be(CircuitState.Open,
                because: "Circuit count equals to failureThreshold then circuit is open.");

            result = await ExecuteAsSuccess(policy);
            result.Retryable.Should().BeTrue(because: "Circuit is open.");
            policy.State.Should().Be(CircuitState.Open);

            // Wait to change state to half-open.
            await Task.Delay(interval * 2);

            result = await ExecuteAsRetryable(policy);
            result.Retryable.Should().BeTrue();
            policy.State.Should().Be(CircuitState.Open,
                "If result is failure when is half-open then circuit is reopen.");

            // Wait to change state to half-open.
            await Task.Delay(interval * 2);

            result = await ExecuteAsSuccess(policy);
            result.Success.Should().BeTrue();
            policy.State.Should().Be(CircuitState.Closed,
                because: "If result is success when is half-open then circuit is closed.");

            policy.Isolate();
            policy.State.Should().Be(CircuitState.Isolated);

            result = await ExecuteAsSuccess(policy);
            result.Failure.Should().BeTrue(
                because: "Once circuit is isolated, circuit does not execute action.");

            result = await ExecuteAsSuccess(policy);
            result.Failure.Should().BeTrue();
            result = await ExecuteAsSuccess(policy);
            result.Failure.Should().BeTrue();
            result = await ExecuteAsSuccess(policy);
            result.Failure.Should().BeTrue();
            result = await ExecuteAsSuccess(policy);
            result.Failure.Should().BeTrue();
        }

        [Test]
        [RequiresPlayMode(false)]
        public async Task PrimitiveCircuitBreakerTest()
        {
            var interval = TimeSpan.FromSeconds(0.1f);
            ICircuitBreakerPolicy<bool> policy = CircuitBreakerFactory
                .CircuitBreaker<bool>(
                    failureThreshold: 3,
                    interval);

            IUncertainResult<bool> result;

            policy.State.Should().Be(CircuitState.Closed,
                because: "Default state of circuit is closed.");

            result = await ExecuteAsSuccess(policy, true);

            result.Success.Should().BeTrue();
            policy.State.Should().Be(CircuitState.Closed,
                because: "Success result does not open circuit.");

            result = await ExecuteAsRetryable(policy);
            result = await ExecuteAsRetryable(policy);
            result.Retryable.Should().BeTrue();
            policy.State.Should().Be(CircuitState.Closed,
                because: "Circuit count is less than failureThreshold then circuit holds closed.");

            result = await ExecuteAsRetryable(policy);
            result.Retryable.Should().BeTrue();
            policy.State.Should().Be(CircuitState.Open,
                because: "Circuit count equals to failureThreshold then circuit is open.");

            result = await ExecuteAsSuccess(policy, true);
            result.Retryable.Should().BeTrue(because: "Circuit is open.");
            policy.State.Should().Be(CircuitState.Open);

            // Wait to change state to half-open.
            await Task.Delay(interval * 2);

            result = await ExecuteAsRetryable(policy);
            result.Retryable.Should().BeTrue();
            policy.State.Should().Be(CircuitState.Open,
                "If result is failure when is half-open then circuit is reopen.");

            // Wait to change state to half-open.
            await Task.Delay(interval * 2);

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

        private static async Task<IUncertainResult> ExecuteAsRetryable(
            ICircuitBreakerPolicy policy)
        {
            return await policy.ExecuteAsync(
                execute: _ => Task.FromResult<IUncertainResult>(
                    UncertainResultExtensions.RetryWithTrace("Force retry")),
                cancellationToken: CancellationToken.None);
        }

        private static async Task<IUncertainResult<TResult>> ExecuteAsRetryable<TResult>(
            ICircuitBreakerPolicy<TResult> policy)
        {
            return await policy.ExecuteAsync(
                execute: _ => Task.FromResult<IUncertainResult<TResult>>(
                    UncertainResultExtensions.RetryWithTrace<TResult>("Force retry")),
                cancellationToken: CancellationToken.None);
        }

        private static async Task<IUncertainResult> ExecuteAsSuccess(
            ICircuitBreakerPolicy policy)
        {
            return await policy.ExecuteAsync(
                execute: _ => Task.FromResult<IUncertainResult>(
                    UncertainResultFactory.Succeed()),
                cancellationToken: CancellationToken.None);
        }

        private static async Task<IUncertainResult<TResult>> ExecuteAsSuccess<TResult>(
            ICircuitBreakerPolicy<TResult> policy,
            TResult resultValue)
        {
            return await policy.ExecuteAsync(
                execute: _ => Task.FromResult<IUncertainResult<TResult>>(
                    UncertainResultFactory.Succeed(resultValue)),
                cancellationToken: CancellationToken.None);
        }
    }
}