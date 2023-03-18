#nullable enable
namespace Mochineko.Resilience
{
    public interface ICircuitBreakerPolicy<TResult>
        : IPolicy<TResult>
    {
        CircuitState State { get; }
        void Isolate();
    }
}