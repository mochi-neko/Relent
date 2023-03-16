#nullable enable
using System;

namespace Mochineko.Result.Experimental
{
    public static class ExperimentalResult
    {
        public static IResult<TResult> Fail<TResult, TError>(string message, TError error)
            where TError : Exception
            => new FailureResultWithError<TResult, TError>(message, error);

        public static IResult<TResult> ChainFail<TResult>(string message, IFailureResult<TResult> inner)
            => new ChainedFailureResult<TResult>(
                message,
                inner);
    }
}