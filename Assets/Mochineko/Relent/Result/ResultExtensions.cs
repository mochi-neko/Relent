#nullable enable
using System;

namespace Mochineko.Relent.Result
{
    public static class ResultExtensions
    {
        public static TResult Unwrap<TResult>(this IResult<TResult> result)
        {
            if (result is ISuccessResult<TResult> success)
            {
                return success.Result;
            }
            else
            {
                throw new InvalidOperationException("Failed to unwrap result.");
            }
        }

        public static string ExtractMessage(this IResult result)
        {
            if (result is IFailureResult failure)
            {
                return failure.Message;
            }
            else
            {
                throw new InvalidOperationException("Failed to extract message from failure result.");
            }
        }

        public static string ExtractMessage<TResult>(this IResult<TResult> result)
        {
            if (result is IFailureResult<TResult> failure)
            {
                return failure.Message;
            }
            else
            {
                throw new InvalidOperationException("Failed to extract message from failure result.");
            }
        }

        public static IResult<TResult> ToResult<TResult>(this TResult result)
            => ResultFactory.Succeed(result);
        
        public static IFailureTraceResult FailWithTrace(
            string message)
            => new FailureTraceResult(message);

        public static IFailureTraceResult<TResult> FailWithTrace<TResult>(
            string message)
            => new FailureTraceResult<TResult>(message);

        public static IFailureTraceResult Trace(
            this IFailureTraceResult result,
            string message)
        {
            result.AddTrace(message);
            return result;
        }

        public static IFailureTraceResult<TResult> Trace<TResult>(
            this IFailureTraceResult<TResult> result,
            string message)
        {
            result.AddTrace(message);
            return result;
        }
    }
}