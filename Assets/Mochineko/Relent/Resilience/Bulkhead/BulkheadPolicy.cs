#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Mochineko.Relent.UncertainResult;

namespace Mochineko.Relent.Resilience.Bulkhead
{
    internal sealed class BulkheadPolicy
        : IBulkheadPolicy
    {
        private readonly SemaphoreSlim semaphoreSlim;

        public int RemainingParallelizationCount
            => semaphoreSlim.CurrentCount;

        public BulkheadPolicy(int maxParallelization)
        {
            semaphoreSlim = new SemaphoreSlim(maxParallelization, maxParallelization);
        }

        public async UniTask<IUncertainResult> ExecuteAsync(
            Func<CancellationToken, UniTask<IUncertainResult>> execute,
            CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return UncertainResults.RetryWithTrace(
                    $"Cancelled before bulkhead because of {nameof(cancellationToken)} is cancelled.");
            }

            var waitResult = await WaitUtility.WaitAsync(semaphoreSlim, cancellationToken);
            switch (waitResult)
            {
                case IUncertainSuccessResult:
                {
                    var result = await execute.Invoke(cancellationToken);

                    semaphoreSlim.Release();

                    return result switch
                    {
                        IUncertainSuccessResult success => success,

                        IUncertainTraceRetryableResult traceRetryable =>
                            traceRetryable.Trace($"Retryable at bulkhead."),

                        IUncertainRetryableResult retryable => UncertainResults.RetryWithTrace(
                            $"Retryable at bulkhead because -> {retryable.Message}."),

                        IUncertainTraceFailureResult traceFailure => traceFailure.Trace($"Failed at bulkhead."),

                        IUncertainFailureResult failure => UncertainResults.Fail(
                            $"Failed at bulkhead because -> {failure.Message}."),

                        _ => throw new UncertainResultPatternMatchException(nameof(result))
                    };
                }

                case IUncertainTraceRetryableResult waitRetryable:
                    semaphoreSlim.Release();
                    return waitRetryable.Trace(
                        $"Cancelled in bulkhead waiting because -> {waitRetryable.Message}.");

                default:
                    semaphoreSlim.Release();
                    // Panic!
                    throw new UncertainResultPatternMatchException(nameof(waitResult));
            }
        }
    }

    internal sealed class BulkheadPolicy<TResult>
        : IBulkheadPolicy<TResult>
    {
        private readonly SemaphoreSlim semaphoreSlim;

        public int RemainingParallelizationCount
            => semaphoreSlim.CurrentCount;

        public BulkheadPolicy(int maxParallelization)
        {
            semaphoreSlim = new SemaphoreSlim(maxParallelization, maxParallelization);
        }

        public async UniTask<IUncertainResult<TResult>> ExecuteAsync(
            Func<CancellationToken, UniTask<IUncertainResult<TResult>>> execute,
            CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return UncertainResults.RetryWithTrace<TResult>(
                    $"Cancelled before bulkhead because of {nameof(cancellationToken)} is cancelled.");
            }

            var waitResult = await WaitUtility.WaitAsync(semaphoreSlim, cancellationToken);
            switch (waitResult)
            {
                case IUncertainSuccessResult:
                {
                    var result = await execute.Invoke(cancellationToken);

                    semaphoreSlim.Release();

                    switch (result)
                    {
                        case IUncertainSuccessResult<TResult> success:
                            return success;

                        case IUncertainTraceRetryableResult<TResult> traceRetryable:
                            return traceRetryable.Trace($"Retryable at bulkhead.");

                        case IUncertainRetryableResult<TResult> retryable:
                            return UncertainResults.Retry<TResult>(
                                $"Retryable at bulkhead because -> {retryable.Message}.");

                        case IUncertainTraceFailureResult<TResult> traceFailure:
                            return traceFailure.Trace($"Failed at bulkhead.");

                        case IUncertainFailureResult<TResult> failure:
                            return UncertainResults.Fail<TResult>(
                                $"Failed at bulkhead because -> {failure.Message}.");

                        default:
                            // Panic!
                            throw new UncertainResultPatternMatchException(nameof(result));
                    }
                }

                case IUncertainTraceRetryableResult waitRetryable:
                    semaphoreSlim.Release();
                    return UncertainResults.Retry<TResult>(
                        $"Cancelled in bulkhead waiting because -> {waitRetryable.Message}.");

                default:
                    semaphoreSlim.Release();
                    // Panic!
                    throw new UncertainResultPatternMatchException(nameof(waitResult));
            }
        }
    }
}