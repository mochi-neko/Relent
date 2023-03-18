#nullable enable
namespace Mochineko.UncertainResult
{
    /// <summary>
    /// A factory of <see cref="IUncertainResult"/> and <see cref="IUncertainResult{TResult}"/>.
    /// </summary>
    public static class UncertainResult
    {
        /// <summary>
        /// Creates a <see cref="IUncertainSuccessResult"/>.
        /// </summary>
        /// <returns></returns>
        public static IUncertainSuccessResult Succeed()
            => new UncertainSuccessResult();
        
        /// <summary>
        /// Creates a <see cref="IUncertainRetryableResult"/> with a message.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static IUncertainRetryableResult Retry(string message)
            => new UncertainRetryableResult(message);
        
        /// <summary>
        /// Creates a <see cref="IUncertainFailureResult"/> with a message.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static IUncertainFailureResult Fail(string message)
            => new UncertainFailureResult(message);
        
        /// <summary>
        /// Creates a <see cref="IUncertainSuccessResult{TResult}"/> with a value.
        /// </summary>
        /// <param name="result"></param>
        /// <typeparam name="TResult">Type of result value</typeparam>
        /// <returns></returns>
        public static IUncertainSuccessResult<TResult> Succeed<TResult>(TResult result)
            => new UncertainSuccessResult<TResult>(result);

        /// <summary>
        /// Creates a <see cref="IUncertainRetryableResult{TResult}"/> with a message.
        /// </summary>
        /// <param name="message"></param>
        /// <typeparam name="TResult">Type of result value</typeparam>
        /// <returns></returns>
        public static IUncertainRetryableResult<TResult> Retry<TResult>(string message)
            => new UncertainRetryableResult<TResult>(message);

        /// <summary>
        /// Creates a <see cref="IUncertainFailureResult{TResult}"/> with a message.
        /// </summary>
        /// <param name="message"></param>
        /// <typeparam name="TResult">Type of result value</typeparam>
        /// <returns></returns>
        public static IUncertainFailureResult<TResult> Fail<TResult>(string message)
            => new UncertainFailureResult<TResult>(message);
    }
}