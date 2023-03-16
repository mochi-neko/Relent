#nullable enable
namespace Mochineko.Result
{
    public static class Result
    {
        public static IResult<TResult> Ok<TResult>(TResult result)
            => new SuccessResult<TResult>(result);

        public static IResult<TResult> Fail<TResult>(string message)
            => new FailureResult<TResult>(message);
    }
}