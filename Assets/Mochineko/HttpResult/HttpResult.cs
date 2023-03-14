#nullable enable
using System;
using System.Net;

namespace Mochineko.HttpResult
{
    public static class HttpResult
    {
        public static IHttpResult Ok(HttpStatusCode statusCode)
            => new HttpSuccessResult(statusCode);
        
        public static IHttpResult Retry<TReason>(HttpStatusCode statusCode, TReason reason)
            where TReason : Exception
            => new HttpRetryableResult<TReason>(statusCode, reason);
        
        public static IHttpResult Fail<TReason>(HttpStatusCode statusCode, TReason reason)
            where TReason : Exception
            => new HttpFailureResult<TReason>(statusCode, reason);
        
        public static IHttpResult<TResult> Ok<TResult>(HttpStatusCode statusCode, TResult result)
            => new HttpSuccessResult<TResult>(statusCode, result);

        public static IHttpResult<TResult> Retry<TResult, TReason>(HttpStatusCode statusCode, TReason reason)
            where TReason : Exception
            => new HttpRetryableResult<TResult, TReason>(statusCode, reason);

        public static IHttpResult<TResult> Fail<TResult, TReason>(HttpStatusCode statusCode, TReason reason)
            where TReason : Exception
            => new HttpFailureResult<TResult, TReason>(statusCode, reason);
    }
}