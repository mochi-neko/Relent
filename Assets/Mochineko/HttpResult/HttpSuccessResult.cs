#nullable enable
namespace Mochineko.HttpResult
{
    internal sealed class HttpSuccessResult
        : IHttpSuccessResult
    {
        public bool Success => true;
        public bool Retryable => false;
        public bool Failure => false;
    }
    
    internal sealed class HttpSuccessResult<TResult>
        : IHttpSuccessResult<TResult>
    {
        public bool Success => true;
        public bool Retryable => false;
        public bool Failure => false;
        public TResult Result { get; }
        
        public HttpSuccessResult(TResult result)
        {
            Result = result;
        }
    }
}