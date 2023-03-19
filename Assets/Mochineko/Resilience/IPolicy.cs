#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using Mochineko.UncertainResult;

namespace Mochineko.Resilience
{
    public interface IPolicy
    {
        Task<IUncertainResult> ExecuteAsync(
            Func<CancellationToken, Task<IUncertainResult>> execute,
            CancellationToken cancellationToken);
    }
    
    public interface IPolicy<TResult>
    {
        Task<IUncertainResult<TResult>> ExecuteAsync(
            Func<CancellationToken, Task<IUncertainResult<TResult>>> execute,
            CancellationToken cancellationToken);
    }
}