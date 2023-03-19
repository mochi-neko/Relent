#nullable enable
namespace Mochineko.UncertainResult
{
    /// <summary>
    /// Defines a success result of an operation.
    /// </summary>
    public interface IUncertainSuccessResult
        : IUncertainResult
    {
    }

    /// <summary>
    /// Defines a success result of an operation with a value.
    /// </summary>
    /// <typeparam name="TResult">Type of result value</typeparam>
    public interface IUncertainSuccessResult<TResult>
        : IUncertainResult<TResult>
    {
        /// <summary>
        /// Result value.
        /// </summary>
        TResult Result { get; }
    }
}