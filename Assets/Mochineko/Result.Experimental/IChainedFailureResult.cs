#nullable enable

namespace Mochineko.Result.Experimental
{
    public interface IChainedFailureResult<TResult>
        : IFailureResult<TResult>
    {
        IFailureResult<TResult> Inner { get; }
    }
}