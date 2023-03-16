#nullable enable

namespace Mochineko.HttpResult
{
    public interface IHttpFailureResult
        : IHttpResult
    {
        string Message { get; }
    }

    public interface IHttpFailureResult<TResult>
        : IHttpResult<TResult>
    {
        string Message { get; }
    }
}