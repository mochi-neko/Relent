#nullable enable
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Mochineko.Relent.Result
{
    /// <summary>
    /// Defines a policy to try an asynchronous operation.
    /// </summary>
    public interface IAsyncTryPolicy
    {
        /// <summary>
        /// Executes the asynchronous operation.
        /// </summary>
        /// <returns></returns>
        UniTask<IResult> ExecuteAsync(CancellationToken cancellationToken);
    }

    /// <summary>
    /// Defines a policy to try an asynchronous operation with value.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public interface IAsyncTryPolicy<TResult>
    {
        /// <summary>
        /// Executes the asynchronous operation.
        /// </summary>
        /// <returns></returns>
        UniTask<IResult<TResult>> ExecuteAsync(CancellationToken cancellationToken);
    }
}