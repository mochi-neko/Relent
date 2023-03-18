#nullable enable
namespace Mochineko.UncertainResult
{
    /// <summary>
    /// Defines a retryable result of a process with a message.
    /// </summary>
    public interface IUncertainRetryableResult
        : IUncertainResult
    {
        string Message { get; }
    }
    
    /// <summary>
    /// Defines a retryable result of a process with a message.
    /// </summary>
    /// <typeparam name="TResult">Type of result value</typeparam>
    public interface IUncertainRetryableResult<TResult>
        : IUncertainResult<TResult>
    {
        string Message { get; }
    }
}