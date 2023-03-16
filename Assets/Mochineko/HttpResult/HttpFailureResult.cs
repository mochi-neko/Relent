#nullable enable
namespace Mochineko.HttpResult
{
    internal sealed class HttpFailureResult<TResult>
        : IHttpFailureResult<TResult>
    {
        public bool Success => false;
        public bool Retryable => false;
        public bool Failure => true;
        public string Message { get; }

        public HttpFailureResult(string message)
        {
            Message = message;
        }
    }
}