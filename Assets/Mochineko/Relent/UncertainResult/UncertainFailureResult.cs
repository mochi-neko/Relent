#nullable enable
namespace Mochineko.Relent.UncertainResult
{
    internal sealed class UncertainFailureResult
        : IUncertainFailureResult
    {
        public bool Success => false;
        public bool Retryable => false;
        public bool Failure => true;
        public string Message { get; }

        public UncertainFailureResult(string message)
        {
            Message = message;
        }
    }
    
    internal sealed class UncertainFailureResult<TResult>
        : IUncertainFailureResult<TResult>
    {
        public bool Success => false;
        public bool Retryable => false;
        public bool Failure => true;
        public string Message { get; }

        public UncertainFailureResult(string message)
        {
            Message = message;
        }
    }
}