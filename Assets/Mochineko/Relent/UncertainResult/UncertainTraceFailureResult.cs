#nullable enable
using System.Text;

namespace Mochineko.Relent.UncertainResult
{
    internal sealed class UncertainTraceFailureResult
        : IUncertainTraceFailureResult
    {
        public bool Success => false;
        public bool Retryable => false;
        public bool Failure => true;
        public string Message => trace.ToString();

        private readonly StringBuilder trace = new();

        public UncertainTraceFailureResult(string message)
        {
            AddTrace(message);
        }
        
        public void AddTrace(string message)
        {
            trace.AppendLine(message);
        }
    }

    internal sealed class UncertainTraceFailureResult<TResult>
        : IUncertainTraceFailureResult<TResult>
    {
        public bool Success => false;
        public bool Retryable => false;
        public bool Failure => true;
        public string Message => trace.ToString();

        private readonly StringBuilder trace = new();

        public UncertainTraceFailureResult(string message)
        {
            AddTrace(message);
        }

        public void AddTrace(string message)
        {
            trace.AppendLine(message);
        }
    }
}