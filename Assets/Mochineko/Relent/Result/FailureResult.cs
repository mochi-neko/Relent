#nullable enable
namespace Mochineko.Relent.Result
{
    internal sealed class FailureResult
        : IFailureResult
    {
        public bool Success => false;
        public bool Failure => true;
        public string Message { get; }

        public FailureResult(string message)
        {
            Message = message;
        }
    }
    
    internal sealed class FailureResult<TResult>
        : IFailureResult<TResult>
    {
        public bool Success => false;
        public bool Failure => true;
        public string Message { get; }

        public FailureResult(string message)
        {
            Message = message;
        }
    }
}