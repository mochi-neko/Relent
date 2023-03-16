#nullable enable
namespace Mochineko.Result
{
    /// <summary>
    /// Defines a success result of a process.
    /// </summary>
    public interface ISuccessResult
        : IResult
    {
    }

    /// <summary>
    /// Defines a success result of a process with a value.
    /// </summary>
    /// <typeparam name="TResult">Type of result value</typeparam>
    public interface ISuccessResult<TResult>
        : IResult<TResult>
    {
        TResult Result { get; }
    }
}