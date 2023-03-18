#nullable enable
using System;

namespace Mochineko.Resilience
{
    public static class CircuitBreakerFactory
    {
        public static ICircuitBreakerPolicy<TResult> CircuitBreaker<TResult>(
            int failureThreshold,
            TimeSpan breakTime)
            => new CircuitBreakerPolicy<TResult>(failureThreshold, breakTime);
    }
}