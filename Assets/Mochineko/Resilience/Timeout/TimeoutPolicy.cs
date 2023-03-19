#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using Mochineko.Result;
using Mochineko.UncertainResult;

namespace Mochineko.Resilience.Timeout
{
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
                return UncertainResultFactory.Succeed(success.Result);
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