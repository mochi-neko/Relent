#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using Mochineko.UncertainResult;

namespace Mochineko.Resilience
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
        Task<IUncertainResult> ExecuteAsync(
            Func<CancellationToken, Task<IUncertainResult>> execute,
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
        Task<IUncertainResult<TResult>> ExecuteAsync(
            Func<CancellationToken, Task<IUncertainResult<TResult>>> execute,
            CancellationToken cancellationToken);
    }
}