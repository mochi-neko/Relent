#nullable enable
using System;
using System.Net;

namespace Mochineko.HttpResult
{
    internal sealed class HttpRetryableResult<TReason>
        : IHttpRetryableResult<TReason>
        where TReason : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public TReason Reason { get; }

        public HttpRetryableResult(HttpStatusCode statusCode, TReason reason)
        {
            StatusCode = statusCode;
            Reason = reason;
        }
    }
    
    internal sealed class HttpRetryableResult<TResult, TReason>
        : IHttpRetryableResult<TResult, TReason>
        where TReason : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public TReason Reason { get; }
        
        public HttpRetryableResult(HttpStatusCode statusCode, TReason reason)
        {
            StatusCode = statusCode;
            Reason = reason;
        }
    }
}