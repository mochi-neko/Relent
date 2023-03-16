#nullable enable
namespace Mochineko.Result
{
    /// <summary>
    /// Defines a failure result of a process with a message.
    /// </summary>
    public interface IFailureResult
        : IResult
    {
        string Message { get; }
    }

    /// <summary>
    /// Defines a failure result of a process with a message.
    /// </summary>
    /// <typeparam name="TResult">Type of result value</typeparam>
    public interface IFailureResult<TResult>
        : IResult<TResult>
    {
        string Message { get; }
    }
}