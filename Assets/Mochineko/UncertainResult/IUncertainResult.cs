#nullable enable
namespace Mochineko.UncertainResult
{
    /// <summary>
    /// Defines an uncertain result of a process.
    /// </summary>
    public interface IUncertainResult
    {
        bool Success { get; }
        bool Retryable { get; }
        bool Failure { get; }
    }
    
    /// <summary>
    /// Defines an uncertain result of a process.
    /// The type argument may seem to do nothing,
    ///  but prevents cast to wrong types.
    /// </summary>
    /// <typeparam name="TResult">Type of result value</typeparam>
    public interface IUncertainResult<TResult>
    {
        bool Success { get; }
        bool Retryable { get; }
        bool Failure { get; }
    }
}