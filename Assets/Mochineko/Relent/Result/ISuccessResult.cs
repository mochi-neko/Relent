#nullable enable
namespace Mochineko.Relent.Result
{
    /// <summary>
    /// Defines a success result of an operation.
    /// </summary>
    public interface ISuccessResult
        : IResult
    {
    }

    /// <summary>
    /// Defines a success result of an operation with a value.
    /// </summary>
    /// <typeparam name="TResult">Type of result value</typeparam>
    public interface ISuccessResult<TResult>
        : IResult<TResult>
    {
        /// <summary>
        /// Result value.
        /// </summary>
        TResult Result { get; }
    }
}