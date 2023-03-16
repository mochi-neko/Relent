#nullable enable
namespace Mochineko.Result
{
    public interface ISuccessResult
        : IResult
    {
    }

    public interface ISuccessResult<TResult>
        : IResult<TResult>
    {
        TResult Result { get; }
    }
}