#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using Mochineko.Relent.UncertainResult;

namespace Mochineko.Relent.Resilience.Retry
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
            if (cancellationToken.IsCancellationRequested)
            {
                return UncertainResults.RetryWithTrace(
                    $"Cancelled before retry because of {nameof(cancellationToken)} is cancelled.");
            }
            
            retryCount = 0;

            while (retryCount < maxRetryCount)
            {
                var result = await execute.Invoke(cancellationToken);
                switch (result)
                {
                    case IUncertainSuccessResult success:
                        return success;

                    case IUncertainTraceRetryableResult traceRetryable:
                    {
                        retryCount++;

                        var intervalResult = await WaitUtility
                            .WaitAsync(intervalProvider.Invoke(retryCount), cancellationToken);
                        if (intervalResult is IUncertainRetryableResult cancelled)
                        {
                            return traceRetryable.Trace(
                                $"Cancelled in interval at {retryCount}th retry because of {cancelled.Message}");
                        }

                        break;
                    }

                    case IUncertainRetryableResult:
                    {
                        retryCount++;

                        var intervalResult = await WaitUtility
                            .WaitAsync(intervalProvider.Invoke(retryCount), cancellationToken);
                        if (intervalResult is IUncertainTraceRetryableResult cancelled)
                        {
                            return cancelled.Trace(
                                $"Cancelled in interval at {retryCount}th retry.");
                        }

                        break;
                    }

                    case IUncertainTraceFailureResult traceFailure:
                        return traceFailure.Trace($"Failed to retry at {retryCount}th retry.");

                    case IUncertainFailureResult failure:
                        return UncertainResults.FailWithTrace(
                            $"Failed to retry at {retryCount}th retry because -> {failure.Message}");

                    default:
                        // Panic!
                        throw new UncertainResultPatternMatchException(nameof(result));
                }
            }

            // Over max retry count
            return UncertainResults.RetryWithTrace(
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
            if (cancellationToken.IsCancellationRequested)
            {
                return UncertainResults.RetryWithTrace<TResult>(
                    $"Cancelled before retry because of {nameof(cancellationToken)} is cancelled.");
            }
            
            retryCount = 0;

            while (retryCount < maxRetryCount)
            {
                var result = await execute.Invoke(cancellationToken);
                switch (result)
                {
                    case IUncertainSuccessResult<TResult> success:
                        return success;

                    case IUncertainTraceRetryableResult<TResult> traceRetryable:
                    {
                        retryCount++;

                        var intervalResult = await WaitUtility
                            .WaitAsync(intervalProvider.Invoke(retryCount), cancellationToken);
                        if (intervalResult is IUncertainTraceRetryableResult cancelled)
                        {
                            return traceRetryable.Trace(
                                $"Cancelled in interval at {retryCount}th retry because of {cancelled.Message}.");
                        }

                        break;
                    }

                    case IUncertainRetryableResult<TResult>:
                    {
                        retryCount++;

                        var intervalResult = await WaitUtility
                            .WaitAsync(intervalProvider.Invoke(retryCount), cancellationToken);
                        if (intervalResult is IUncertainTraceRetryableResult cancelled)
                        {
                            return UncertainResults.RetryWithTrace<TResult>(
                                $"Cancelled in interval at {retryCount}th retry -> {cancelled.Message}.");
                        }

                        break;
                    }

                    case IUncertainTraceFailureResult<TResult> traceFailure:
                        return traceFailure.Trace(
                            $"Failed to retry at {retryCount}th retry.");

                    case IUncertainFailureResult<TResult> failure:
                        return UncertainResults.FailWithTrace<TResult>(
                            $"Failed to retry at {retryCount}th retry because -> {failure.Message}");

                    default:
                        // Panic!
                        throw new UncertainResultPatternMatchException(nameof(result));
                }
            }

            // Over max retry count
            return UncertainResults.RetryWithTrace<TResult>(
                $"Retryable because retry count was over max count:{maxRetryCount}.");
        }
    }
}