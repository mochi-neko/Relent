#nullable enable
using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Mochineko.Relent.UncertainResult
{
    internal sealed class UncertainAsyncFinalizePolicy
        : IUncertainAsyncTryPolicy
    {
        private readonly IUncertainAsyncTryPolicy tryPolicy;
        private readonly Func<UniTask> finalizer;

        public UncertainAsyncFinalizePolicy(
            IUncertainAsyncTryPolicy tryPolicy,
            Func<UniTask> finalizer)
        {
            this.tryPolicy = tryPolicy;
            this.finalizer = finalizer;
        }

        public async UniTask<IUncertainResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await tryPolicy.ExecuteAsync(cancellationToken);
            }
            finally
            {
                await finalizer.Invoke();
            }
        }
    }

    internal sealed class UncertainAsyncFinalizePolicy<TResult>
        : IUncertainAsyncTryPolicy<TResult>
    {
        private readonly IUncertainAsyncTryPolicy<TResult> tryPolicy;
        private readonly Func<UniTask> finalizer;

        public UncertainAsyncFinalizePolicy(
            IUncertainAsyncTryPolicy<TResult> tryPolicy,
            Func<UniTask> finalizer)
        {
            this.tryPolicy = tryPolicy;
            this.finalizer = finalizer;
        }

        public async UniTask<IUncertainResult<TResult>> ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await tryPolicy.ExecuteAsync(cancellationToken);
            }
            finally
            {
                await finalizer.Invoke();
            }
        }
    }
}