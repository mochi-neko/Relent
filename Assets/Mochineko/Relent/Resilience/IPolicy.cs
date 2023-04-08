#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Mochineko.Relent.UncertainResult;

namespace Mochineko.Relent.Resilience
{
    /// <summary>
    /// Defines a policy that can be applied to an operation with no result value.
    /// </summary>
    public interface IPolicy
    {
        /// <summary>
        /// Executes the operation with the policy.
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        UniTask<IUncertainResult> ExecuteAsync(
            Func<CancellationToken, UniTask<IUncertainResult>> execute,
            CancellationToken cancellationToken);
    }
    
    /// <summary>
    /// Defines a policy that can be applied to an operation with result value.
    /// </summary>
    /// <typeparam name="TResult">Type of result value</typeparam>
    public interface IPolicy<TResult>
    {
        /// <summary>
        /// Executes the operation with the policy.
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="cancellationToken">Type of result value</param>
        /// <returns></returns>
        UniTask<IUncertainResult<TResult>> ExecuteAsync(
            Func<CancellationToken, UniTask<IUncertainResult<TResult>>> execute,
            CancellationToken cancellationToken);
    }
}