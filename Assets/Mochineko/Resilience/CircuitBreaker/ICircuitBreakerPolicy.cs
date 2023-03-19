#nullable enable
namespace Mochineko.Resilience.CircuitBreaker
{
    public interface ICircuitBreakerPolicy
        : IPolicy
    {
        CircuitState State { get; }
        void Isolate();
    }
    
    public interface ICircuitBreakerPolicy<TResult>
        : IPolicy<TResult>
    {
        CircuitState State { get; }
        void Isolate();
    }
}