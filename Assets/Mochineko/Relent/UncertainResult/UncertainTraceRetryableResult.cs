#nullable enable
using System.Text;

namespace Mochineko.Relent.UncertainResult
{
    internal sealed class UncertainTraceRetryableResult
        : IUncertainTraceRetryableResult
    {
        public bool Success => false;
        public bool Retryable => true;
        public bool Failure => false;
        public string Message => trace.ToString();

        private readonly StringBuilder trace = new();

        public UncertainTraceRetryableResult(string message)
        {
            AddTrace(message);
        }
        
        public void AddTrace(string message)
        {
            trace.AppendLine(message);
        }
    }

    internal sealed class UncertainTraceRetryableResult<TResult>
        : IUncertainTraceRetryableResult<TResult>
    {
        public bool Success => false;
        public bool Retryable => true;
        public bool Failure => false;
        public string Message => trace.ToString();

        private readonly StringBuilder trace = new();

        public UncertainTraceRetryableResult(string message)
        {
            AddTrace(message);
        }

        public void AddTrace(string message)
        {
            trace.AppendLine(message);
        }
    }
}