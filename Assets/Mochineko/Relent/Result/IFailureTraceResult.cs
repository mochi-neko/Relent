#nullable enable
namespace Mochineko.Relent.Result
{
    public interface IFailureTraceResult
        : IFailureResult
    {
        void AddTrace(string message);
    }
    
    public interface IFailureTraceResult<TResult>
        : IFailureResult<TResult>
    {
        void AddTrace(string message);
    }
}