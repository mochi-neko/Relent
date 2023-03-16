#nullable enable

namespace Mochineko.HttpResult
{
    public interface IHttpResult<TResult>
    {
        bool Success { get; }
        bool Retryable { get; }
        bool Failure { get; }
    }
}