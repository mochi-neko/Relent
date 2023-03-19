#nullable enable
namespace Mochineko.Resilience.Bulkhead
{
    /// <summary>
    /// Defines a bulkhead policy that can be applied to an operation with no result value.
    /// </summary>
    public interface IBulkheadPolicy
        : IPolicy
    {
        /// <summary>
        /// Current remaining parallelization count.
        /// </summary>
        int RemainingParallelizationCount { get; }
    }
    
    /// <summary>
    /// Defines a bulkhead policy that can be applied to an operation with result value.
    /// </summary>
    /// <typeparam name="TResult">Type of result value</typeparam>
    public interface IBulkheadPolicy<TResult>
        : IPolicy<TResult>
    {
        /// <summary>
        /// Current remaining parallelization count.
        /// </summary>
        int RemainingParallelizationCount { get; }
    }
}