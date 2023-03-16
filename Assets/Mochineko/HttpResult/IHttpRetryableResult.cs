#nullable enable

namespace Mochineko.HttpResult
{
    public interface IHttpRetryableResult
        : IHttpResult
    {
        string Message { get; }
    }
    
    public interface IHttpRetryableResult<TResult>
        : IHttpResult<TResult>
    {
        string Message { get; }
    }
}