#nullable enable
namespace Mochineko.Result
{
    public interface IFailureResult
        : IResult
    {
        string Message { get; }
    }

    public interface IFailureResult<TResult>
        : IResult<TResult>
    {
        string Message { get; }
    }
}