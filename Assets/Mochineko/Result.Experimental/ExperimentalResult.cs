#nullable enable
using System;

namespace Mochineko.Result.Experimental
{
    public static class ExperimentalResult
    {
        public static IResult Fail<TError>(string message, TError error)
            where TError : Exception
            => new FailureResultWithError<TError>(message, error);

        public static IResult ChainFail(string message, IFailureResult inner)
            => new ChainedFailureResult(
                message,
                inner);

        public static IResult<TResult> Fail<TResult, TError>(string message, TError error)
            where TError : Exception
            => new FailureResultWithError<TResult, TError>(message, error);

        public static IResult<TResult> ChainFail<TResult>(string message, IFailureResult<TResult> inner)
            => new ChainedFailureResult<TResult>(
                message,
                inner);

        public static IResult FailureBy<TError>(this IResult result)
            where TError : Exception
        {
            return result switch
            {
                ISuccessResult => global::Mochineko.Result.Result.Fail($"Result type:{result.GetType()} is not failure."),
                IFailureResultWithError<TError> => global::Mochineko.Result.Result.Ok(),
                _ => MissingTypePatternMatchResult(result)
            };
        }
        
        public static IResult ChainedFailure(this IResult result)
        {
            return result switch
            {
                ISuccessResult => global::Mochineko.Result.Result.Fail($"Result type:{result.GetType()} is not failure."),
                IChainedFailureResult => global::Mochineko.Result.Result.Ok(),
                _ => MissingTypePatternMatchResult(result)
            };
        }

        private static IResult MissingTypePatternMatchResult(IResult result)
            => global::Mochineko.Result.Result.Fail($"Missing match type for result type:{result.GetType()}.");
    }
}