#nullable enable
namespace Mochineko.Result
{
    /// <summary>
    /// Defines a failure result with message.
    /// </summary>
    /// <typeparam name="TResult">Type of result value</typeparam>
    public interface IFailureResult<TResult>
        : IResult<TResult>
    {
        string Message { get; }
    }
}