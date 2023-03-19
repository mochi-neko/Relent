#nullable enable
using System;

namespace Mochineko.Relent.Resilience.CircuitBreaker
{
    /// <summary>
    /// A factory of circuit breaker policies.
    /// </summary>
    public static class CircuitBreakerFactory
    {
        /// <summary>
        /// Creates a circuit breaker policy that isolates the operation when the failure threshold is exceeded.
        /// </summary>
        /// <param name="failureThreshold"></param>
        /// <param name="interval"></param>
        /// <returns></returns>
        public static ICircuitBreakerPolicy CircuitBreaker(
            int failureThreshold,
            TimeSpan interval)
            => new CircuitBreakerPolicy(failureThreshold, interval);
        
        /// <summary>
        /// Creates a circuit breaker policy that isolates the operation when the failure threshold is exceeded.
        /// </summary>
        /// <param name="failureThreshold"></param>
        /// <param name="interval"></param>
        /// <typeparam name="TResult">Type of result value</typeparam>
        /// <returns></returns>
        public static ICircuitBreakerPolicy<TResult> CircuitBreaker<TResult>(
            int failureThreshold,
            TimeSpan interval)
            => new CircuitBreakerPolicy<TResult>(failureThreshold, interval);
    }
}