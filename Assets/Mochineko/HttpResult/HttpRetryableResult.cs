#nullable enable
namespace Mochineko.HttpResult
{
    internal sealed class HttpRetryableResult
        : IHttpRetryableResult
    {
        public bool Success => false;
        public bool Retryable => true;
        public bool Failure => false;
        public string Message { get; }

        public HttpRetryableResult(string message)
        {
            Message = message;
        }
    }
    
    internal sealed class HttpRetryableResult<TResult>
        : IHttpRetryableResult<TResult>
    {
        public bool Success => false;
        public bool Retryable => true;
        public bool Failure => false;
        public string Message { get; }

        public HttpRetryableResult(string message)
        {
            Message = message;
        }
    }
}