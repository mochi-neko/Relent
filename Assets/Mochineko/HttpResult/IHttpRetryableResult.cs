#nullable enable
namespace Mochineko.HttpResult
{
    /// <summary>
    /// Defines a retryable result of HTTP communication with message.
    /// </summary>
    /// <typeparam name="TResult">Type of result value</typeparam>
    public interface IHttpRetryableResult<TResult>
        : IHttpResult<TResult>
    {
        string Message { get; }
    }
}