#nullable enable
namespace Mochineko.Resilience.Bulkhead
{
    public interface IBulkheadPolicy
        : IPolicy
    {
        int RemainingParallelizationCount { get; }
    }
    
    public interface IBulkheadPolicy<TResult>
        : IPolicy<TResult>
    {
        int RemainingParallelizationCount { get; }
    }
}