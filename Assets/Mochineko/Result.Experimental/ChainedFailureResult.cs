#nullable enable

namespace Mochineko.Result.Experimental
{
    internal sealed class ChainedFailureResult
        : IChainedFailureResult
    {
        public bool Success => false;
        public bool Failure => true;
        public string Message { get; }
        public IFailureResult Inner { get; }

        public ChainedFailureResult(string message, IFailureResult parent)
        {
            Message = message;
            Inner = parent;
        }
    }
    
    internal sealed class ChainedFailureResult<TResult>
        : IChainedFailureResult<TResult>
    {
        public bool Success => false;
        public bool Failure => true;
        public string Message { get; }
        public IFailureResult<TResult> Inner { get; }

        public ChainedFailureResult(string message, IFailureResult<TResult> parent)
        {
            Message = message;
            Inner = parent;
        }
    }
}