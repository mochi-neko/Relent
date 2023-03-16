#nullable enable
namespace Mochineko.Result
{
    public interface IResult<TResult>
    {
        bool Success { get; }
        bool Failure { get; }
    }
}