#nullable enable
using System;

namespace Mochineko.Relent.UncertainResult
{
    /// <summary>
    /// Extensions for <see cref="IUncertainResult"/> and <see cref="IUncertainResult{TResult}"/>.
    /// </summary>
    public static class UncertainResultExtensions
    {
        /// <summary>
        /// Unwraps <typeparamref name="TResult"/> to <see cref="IUncertainSuccessResult"/>.
        /// Note that this method throws <see cref="InvalidOperationException"/> if the result is not success.
        /// </summary>
        /// <param name="result"></param>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Not success result</exception>
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

        /// <summary>
        /// Extracts message from <see cref="IUncertainRetryableResult"/> or <see cref="IUncertainFailureResult"/>.
        /// Note that this method throws <see cref="InvalidOperationException"/> if the result is not retryable or failure.
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Success result</exception>
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

        /// <summary>
        /// Extracts message from <see cref="IUncertainRetryableResult{TResult}"/> or <see cref="IUncertainFailureResult{TResult}"/>.
        /// Note that this method throws <see cref="InvalidOperationException"/> if the result is not retryable or failure.
        /// </summary>
        /// <param name="result"></param>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Success result</exception>
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

        /// <summary>
        /// Converts <typeparamref name="TResult"/> to <see cref="IUncertainSuccessResult{TResult}"/>.
        /// </summary>
        /// <param name="result"></param>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public static IUncertainResult<TResult> ToResult<TResult>(this TResult result)
            => UncertainResults.Succeed(result);
        
        /// <summary>
        /// Traces a message to <see cref="IUncertainTraceRetryableResult"/>.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static IUncertainTraceRetryableResult Trace(
            this IUncertainTraceRetryableResult result,
            string message)
        {
            result.AddTrace(message);
            return result;
        }

        /// <summary>
        /// Traces a message to <see cref="IUncertainTraceRetryableResult{TResult}"/>.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="message"></param>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public static IUncertainTraceRetryableResult<TResult> Trace<TResult>(
            this IUncertainTraceRetryableResult<TResult> result,
            string message)
        {
            result.AddTrace(message);
            return result;
        }
        
        /// <summary>
        /// Traces a message to <see cref="IUncertainTraceFailureResult"/>.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static IUncertainTraceFailureResult Trace(
            this IUncertainTraceFailureResult result,
            string message)
        {
            result.AddTrace(message);
            return result;
        }

        /// <summary>
        /// Traces a message to <see cref="IUncertainTraceFailureResult{TResult}"/>.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="message"></param>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public static IUncertainTraceFailureResult<TResult> Trace<TResult>(
            this IUncertainTraceFailureResult<TResult> result,
            string message)
        {
            result.AddTrace(message);
            return result;
        }
    }
}