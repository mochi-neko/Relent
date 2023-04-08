#nullable enable
namespace Mochineko.Relent.UncertainResult
{
    /// <summary>
    /// Defines a policy to try an operation.
    /// </summary>
    public interface IUncertainTryPolicy
    {
        /// <summary>
        /// Executes the operation.
        /// </summary>
        /// <returns></returns>
        IUncertainResult Execute();
    }

    /// <summary>
    /// Defines a policy to try an operation with value.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public interface IUncertainTryPolicy<TResult>
    {
        /// <summary>
        /// Executes the operation.
        /// </summary>
        /// <returns></returns>
        IUncertainResult<TResult> Execute();
    }
}