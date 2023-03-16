#nullable enable
namespace Mochineko.Result
{
    /// <summary>
    /// Defines a success result with value.
    /// </summary>
    /// <typeparam name="TResult">Type of result value</typeparam>
    public interface ISuccessResult<TResult>
        : IResult<TResult>
    {
        TResult Result { get; }
    }
}