#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using Mochineko.UncertainResult;

namespace Mochineko.Resilience.Wrap
{
    internal sealed class PolicyWrap<TResult>
        : IPolicy<TResult>
    {
        private readonly IPolicy<TResult> innerPolicy;
        private readonly IPolicy<TResult> outerPolicy;

        public PolicyWrap(
            IPolicy<TResult> innerPolicy,
            IPolicy<TResult> outerPolicy)
        {
            this.innerPolicy = innerPolicy;
            this.outerPolicy = outerPolicy;
        }

        public async Task<IUncertainResult<TResult>> ExecuteAsync(
            Func<CancellationToken, Task<IUncertainResult<TResult>>> execute,
            CancellationToken cancellationToken)
            => await outerPolicy.ExecuteAsync(
                execute: async innerCancellationToken
                    => await innerPolicy.ExecuteAsync(execute, innerCancellationToken),
                cancellationToken);
    }
}