#nullable enable
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Mochineko.Relent.UncertainResult
{
    /// <summary>
    /// Defines a policy to try an asynchronous operation.
    /// </summary>
    public interface IUncertainAsyncTryPolicy
    {
        /// <summary>
        /// Executes the asynchronous operation.
        /// </summary>
        /// <returns></returns>
        UniTask<IUncertainResult> ExecuteAsync(CancellationToken cancellationToken);
    }

    /// <summary>
    /// Defines a policy to try an asynchronous operation with value.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public interface IUncertainAsyncTryPolicy<TResult>
    {
        /// <summary>
        /// Executes the asynchronous operation.
        /// </summary>
        /// <returns></returns>
        UniTask<IUncertainResult<TResult>> ExecuteAsync(CancellationToken cancellationToken);
    }
}