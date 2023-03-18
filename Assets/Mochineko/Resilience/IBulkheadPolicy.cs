#nullable enable
namespace Mochineko.Resilience
{
    public interface IBulkheadPolicy<TResult>
        : IPolicy<TResult>
    {
        int RemainingParallelizationCount { get; }
    }
}