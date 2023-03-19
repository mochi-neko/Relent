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
        
        public async Task<IResult<TResult>> ExecuteAsync(
            Func<CancellationToken, Task<IUncertainResult<TResult>>> execute,
            CancellationToken cancellationToken)
        {
            var waitResult = await WaitUtility.WaitAsync(semaphoreSlim, cancellationToken);
            if (waitResult is IFailureResult waitFailure)
            {
                semaphoreSlim.Release();
                return ResultFactory.Fail<TResult>(waitFailure.Message);
            }
            
            var result = await execute.Invoke(cancellationToken);
            
            semaphoreSlim.Release();
            
            if (result is IUncertainSuccessResult<TResult> success)
            {
                return ResultFactory.Succeed(success.Result);
            }
            else if (result is IUncertainRetryableResult<TResult> retryable)
            {
                return ResultFactory.Fail<TResult>(
                    $"Failed in bulkhead because result was retryable:{retryable.Message}.");
            }
            else if (result is IUncertainFailureResult<TResult> failure)
            {
                return ResultFactory.Fail<TResult>(
                    $"Failed in bulkhead because result was failure:{failure.Message}.");
            }
            else
            {
                throw new UncertainResultPatternMatchException(nameof(result));
            }
        }
    }
}