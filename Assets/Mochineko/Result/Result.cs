#nullable enable
namespace Mochineko.Result
{
    public static class Result
    {
        public static IResult Ok()
            => new SuccessResult();
        
        public static IResult Fail(string message)
            => new FailureResult(message);

        public static IResult<TResult> Ok<TResult>(TResult result)
            => new SuccessResult<TResult>(result);

        public static IResult<TResult> Fail<TResult>(string message)
            => new FailureResult<TResult>(message);

        public static TResult Unwrap<TResult>(this IResult<TResult> result)
        {
            if (result is ISuccessResult<TResult> success)
            {
                return success.Result;
            }
            else
            {
                throw new FailedToUnwrapResultException($"Failed to unwrap {result.GetType()} into success result.");
            }
        }
    }
}