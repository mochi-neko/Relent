#nullable enable
namespace Mochineko.Resilience
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