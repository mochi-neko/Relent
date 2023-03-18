#nullable enable
namespace Mochineko.Resilience
{
    public enum CircuitState
    {
        Closed,
        Open,
        HalfOpen,
        Isolated,
    }
}