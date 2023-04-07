#nullable enable
using System.Text;

namespace Mochineko.Relent.Result
{
    internal sealed class FailureTraceResult
        : IFailureTraceResult
    {
        public bool Success => false;
        public bool Failure => true;
        public string Message => trace.ToString();

        private readonly StringBuilder trace = new();

        public FailureTraceResult(string message)
        {
            AddTrace(message);
        }
        
        public void AddTrace(string message)
        {
            trace.AppendLine(message);
        }
    }

    internal sealed class FailureTraceResult<TResult>
        : IFailureTraceResult<TResult>
    {
        public bool Success => false;
        public bool Failure => true;
        public string Message => trace.ToString();

        private readonly StringBuilder trace = new();

        public FailureTraceResult(string message)
        {
            AddTrace(message);
        }
        
        public void AddTrace(string message)
        {
            trace.AppendLine(message);
        }
    }
}