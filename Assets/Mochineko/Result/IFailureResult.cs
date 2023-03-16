#nullable enable
namespace Mochineko.Result
{
    public interface IFailureResult<TResult>
        : IResult<TResult>
    {
        string Message { get; }
    }
}