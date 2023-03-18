#nullable enable
namespace Mochineko.UncertainResult
{
    internal sealed class UncertainRetryableResult
        : IUncertainRetryableResult
    {
        public bool Success => false;
        public bool Retryable => true;
        public bool Failure => false;
        public string Message { get; }

        public UncertainRetryableResult(string message)
        {
            Message = message;
        }
    }
    
    internal sealed class UncertainRetryableResult<TResult>
        : IUncertainRetryableResult<TResult>
    {
        public bool Success => false;
        public bool Retryable => true;
        public bool Failure => false;
        public string Message { get; }

        public UncertainRetryableResult(string message)
        {
            Message = message;
        }
    }
}