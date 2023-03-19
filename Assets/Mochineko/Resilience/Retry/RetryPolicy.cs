#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using Mochineko.UncertainResult;

namespace Mochineko.Resilience.Retry
{
    internal sealed class RetryPolicy
        : IRetryPolicy
    {
        private readonly int maxRetryCount;
        private readonly Func<int, TimeSpan> intervalProvider;
        
        private int retryCount;
        public int RetryCount => retryCount;

        public RetryPolicy(int maxRetryCount)
        {
            this.maxRetryCount = maxRetryCount;
            intervalProvider = _ => TimeSpan.Zero;
        }

        public RetryPolicy(int maxRetryCount, TimeSpan interval)
        {
            this.maxRetryCount = maxRetryCount;
            intervalProvider = _ => interval;
        }

        public RetryPolicy(int maxRetryCount, Func<int, TimeSpan> intervalProvider)
        {
            this.maxRetryCount = maxRetryCount;
            this.intervalProvider = intervalProvider ?? throw new ArgumentNullException(nameof(intervalProvider));
        }

        public async Task<IUncertainResult> ExecuteAsync(
            Func<CancellationToken, Task<IUncertainResult>> execute,
            CancellationToken cancellationToken)
        {
            retryCount = 0;

            while (retryCount < maxRetryCount)
            {
                var result = await execute.Invoke(cancellationToken);
                if (result is IUncertainSuccessResult success)
                {
                    return success;
                }
                else if (result is IUncertainRetryableResult retryable)
                {
                    retryCount++;

                    var intervalResult = await WaitUtility
                        .WaitAsync(intervalProvider.Invoke(retryCount), cancellationToken);
                    if (intervalResult is IUncertainRetryableResult cancelled)
                    {
                        return UncertainResultFactory.Retry(
                            $"Cancelled in interval at {retryCount}th retry -> {cancelled.Message}");
                    }
                    else if (intervalResult is IUncertainFailureResult failure)
                    {
                        return UncertainResultFactory.Fail(
                            $"Failed to wait interval because -> {failure.Message}");
                    }
                }
                else if (result is IUncertainFailureResult failure)
                {
                    return UncertainResultFactory.Fail(
                        $"Failed to retry at {retryCount}th retry because -> {failure.Message}");
                }
                else
                {
                    // Panic!
                    throw new UncertainResultPatternMatchException(nameof(result));
                }
            }

            // Over max retry count
            return UncertainResultFactory.Retry(
                $"Retryable because retry count was over max count:{maxRetryCount}.");
        }
    }
    
    internal sealed class RetryPolicy<TResult>
        : IRetryPolicy<TResult>
    {
        private readonly int maxRetryCount;
        private readonly Func<int, TimeSpan> intervalProvider;
        
        private int retryCount;
        public int RetryCount => retryCount;

        public RetryPolicy(int maxRetryCount)
        {
            this.maxRetryCount = maxRetryCount;
            intervalProvider = _ => TimeSpan.Zero;
        }

        public RetryPolicy(int maxRetryCount, TimeSpan interval)
        {
            this.maxRetryCount = maxRetryCount;
            intervalProvider = _ => interval;
        }

        public RetryPolicy(int maxRetryCount, Func<int, TimeSpan> intervalProvider)
        {
            this.maxRetryCount = maxRetryCount;
            this.intervalProvider = intervalProvider ?? throw new ArgumentNullException(nameof(intervalProvider));
        }

        public async Task<IUncertainResult<TResult>> ExecuteAsync(
            Func<CancellationToken, Task<IUncertainResult<TResult>>> execute,
            CancellationToken cancellationToken)
        {
            retryCount = 0;

            while (retryCount < maxRetryCount)
            {
                var result = await execute.Invoke(cancellationToken);
                if (result is IUncertainSuccessResult<TResult> success)
                {
                    return success;
                }
                else if (result is IUncertainRetryableResult<TResult> retryable)
                {
                    retryCount++;

                    var intervalResult = await WaitUtility
                        .WaitAsync(intervalProvider.Invoke(retryCount), cancellationToken);
                    if (intervalResult is IUncertainRetryableResult cancelled)
                    {
                        return UncertainResultFactory.Retry<TResult>(
                            $"Cancelled in interval at {retryCount}th retry -> {cancelled.Message}");
                    }
                    else if (intervalResult is IUncertainFailureResult failure)
                    {
                        return UncertainResultFactory.Fail<TResult>(
                            $"Failed to wait interval because -> {failure.Message}");
                    }
                }
                else if (result is IUncertainFailureResult<TResult> failure)
                {
                    return UncertainResultFactory.Fail<TResult>(
                        $"Failed to retry at {retryCount}th retry because -> {failure.Message}");
                }
                else
                {
                    // Panic!
                    throw new UncertainResultPatternMatchException(nameof(result));
                }
            }

            // Over max retry count
            return UncertainResultFactory.Retry<TResult>(
                $"Retryable because retry count was over max count:{maxRetryCount}.");
        }
    }
}