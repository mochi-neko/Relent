#nullable enable
namespace Mochineko.HttpResult
{
    /// <summary>
    /// Defines a failure result of HTTP communication with message.
    /// </summary>
    /// <typeparam name="TResult">Type of result value</typeparam>
    public interface IHttpFailureResult<TResult>
        : IHttpResult<TResult>
    {
        string Message { get; }
    }
}