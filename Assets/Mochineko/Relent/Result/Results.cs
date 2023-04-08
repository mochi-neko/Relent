#nullable enable
namespace Mochineko.Relent.Result
{
    /// <summary>
    /// A factory of <see cref="IResult"/> and <see cref="IResult{TResult}"/>.
    /// </summary>
    public static class Results
    {
        /// <summary>
        /// Creates a <see cref="ISuccessResult"/>.
        /// </summary>
        /// <returns></returns>
        public static ISuccessResult Succeed()
            => SuccessResult.Instance;
        
        /// <summary>
        /// Creates a <see cref="IFailureResult"/> with message.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static IFailureResult Fail(string message)
            => new FailureResult(message);
        
        public static IFailureTraceResult FailWithTrace(
            string message)
            => new FailureTraceResult(message);

        /// <summary>
        /// Creates a <see cref="ISuccessResult{TResult}"/> with a value.
        /// </summary>
        /// <param name="result"></param>
        /// <typeparam name="TResult">Type of result value</typeparam>
        /// <returns></returns>
        public static ISuccessResult<TResult> Succeed<TResult>(TResult result)
            => new SuccessResult<TResult>(result);

        /// <summary>
        /// Creates a <see cref="IFailureResult{TResult}"/> with a message.
        /// </summary>
        /// <param name="message"></param>
        /// <typeparam name="TResult">Type of result value</typeparam>
        /// <returns></returns>
        public static IFailureResult<TResult> Fail<TResult>(string message)
            => new FailureResult<TResult>(message);

        public static IFailureTraceResult<TResult> FailWithTrace<TResult>(
            string message)
            => new FailureTraceResult<TResult>(message);
    }
}