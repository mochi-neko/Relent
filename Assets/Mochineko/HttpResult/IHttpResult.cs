#nullable enable

namespace Mochineko.HttpResult
{
    public interface IHttpResult
    {
        bool Success { get; }
        bool Retryable { get; }
        bool Failure { get; }
    }
    
    public interface IHttpResult<TResult>
    {
        bool Success { get; }
        bool Retryable { get; }
        bool Failure { get; }
    }
}