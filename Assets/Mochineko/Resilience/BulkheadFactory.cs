#nullable enable
namespace Mochineko.Resilience
{
    public static class BulkheadFactory
    {
        public static IBulkheadPolicy<TResult> Bulkhead<TResult>(
            int maxParallelization)
            => new BulkheadPolicy<TResult>(maxParallelization);
    }
}