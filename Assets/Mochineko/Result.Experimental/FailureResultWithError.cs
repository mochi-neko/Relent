#nullable enable
using System;

namespace Mochineko.Result.Experimental
{
    internal sealed class FailureResultWithError<TError>
        : IFailureResultWithError<TError>
        where TError : Exception
    {
        public bool Success => false;
        public bool Failure => true;
        public string Message { get; }
        public TError Error { get; }

        public FailureResultWithError(string message, TError error)
        {
            Message = message;
            Error = error;
        }
    }

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