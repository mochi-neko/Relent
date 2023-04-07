#nullable enable
namespace Mochineko.Relent.UncertainResult
{
    public interface IUncertainTraceFailureResult
        : IUncertainFailureResult
    {
        void AddTrace(string message);
    }

    public interface IUncertainTraceFailureResult<TResult>
        : IUncertainFailureResult<TResult>
    {
        void AddTrace(string message);
    }
}