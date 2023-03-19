#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using Mochineko.Relent.UncertainResult;

namespace Mochineko.Relent.Resilience.CircuitBreaker
{
    internal sealed class CircuitBreakerPolicy
        : ICircuitBreakerPolicy
    {
        private readonly int failureThreshold;
        private readonly TimeSpan interval;
        
        private readonly object lockObject = new();
        
        private CircuitState state;
        public CircuitState State => state;
        private int failureCount;
        private DateTime lastFailureTime;
        
        public CircuitBreakerPolicy(int failureThreshold, TimeSpan interval)
        {
            if (failureThreshold <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(failureThreshold));;
            }
            
            this.failureThreshold = failureThreshold;
            this.interval = interval;
            
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
            && DateTime.Now - lastFailureTime >= interval;
        
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

        public async Task<IUncertainResult> ExecuteAsync(
            Func<CancellationToken, Task<IUncertainResult>> execute,
            CancellationToken cancellationToken)
        {
            if (state is CircuitState.Isolated)
            {
                return UncertainResultFactory.Fail(
                    "Failed because circuit breaker is manually isolated.");
            }
            
            if (CanCloseHalf)
            {
                CloseHalf();
            }
            
            if (state is CircuitState.Open)
            {
                return UncertainResultFactory.Retry(
                    $"Retryable because circuit breaker is open by over threshold failure:{failureThreshold}.");
            }
            else // Closed or HalfOpen
            {
                var result = await execute.Invoke(cancellationToken);
                
                if (result is IUncertainSuccessResult success)
                {
                    if (state is CircuitState.HalfOpen)
                    {
                        Close();
                    }
                    
                    return success;
                }
                else if (result is IUncertainRetryableResult retryable)
                {
                    TrackFailure();
                    
                    return UncertainResultFactory.Retry(
                        $"Retryable at circuit breaker because -> {retryable.Message}.");
                }
                else if (result is IUncertainFailureResult failure)
                {
                    return UncertainResultFactory.Fail(
                        $"Failed at circuit breaker because -> {failure.Message}.");
                }
                else
                {
                    // Panic!
                    throw new UncertainResultPatternMatchException(nameof(result));
                }
            }
        }
    }
    
    internal sealed class CircuitBreakerPolicy<TResult>
        : ICircuitBreakerPolicy<TResult>
    {
        private readonly int failureThreshold;
        private readonly TimeSpan interval;
        
        private readonly object lockObject = new();
        
        private CircuitState state;
        public CircuitState State => state;
        private int failureCount;
        private DateTime lastFailureTime;
        
        public CircuitBreakerPolicy(int failureThreshold, TimeSpan interval)
        {
            if (failureThreshold <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(failureThreshold));;
            }
            
            this.failureThreshold = failureThreshold;
            this.interval = interval;
            
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
            && DateTime.Now - lastFailureTime >= interval;
        
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

        public async Task<IUncertainResult<TResult>> ExecuteAsync(
            Func<CancellationToken, Task<IUncertainResult<TResult>>> execute,
            CancellationToken cancellationToken)
        {
            if (state is CircuitState.Isolated)
            {
                return UncertainResultFactory.Fail<TResult>(
                    "Failed because circuit breaker is manually isolated.");
            }
            
            if (CanCloseHalf)
            {
                CloseHalf();
            }
            
            if (state is CircuitState.Open)
            {
                return UncertainResultFactory.Retry<TResult>(
                    $"Retryable because circuit breaker is open by over threshold failure:{failureThreshold}.");
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
                    
                    return success;
                }
                else if (result is IUncertainRetryableResult<TResult> retryable)
                {
                    TrackFailure();
                    
                    return UncertainResultFactory.Retry<TResult>(
                        $"Retryable at circuit breaker because -> {retryable.Message}.");
                }
                else if (result is IUncertainFailureResult<TResult> failure)
                {
                    return UncertainResultFactory.Fail<TResult>(
                        $"Failed at circuit breaker because -> {failure.Message}.");
                }
                else
                {
                    // Panic!
                    throw new UncertainResultPatternMatchException(nameof(result));
                }
            }
        }
    }
}