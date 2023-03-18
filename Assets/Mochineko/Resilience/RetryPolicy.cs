#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using Mochineko.Result;
using Mochineko.UncertainResult;

namespace Mochineko.Resilience
{
    internal sealed class RetryPolicy<TResult>
        : IPolicy<TResult>
    {
        private readonly int permittedRetryCount;
        private readonly Func<int, TimeSpan> waitDurationProvider;

        public RetryPolicy(int permittedRetryCount)
        {
            this.permittedRetryCount = permittedRetryCount;
            this.waitDurationProvider = retryAttempt => TimeSpan.Zero;
        }
        
        public RetryPolicy(int permittedRetryCount, TimeSpan duration)
        {
            this.permittedRetryCount = permittedRetryCount;
            this.waitDurationProvider = retryAttempt => duration;
        }
        
        public RetryPolicy(int permittedRetryCount, Func<int, TimeSpan> durationProvider)
        {
            this.permittedRetryCount = permittedRetryCount;
            this.waitDurationProvider = durationProvider ?? throw new ArgumentNullException(nameof(durationProvider));
        }

        public async Task<IResult<TResult>> ExecuteAsync(
            Func<CancellationToken, Task<IUncertainResult<TResult>>> execute,
            CancellationToken cancellationToken)
        {
            var retryCount = 0;

            while (retryCount < permittedRetryCount)
            {
                var result = await execute.Invoke(cancellationToken);
                if (result is IUncertainSuccessResult<TResult> success)
                {
                    return global::Mochineko.Result.Result.Succeed(success.Result);
                }
                else if (result is IUncertainRetryableResult<TResult> retryable)
                {
                    retryCount++;

                    try
                    {
                        await Task.Delay(waitDurationProvider.Invoke(retryCount), cancellationToken);
                    }
                    catch (OperationCanceledException exception)
                    {
                        return global::Mochineko.Result.Result.Fail<TResult>(
                            $"Failed to retry because operation was cancelled with exception:{exception}.");
                    }
                    catch (Exception exception)
                    {
                        // Unexpected
                        return global::Mochineko.Result.Result.Fail<TResult>(
                            $"Failed to retry because operation was failed by unhandled exception:{exception}.");
                    }
                }
                else if (result is IUncertainFailureResult<TResult> failure)
                {
                    return global::Mochineko.Result.Result.Fail<TResult>(failure.Message);
                }
                else
                {
                    // Unexpected
                    throw new UncertainResultPatternMatchException(nameof(result));
                }
            }
            
            // Over permitted retry count
            return global::Mochineko.Result.Result.Fail<TResult>(
                "Failed to retry because retry count was over permitted count.");
        }
    }
}