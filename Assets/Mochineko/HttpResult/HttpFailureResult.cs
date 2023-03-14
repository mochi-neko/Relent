#nullable enable
using System;
using System.Net;

namespace Mochineko.HttpResult
{
    internal sealed class HttpFailureResult<TReason>
        : IHttpFailureResult<TReason>
        where TReason : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public TReason Reason { get; }
        
        public HttpFailureResult(HttpStatusCode statusCode, TReason reason)
        {
            StatusCode = statusCode;
            Reason = reason;
        }
    }
    
    internal sealed class HttpFailureResult<TResult, TReason>
        : IHttpFailureResult<TResult, TReason>
        where TReason : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public TReason Reason { get; }
        
        public HttpFailureResult(HttpStatusCode statusCode, TReason reason)
        {
            StatusCode = statusCode;
            Reason = reason;
        }
    }
}