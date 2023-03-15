#nullable enable

namespace Mochineko.Result.Experimental
{
    public interface IChainedFailureResult
        : IFailureResult
    {
        IFailureResult Inner { get; }
    }
    
    public interface IChainedFailureResult<TResult>
        : IFailureResult<TResult>
    {
        IFailureResult<TResult> Inner { get; }
    }
}