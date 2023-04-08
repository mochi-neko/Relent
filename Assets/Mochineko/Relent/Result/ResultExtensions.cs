#nullable enable
using System;

namespace Mochineko.Relent.Result
{
    /// <summary>
    /// Extensions for <see cref="IResult"/> and <see cref="IResult{TResult}"/>.
    /// </summary>
    public static class ResultExtensions
    {
        /// <summary>
        /// Unwraps <see cref="IResult{T}"/> to <typeparamref name="TResult"/>.
        /// Notice that this method throws an exception if the result is failure.
        /// </summary>
        /// <param name="result"></param>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Failure results</exception>
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

        /// <summary>
        /// Extracts message from <see cref="IFailureResult"/>.
        /// Notice that this method throws an exception if the result is success.
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Success results</exception>
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

        /// <summary>
        /// Extracts message from <see cref="IFailureResult{TResult}"/>.
        /// Notice that this method throws an exception if the result is success.
        /// </summary>
        /// <param name="result"></param>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Success results</exception>
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

        /// <summary>
        /// Converts <typeparamref name="TResult"/> to <see cref="IResult{TResult}"/>.
        /// </summary>
        /// <param name="result"></param>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public static IResult<TResult> ToResult<TResult>(this TResult result)
            => Results.Succeed(result);

        /// <summary>
        /// Traces a message with <see cref="IFailureTraceResult"/>.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static IFailureTraceResult Trace(
            this IFailureTraceResult result,
            string message)
        {
            result.AddTrace(message);
            return result;
        }

        /// <summary>
        /// Traces a message with <see cref="IFailureTraceResult{TResult}"/>.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="message"></param>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public static IFailureTraceResult<TResult> Trace<TResult>(
            this IFailureTraceResult<TResult> result,
            string message)
        {
            result.AddTrace(message);
            return result;
        }
    }
}