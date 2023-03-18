#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using Mochineko.Result;
using Mochineko.UncertainResult;

namespace Mochineko.Resilience
{
    public interface IPolicy<TResult>
    {
        Task<IResult<TResult>> ExecuteAsync(
            Func<CancellationToken, Task<IUncertainResult<TResult>>> execute,
            CancellationToken cancellationToken);
    }
}