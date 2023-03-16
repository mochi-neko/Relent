#nullable enable
namespace Mochineko.HttpResult
{
    /// <summary>
    /// Defines a result of HTTP communication.
    /// The type argument may seem to do nothing,
    ///  but prevents cast to wrong types.
    /// </summary>
    /// <typeparam name="TResult">Type of result value</typeparam>
    public interface IHttpResult<TResult>
    {
        bool Success { get; }
        bool Retryable { get; }
        bool Failure { get; }
    }
}