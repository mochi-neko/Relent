#nullable enable
using System;

namespace Mochineko.Result.Experimental
{
    internal sealed class FailureResultWithError<TResult, TReason>
        : IFailureResultWithError<TResult, TReason>
        where TReason : Exception
    {
        public bool Success => false;
        public bool Failure => true;
        public string Message { get; }
        public TReason Error { get; }

        public FailureResultWithError(string message, TReason reason)
        {
            Message = message;
            Error = reason;
        }
    }
}