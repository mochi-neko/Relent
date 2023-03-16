#nullable enable
namespace Mochineko.HttpResult
{
    public static class HttpResult
    {
        public static IHttpResult Ok()
            => new HttpSuccessResult();
        
        public static IHttpResult Retry(string message)
            => new HttpRetryableResult(message);
        
        public static IHttpResult Fail(string message)
            => new HttpFailureResult(message);
        
        public static IHttpResult<TResult> Ok<TResult>(TResult result)
            => new HttpSuccessResult<TResult>(result);

        public static IHttpResult<TResult> Retry<TResult>(string message)
            => new HttpRetryableResult<TResult>(message);

        public static IHttpResult<TResult> Fail<TResult>(string message)
            => new HttpFailureResult<TResult>(message);
        
        public static TResult Unwrap<TResult>(this IHttpResult<TResult> result)
        {
            if (result is IHttpSuccessResult<TResult> success)
            {
                return success.Result;
            }
            else
            {
                throw new FailedToUnwrapResultException($"Failed to unwrap {result.GetType()} into success result.");
            }
        }
    }
}