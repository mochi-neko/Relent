#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using Mochineko.Result;
using Mochineko.UncertainResult;

namespace Mochineko.Resilience
{
    internal sealed class CircuitBreakerPolicy<TResult>
        : ICircuitBreakerPolicy<TResult>
    {
        private readonly int failureThreshold;
        private readonly TimeSpan breakTime;
        
        private readonly object lockObject = new();
        
        private CircuitState state;
        public CircuitState State => state;
        private int failureCount;
        private DateTime lastFailureTime;
        
        public CircuitBreakerPolicy(int failureThreshold, TimeSpan breakTime)
        {
            if (failureThreshold <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(failureThreshold));;
            }
            
            this.failureThreshold = failureThreshold;
            this.breakTime = breakTime;
            
            state = CircuitState.Closed;
            failureCount = 0;
            lastFailureTime = DateTime.MinValue;
        }

        private void Close()
        {
            lock (lockObject)
            {
                failureCount = 0;
                state = CircuitState.Closed;
            }
        }
        
        private void TrackFailure()
        {
            lock (lockObject)
            {
                failureCount++;
                lastFailureTime = DateTime.Now;

                if (failureCount >= failureThreshold)
                {
                    // Reopen if HalfOpen state
                    state = CircuitState.Open;
                }
            }
        }

        private bool CanCloseHalf
            => state is CircuitState.Open
            && DateTime.Now - lastFailureTime >= breakTime;
        
        private void CloseHalf()
        {
            lock (lockObject)
            {
                state = CircuitState.HalfOpen;
            }
        }

        public void Isolate()
        {
            lock (lockObject)
            {
                state = CircuitState.Isolated;
            }
        }

        public async Task<IResult<TResult>> ExecuteAsync(
            Func<CancellationToken, Task<IUncertainResult<TResult>>> execute,
            CancellationToken cancellationToken)
        {
            if (state is CircuitState.Isolated)
            {
                return ResultFactory.Fail<TResult>(
                    "Failed because circuit breaker is isolated.");
            }
            
            if (CanCloseHalf)
            {
                CloseHalf();
            }
            
            if (state is CircuitState.Open)
            {
                return ResultFactory.Fail<TResult>(
                    "Failed because circuit breaker is open.");
            }
            else // Closed or HalfOpen
            {
                var result = await execute.Invoke(cancellationToken);
                
                if (result is IUncertainSuccessResult<TResult> success)
                {
                    if (state is CircuitState.HalfOpen)
                    {
                        Close();
                    }
                    
                    return ResultFactory.Succeed(success.Result);
                }
                else if (result is IUncertainRetryableResult<TResult> retryable)
                {
                    TrackFailure();
                    
                    return ResultFactory.Fail<TResult>(
                        $"Failed because result was retryable:{retryable.Message}.");
                }
                else if (result is IUncertainFailureResult<TResult> failure)
                {
                    TrackFailure();
                    
                    return ResultFactory.Fail<TResult>(
                        $"Failed because result was failure:{failure.Message}.");
                }
                else
                {
                    throw new ResultPatternMatchException(nameof(result));
                }
            }
        }
    }
}