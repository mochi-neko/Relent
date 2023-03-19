#nullable enable
namespace Mochineko.Relent.Resilience.CircuitBreaker
{
    /// <summary>
    /// Defines a circuit breaker policy that can be applied to an operation with no result value.
    /// </summary>
    public interface ICircuitBreakerPolicy
        : IPolicy
    {
        /// <summary>
        /// Current state of the circuit breaker.
        /// </summary>
        CircuitState State { get; }
        /// <summary>
        /// Isolates the circuit breaker.
        /// </summary>
        void Isolate();
    }
    
    /// <summary>
    /// Defines a circuit breaker policy that can be applied to an operation with result value.
    /// </summary>
    /// <typeparam name="TResult">Type of result value</typeparam>
    public interface ICircuitBreakerPolicy<TResult>
        : IPolicy<TResult>
    {
        /// <summary>
        /// Current state of the circuit breaker.
        /// </summary>
        CircuitState State { get; }
        /// <summary>
        /// Isolates the circuit breaker.
        /// </summary>
        void Isolate();
    }
}