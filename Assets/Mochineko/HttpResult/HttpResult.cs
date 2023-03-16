#nullable enable
namespace Mochineko.HttpResult
{
    /// <summary>
    /// A factory of <see cref="IHttpResult"/> and <see cref="IHttpResult{TResult}"/>.
    /// </summary>
    public static class HttpResult
    {
        /// <summary>
        /// Creates a <see cref="IHttpSuccessResult"/> without value.
        /// </summary>
        /// <returns></returns>
        public static IHttpSuccessResult Succeed()
            => new HttpSuccessResult();
        
        /// <summary>
        /// Creates a <see cref="IHttpRetryableResult"/> with message.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static IHttpRetryableResult Retry(string message)
            => new HttpRetryableResult(message);
        
        /// <summary>
        /// Creates a <see cref="IHttpFailureResult"/> with message.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static IHttpFailureResult Fail(string message)
            => new HttpFailureResult(message);
        
        /// <summary>
        /// Creates a <see cref="IHttpSuccessResult{TResult}"/> with value.
        /// </summary>
        /// <param name="result"></param>
        /// <typeparam name="TResult">Type of result value</typeparam>
        /// <returns></returns>
        public static IHttpSuccessResult<TResult> Succeed<TResult>(TResult result)
            => new HttpSuccessResult<TResult>(result);

        /// <summary>
        /// Creates a <see cref="IHttpRetryableResult{TResult}"/> with message.
        /// </summary>
        /// <param name="message"></param>
        /// <typeparam name="TResult">Type of result value</typeparam>
        /// <returns></returns>
        public static IHttpRetryableResult<TResult> Retry<TResult>(string message)
            => new HttpRetryableResult<TResult>(message);

        /// <summary>
        /// Creates a <see cref="IHttpFailureResult{TResult}"/> with message
        /// </summary>
        /// <param name="message"></param>
        /// <typeparam name="TResult">Type of result value</typeparam>
        /// <returns></returns>
        public static IHttpFailureResult<TResult> Fail<TResult>(string message)
            => new HttpFailureResult<TResult>(message);
    }
}