#nullable enable
using System.Net;

namespace Mochineko.HttpResult
{
    internal sealed class HttpSuccessResult
        : IHttpSuccessResult
    {
        public HttpStatusCode StatusCode { get; }
        
        public HttpSuccessResult(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }
    }
    
    internal sealed class HttpSuccessResult<TResult>
        : IHttpSuccessResult<TResult>
    {
        public HttpStatusCode StatusCode { get; }
        public TResult Result { get; }
        
        public HttpSuccessResult(HttpStatusCode statusCode, TResult result)
        {
            StatusCode = statusCode;
            Result = result;
        }
    }
}