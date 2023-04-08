#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Mochineko.Relent.UncertainResult;

namespace Mochineko.Relent.Resilience.Wrap
{
    internal sealed class PolicyWrap
        : IPolicy
    {
        private readonly IPolicy innerPolicy;
        private readonly IPolicy outerPolicy;

        public PolicyWrap(
            IPolicy innerPolicy,
            IPolicy outerPolicy)
        {
            this.innerPolicy = innerPolicy;
            this.outerPolicy = outerPolicy;
        }

        public async UniTask<IUncertainResult> ExecuteAsync(
            Func<CancellationToken, UniTask<IUncertainResult>> execute,
            CancellationToken cancellationToken)
            => await outerPolicy.ExecuteAsync(
                execute: async innerCancellationToken
                    => await innerPolicy.ExecuteAsync(execute, innerCancellationToken),
                cancellationToken);
    }
    
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

        public async UniTask<IUncertainResult<TResult>> ExecuteAsync(
            Func<CancellationToken, UniTask<IUncertainResult<TResult>>> execute,
            CancellationToken cancellationToken)
            => await outerPolicy.ExecuteAsync(
                execute: async innerCancellationToken
                    => await innerPolicy.ExecuteAsync(execute, innerCancellationToken),
                cancellationToken);
    }
}