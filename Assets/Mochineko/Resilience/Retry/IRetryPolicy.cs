#nullable enable
namespace Mochineko.Resilience.Retry
{
    /// <summary>
    /// Defines a retry policy that can be applied to an operation with no result value.
    /// </summary>
    public interface IRetryPolicy
        : IPolicy
    {
        /// <summary>
        /// Current retry count.
        /// </summary>
        int RetryCount { get; }
    }
    
    /// <summary>
    /// Defines a retry policy that can be applied to an operation with result value.
    /// </summary>
    /// <typeparam name="TResult">Type of result value</typeparam>
    public interface IRetryPolicy<TResult>
        : IPolicy<TResult>
    {
        /// <summary>
        /// Current retry count.
        /// </summary>
        int RetryCount { get; }
    }
}