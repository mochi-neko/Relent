#nullable enable
namespace Mochineko.Relent.UncertainResult
{
    /// <summary>
    /// Defines a failure result of an operation with trace messages.
    /// </summary>
    public interface IUncertainTraceFailureResult
        : IUncertainFailureResult
    {
        /// <summary>
        /// Adds a trace message.
        /// </summary>
        /// <param name="message"></param>
        void AddTrace(string message);
    }

    /// <summary>
    /// Defines a failure result of an operation with trace messages.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public interface IUncertainTraceFailureResult<TResult>
        : IUncertainFailureResult<TResult>
    {
        /// <summary>
        /// Adds a trace message.
        /// </summary>
        /// <param name="message"></param>
        void AddTrace(string message);
    }
}