#nullable enable
namespace Mochineko.Resilience.CircuitBreaker
{
    public enum CircuitState
    {
        Closed,
        Open,
        HalfOpen,
        Isolated,
    }
}