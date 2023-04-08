#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using Mochineko.Relent.UncertainResult;

namespace Mochineko.Relent.Resilience.Timeout
{
    internal sealed class TimeoutPolicy
        : ITimeoutPolicy
    {
        private readonly TimeSpan timeout;

        public TimeoutPolicy(TimeSpan timeout)
        {
            this.timeout = timeout;
        }

        public async Task<IUncertainResult> ExecuteAsync(
            Func<CancellationToken, Task<IUncertainResult>> execute,
            CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return UncertainResults.RetryWithTrace(
                    $"Cancelled before retry because of {nameof(cancellationToken)} is cancelled.");
            }

            using var timeoutCancellationTokenSource = new CancellationTokenSource(timeout);
            using var linkedCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(
                cancellationToken,
                timeoutCancellationTokenSource.Token);

            var result = await execute(linkedCancellationTokenSource.Token);
            var reason = timeoutCancellationTokenSource.IsCancellationRequested
                ? "timeout"
                : "retryable";
            switch (result)
            {
                case IUncertainSuccessResult success:
                    return success;

                case IUncertainTraceRetryableResult traceRetryable:
                    return traceRetryable.Trace(
                        $"Retryable timeout because result was {reason}.");

                case IUncertainRetryableResult retryable:
                    return UncertainResults.RetryWithTrace(
                        $"Retryable timeout because result was {reason} -> {retryable.Message}.");

                case IUncertainTraceFailureResult traceFailure:
                    return traceFailure.Trace(
                        $"Failed timeout because result was failure or timeout.");

                case IUncertainFailureResult failure:
                    return UncertainResults.FailWithTrace(
                        $"Failed timeout because -> {failure.Message}.");

                default:
                    // Panic!
                    throw new UncertainResultPatternMatchException(nameof(result));
            }
        }
    }

    internal sealed class TimeoutPolicy<TResult>
        : ITimeoutPolicy<TResult>
    {
        private readonly TimeSpan timeout;

        public TimeoutPolicy(TimeSpan timeout)
        {
            this.timeout = timeout;
        }

        public async Task<IUncertainResult<TResult>> ExecuteAsync(
            Func<CancellationToken, Task<IUncertainResult<TResult>>> execute,
            CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return UncertainResults.RetryWithTrace<TResult>(
                    $"Cancelled before retry because of {nameof(cancellationToken)} is cancelled.");
            }

            using var timeoutCancellationTokenSource = new CancellationTokenSource(timeout);
            using var linkedCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(
                cancellationToken,
                timeoutCancellationTokenSource.Token);

            var result = await execute(linkedCancellationTokenSource.Token);
            var reason = timeoutCancellationTokenSource.IsCancellationRequested
                ? "timeout"
                : "retryable";
            switch (result)
            {
                case IUncertainSuccessResult<TResult> success:
                    return success;

                case IUncertainTraceRetryableResult<TResult> traceRetryable:
                    return traceRetryable.Trace(
                        $"Retryable timeout because result was {reason}.");

                case IUncertainRetryableResult<TResult> retryable:
                    return UncertainResults.Retry<TResult>(
                        $"Retryable timeout because result was {reason} -> {retryable.Message}.");

                case IUncertainFailureResult<TResult> failure:
                    return UncertainResults.Fail<TResult>(
                        $"Failed timeout because -> {failure.Message}.");

                default:
                    // Panic!
                    throw new UncertainResultPatternMatchException(nameof(result));
            }
        }
    }
}