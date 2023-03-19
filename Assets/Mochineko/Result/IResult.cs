#nullable enable
namespace Mochineko.Result
{
    /// <summary>
    /// Defines a result of an operation.
    /// </summary>
    public interface IResult
    {
        /// <summary>
        /// Whether the operation was success.
        /// </summary>
        bool Success { get; }
        /// <summary>
        /// Whether the operation was failure.
        /// </summary>
        bool Failure { get; }
    }

    /// <summary>
    /// Defines a result of an operation.
    /// The type argument prevents to confuse result types.
    /// </summary>
    /// <typeparam name="TResult">Type of result value</typeparam>
    public interface IResult<TResult>
    {
        /// <summary>
        /// Whether the operation was success.
        /// </summary>
        bool Success { get; }
        /// <summary>
        /// Whether the operation was failure.
        /// </summary>
        bool Failure { get; }
    }
}