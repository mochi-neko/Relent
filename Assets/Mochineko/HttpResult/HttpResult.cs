#nullable enable
namespace Mochineko.HttpResult
{
    public static class HttpResult
    {
        public static IHttpResult<TResult> Ok<TResult>(TResult result)
            => new HttpSuccessResult<TResult>(result);

        public static IHttpResult<TResult> Retry<TResult>(string message)
            => new HttpRetryableResult<TResult>(message);

        public static IHttpResult<TResult> Fail<TResult>(string message)
            => new HttpFailureResult<TResult>(message);
    }
}