#nullable enable
namespace Mochineko.Relent.Resilience.Bulkhead
{
    /// <summary>
    /// A factory of bulkhead policies.
    /// </summary>
    public static class BulkheadFactory
    {
        /// <summary>
        /// Creates a bulkhead policy that limits the number of parallelization.
        /// </summary>
        /// <param name="maxParallelization"></param>
        /// <returns></returns>
        public static IBulkheadPolicy Bulkhead(
            int maxParallelization)
            => new BulkheadPolicy(maxParallelization);
        
        /// <summary>
        /// Creates a bulkhead policy that limits the number of parallelization.
        /// </summary>
        /// <param name="maxParallelization"></param>
        /// <typeparam name="TResult">Type of result value</typeparam>
        /// <returns></returns>
        public static IBulkheadPolicy<TResult> Bulkhead<TResult>(
            int maxParallelization)
            => new BulkheadPolicy<TResult>(maxParallelization);
    }
}