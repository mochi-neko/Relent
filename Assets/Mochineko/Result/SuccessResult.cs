#nullable enable
namespace Mochineko.Result
{
    internal sealed class SuccessResult
        : ISuccessResult
    {
        public bool Success => true;
        public bool Failure => false;
    }
    
    internal sealed class SuccessResult<TResult>
        : ISuccessResult<TResult>
    {
        public bool Success => true;
        public bool Failure => false;
        public TResult Result { get; }
        
        public SuccessResult(TResult result)
        {
            Result = result;
        }
    }
}