#nullable enable
namespace Mochineko.Relent.Result
{
    /// <summary>
    /// Defines a failure result of an operation with trace messages
    /// </summary>
    public interface IFailureTraceResult
        : IFailureResult
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
    /// <typeparam name="TResult">Type of result value</typeparam>
    public interface IFailureTraceResult<TResult>
        : IFailureResult<TResult>
    {
        /// <summary>
        /// Adds a trace message.
        /// </summary>
        /// <param name="message"></param>
        void AddTrace(string message);
    }
}