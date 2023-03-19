#nullable enable
namespace Mochineko.UncertainResult
{
    /// <summary>
    /// Defines a retryable failure result of an operation with a message.
    /// </summary>
    public interface IUncertainRetryableResult
        : IUncertainResult
    {
        /// <summary>
        /// Message of the retryable failure.
        /// </summary>
        string Message { get; }
    }
    
    /// <summary>
    /// Defines a retryable failure result of an operation with a message.
    /// </summary>
    /// <typeparam name="TResult">Type of result value</typeparam>
    public interface IUncertainRetryableResult<TResult>
        : IUncertainResult<TResult>
    {
        /// <summary>
        /// Message of the retryable failure.
        /// </summary>
        string Message { get; }
    }
}