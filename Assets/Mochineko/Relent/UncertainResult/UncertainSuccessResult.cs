#nullable enable
namespace Mochineko.Relent.UncertainResult
{
    internal sealed class UncertainSuccessResult
        : IUncertainSuccessResult
    {
        public bool Success => true;
        public bool Retryable => false;
        public bool Failure => false;

        public static IUncertainSuccessResult Instance { get; }
            = new UncertainSuccessResult();
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