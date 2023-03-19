#nullable enable
namespace Mochineko.UncertainResult
{
    /// <summary>
    /// Defines a failure result of an operation with a message.
    /// </summary>
    public interface IUncertainFailureResult
        : IUncertainResult
    {
        /// <summary>
        /// Message of the failure.
        /// </summary>
        string Message { get; }
    }

    /// <summary>
    /// Defines a failure result of an operation with a message.
    /// </summary>
    /// <typeparam name="TResult">Type of result value</typeparam>
    public interface IUncertainFailureResult<TResult>
        : IUncertainResult<TResult>
    {
        /// <summary>
        /// Message of the failure.
        /// </summary>
        string Message { get; }
    }
}