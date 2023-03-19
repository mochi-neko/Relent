#nullable enable
namespace Mochineko.Relent.Resilience.CircuitBreaker
{
    /// <summary>
    /// Circuit state in circuit breaker.
    /// </summary>
    public enum CircuitState
    {
        /// <summary>
        /// Circuit is closed.
        /// </summary>
        Closed,
        /// <summary>
        /// Circuit is open.
        /// </summary>
        Open,
        /// <summary>
        /// Circuit is half-open.
        /// </summary>
        HalfOpen,
        /// <summary>
        /// Circuit is isolated.
        /// </summary>
        Isolated,
    }
}