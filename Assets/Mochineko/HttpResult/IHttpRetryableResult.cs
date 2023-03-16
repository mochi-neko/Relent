#nullable enable

namespace Mochineko.HttpResult
{
    public interface IHttpRetryableResult<TResult>
        : IHttpResult<TResult>
    {
        string Message { get; }
    }
}