#nullable enable
namespace Mochineko.UncertainResult
{
    /// <summary>
    /// Defines a failure result of a process with a message.
    /// </summary>
    public interface IUncertainFailureResult
        : IUncertainResult
    {
        string Message { get; }
    }

    /// <summary>
    /// Defines a failure result of a process with a message.
    /// </summary>
    /// <typeparam name="TResult">Type of result value</typeparam>
    public interface IUncertainFailureResult<TResult>
        : IUncertainResult<TResult>
    {
        string Message { get; }
    }
}