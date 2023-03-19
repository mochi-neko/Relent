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
            using var timeoutCancellationTokenSource = new CancellationTokenSource(timeout);
            using var linkedCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(
                cancellationToken,
                timeoutCancellationTokenSource.Token);

            var result = await execute(linkedCancellationTokenSource.Token);
            if (result is IUncertainSuccessResult success)
            {
                return success;
            }
            else if (result is IUncertainRetryableResult retryable)
            {
                return UncertainResultFactory.Retry(
                    $"Retryable because result was retryable or timeout -> {retryable.Message}.");
            }
            else if (result is IUncertainFailureResult failure)
            {
                return UncertainResultFactory.Fail(
                    $"Failed because result was failure or timeout -> {failure.Message}.");
            }
            else
            {
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
            using var timeoutCancellationTokenSource = new CancellationTokenSource(timeout);
            using var linkedCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(
                cancellationToken,
                timeoutCancellationTokenSource.Token);

            var result = await execute(linkedCancellationTokenSource.Token);
            if (result is IUncertainSuccessResult<TResult> success)
            {
                return success;
            }
            else if (result is IUncertainRetryableResult<TResult> retryable)
            {
                return UncertainResultFactory.Retry<TResult>(
                    $"Retryable because result was retryable or timeout -> {retryable.Message}.");
            }
            else if (result is IUncertainFailureResult<TResult> failure)
            {
                return UncertainResultFactory.Fail<TResult>(
                    $"Failed because result was failure or timeout -> {failure.Message}.");
            }
            else
            {
                // Panic!
                throw new UncertainResultPatternMatchException(nameof(result));
            }
        }
    }
}