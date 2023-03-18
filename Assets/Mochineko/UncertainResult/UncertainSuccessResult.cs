#nullable enable
namespace Mochineko.UncertainResult
{
    internal sealed class UncertainSuccessResult
        : IUncertainSuccessResult
    {
        public bool Success => true;
        public bool Retryable => false;
        public bool Failure => false;
    }
    
    internal sealed class UncertainSuccessResult<TResult>
        : IUncertainSuccessResult<TResult>
    {
        public bool Success => true;
        public bool Retryable => false;
        public bool Failure => false;
        public TResult Result { get; }
        
        public UncertainSuccessResult(TResult result)
        {
            Result = result;
        }
    }
}