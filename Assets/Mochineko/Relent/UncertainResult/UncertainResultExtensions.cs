#nullable enable
using System;

namespace Mochineko.Relent.UncertainResult
{
    public static class UncertainResultExtensions
    {
        public static TResult Unwrap<TResult>(this IUncertainResult<TResult> result)
        {
            if (result is IUncertainSuccessResult<TResult> success)
            {
                return success.Result;
            }
            else
            {
                throw new InvalidOperationException("Failed to unwrap result.");
            }
        }

        public static string ExtractMessage(this IUncertainResult result)
        {
            if (result is IUncertainRetryableResult retryable)
            {
                return retryable.Message;
            }
            else if (result is IUncertainFailureResult failure)
            {
                return failure.Message;
            }
            else
            {
                throw new InvalidOperationException("Failed to extract message from failure result.");
            }
        }

        public static string ExtractMessage<TResult>(this IUncertainResult<TResult> result)
        {
            if (result is IUncertainRetryableResult<TResult> retryable)
            {
                return retryable.Message;
            }
            else if (result is IUncertainFailureResult<TResult> failure)
            {
                return failure.Message;
            }
            else
            {
                throw new InvalidOperationException("Failed to extract message from failure result.");
            }
        }

        public static IUncertainResult<TResult> ToResult<TResult>(this TResult result)
            => UncertainResults.Succeed(result);

        public static IUncertainResult Try<TException>(
            Action operation,
            Action? finalizer = null)
            where TException : Exception
        {
            try
            {
                operation.Invoke();
                return UncertainResults.Succeed();
            }
            catch (TException exception)
            {
                return UncertainResults.Fail(
                    $"Failed to execute operation because of {exception}.");
            }
            finally
            {
                finalizer?.Invoke();
            }
        }
        
        public static IUncertainTraceRetryableResult Trace(
            this IUncertainTraceRetryableResult result,
            string message)
        {
            result.AddTrace(message);
            return result;
        }

        public static IUncertainTraceRetryableResult<TResult> Trace<TResult>(
            this IUncertainTraceRetryableResult<TResult> result,
            string message)
        {
            result.AddTrace(message);
            return result;
        }
        
        public static IUncertainTraceFailureResult Trace(
            this IUncertainTraceFailureResult result,
            string message)
        {
            result.AddTrace(message);
            return result;
        }

        public static IUncertainTraceFailureResult<TResult> Trace<TResult>(
            this IUncertainTraceFailureResult<TResult> result,
            string message)
        {
            result.AddTrace(message);
            return result;
        }
    }
}