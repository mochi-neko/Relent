#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using Mochineko.HttpResult;
using Mochineko.Result;

namespace Mochineko.Resilience
{
    public interface IPolicy<TResult>
    {
        Task<IResult<TResult>> ExecuteAsync(
            Func<CancellationToken, Task<IHttpResult<TResult>>> execute,
            CancellationToken cancellationToken);
    }
}