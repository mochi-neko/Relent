#nullable enable
namespace Mochineko.UncertainResult
{
    /// <summary>
    /// Defines an uncertain result of an operation.
    /// </summary>
    public interface IUncertainResult
    {
        /// <summary>
        /// Whether the operation was success.
        /// </summary>
        bool Success { get; }
        /// <summary>
        /// Whether the operation was retryable.
        /// </summary>
        bool Retryable { get; }
        /// <summary>
        /// Whether the operation was failure.
        /// </summary>
        bool Failure { get; }
    }
    
    /// <summary>
    /// Defines an uncertain result of an operation.
    /// The type argument prevents to confuse result types.
    /// </summary>
    /// <typeparam name="TResult">Type of result value</typeparam>
    public interface IUncertainResult<TResult>
    {
        /// <summary>
        /// Whether the operation was success.
        /// </summary>
        bool Success { get; }
        /// <summary>
        /// Whether the operation was retryable.
        /// </summary>
        bool Retryable { get; }
        /// <summary>
        /// Whether the operation was failure.
        /// </summary>
        bool Failure { get; }
    }
}