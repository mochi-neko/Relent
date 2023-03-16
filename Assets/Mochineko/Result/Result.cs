#nullable enable
namespace Mochineko.Result
{
    /// <summary>
    /// A factory of <see cref="IResult{TResult}"/>.
    /// </summary>
    public static class Result
    {
        /// <summary>
        /// Creates a <see cref="ISuccessResult{TResult}"/> with value.
        /// </summary>
        /// <param name="result"></param>
        /// <typeparam name="TResult">Type of result value</typeparam>
        /// <returns></returns>
        public static ISuccessResult<TResult> Succeed<TResult>(TResult result)
            => new SuccessResult<TResult>(result);

        /// <summary>
        /// Creates a <see cref="IFailureResult{TResult}"/> with message.
        /// </summary>
        /// <param name="message"></param>
        /// <typeparam name="TResult">Type of result value</typeparam>
        /// <returns></returns>
        public static IFailureResult<TResult> Fail<TResult>(string message)
            => new FailureResult<TResult>(message);
    }
}