#nullable enable
namespace Mochineko.UncertainResult
{
    /// <summary>
    /// Defines a success result of a process.
    /// </summary>
    public interface IUncertainSuccessResult
        : IUncertainResult
    {
    }

    /// <summary>
    /// Defines a success result of a process with a value.
    /// </summary>
    /// <typeparam name="TResult">Type of result value</typeparam>
    public interface IUncertainSuccessResult<TResult>
        : IUncertainResult<TResult>
    {
        TResult Result { get; }
    }
}