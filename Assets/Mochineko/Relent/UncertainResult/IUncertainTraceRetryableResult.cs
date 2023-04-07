#nullable enable
namespace Mochineko.Relent.UncertainResult
{
    public interface IUncertainTraceRetryableResult
        : IUncertainRetryableResult
    {
        void AddTrace(string message);
    }

    public interface IUncertainTraceRetryableResult<TResult>
        : IUncertainRetryableResult<TResult>
    {
        void AddTrace(string message);
    }
}