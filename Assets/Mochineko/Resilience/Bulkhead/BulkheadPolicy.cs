#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using Mochineko.Result;
using Mochineko.UncertainResult;

namespace Mochineko.Resilience.Bulkhead
{
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
        
        public async Task<IUncertainResult<TResult>> ExecuteAsync(
            Func<CancellationToken, Task<IUncertainResult<TResult>>> execute,
            CancellationToken cancellationToken)
        {
            var waitResult = await WaitUtility.WaitAsync(semaphoreSlim, cancellationToken);
            if (waitResult.Success)
            {
                var result = await execute.Invoke(cancellationToken);
            
                semaphoreSlim.Release();
            
                if (result is IUncertainSuccessResult<TResult> success)
                {
                    return UncertainResultFactory.Succeed(success.Result);
                }
                else if (result is IUncertainRetryableResult<TResult> retryable)
                {
                    return UncertainResultFactory.Retry<TResult>(
                        $"Retryable at bulkhead because -> {retryable.Message}.");
                }
                else if (result is IUncertainFailureResult<TResult> failure)
                {
                    return UncertainResultFactory.Fail<TResult>(
                        $"Failed at bulkhead because -> {failure.Message}.");
                }
                else
                {
                    // Panic!
                    throw new UncertainResultPatternMatchException(nameof(result));
                }
            }
            if (waitResult is IUncertainRetryableResult waitRetryable)
            {
                semaphoreSlim.Release();
                return UncertainResultFactory.Retry<TResult>(
                    $"Cancelled in bulkhead waiting because -> {waitRetryable.Message}.");
            }
            else if (waitResult is IUncertainFailureResult waitFailure)
            {
                semaphoreSlim.Release();
                return UncertainResultFactory.Fail<TResult>(
                    $"Failed in bulkhead waiting because -> {waitFailure.Message}.");
            }
            else
            {
                // Panic!
                throw new UncertainResultPatternMatchException(nameof(waitResult));
            }
        }
    }
}