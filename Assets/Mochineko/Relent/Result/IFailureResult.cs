#nullable enable
namespace Mochineko.Relent.Result
{
    /// <summary>
    /// Defines a failure result of an operation with a message.
    /// </summary>
    public interface IFailureResult
        : IResult
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
    public interface IFailureResult<TResult>
        : IResult<TResult>
    {
        /// <summary>
        /// Message of the failure.
        /// </summary>
        string Message { get; }
    }
}