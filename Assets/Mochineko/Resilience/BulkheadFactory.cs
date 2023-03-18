#nullable enable
namespace Mochineko.Resilience
{
    public static class BulkheadFactory
    {
        public static IPolicy<TResult> Bulkhead<TResult>(
            int maxParallelization)
            => new BulkheadPolicy<TResult>(maxParallelization);
    }
}