#nullable enable
using System;

namespace Mochineko.Resilience.CircuitBreaker
{
    public static class CircuitBreakerFactory
    {
        public static ICircuitBreakerPolicy<TResult> CircuitBreaker<TResult>(
            int failureThreshold,
            TimeSpan interval)
            => new CircuitBreakerPolicy<TResult>(failureThreshold, interval);
    }
}