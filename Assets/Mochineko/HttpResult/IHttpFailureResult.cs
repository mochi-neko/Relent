#nullable enable

namespace Mochineko.HttpResult
{
    public interface IHttpFailureResult<TResult>
        : IHttpResult<TResult>
    {
        string Message { get; }
    }
}