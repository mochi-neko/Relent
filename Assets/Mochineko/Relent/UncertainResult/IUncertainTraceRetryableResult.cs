#nullable enable
namespace Mochineko.Relent.UncertainResult
{
    /// <summary>
    /// Defines a retryable failure result of an operation with trace messages.
    /// </summary>
    public interface IUncertainTraceRetryableResult
        : IUncertainRetryableResult
    {
        /// <summary>
        /// Adds a trace message.
        /// </summary>
        /// <param name="message"></param>
        void AddTrace(string message);
    }

    /// <summary>
    /// Defines a retryable failure result of an operation with trace messages.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public interface IUncertainTraceRetryableResult<TResult>
        : IUncertainRetryableResult<TResult>
    {
        /// <summary>
        /// Adds a trace message.
        /// </summary>
        /// <param name="message"></param>
        void AddTrace(string message);
    }
}