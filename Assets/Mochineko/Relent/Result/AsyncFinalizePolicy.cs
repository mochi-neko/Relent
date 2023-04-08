#nullable enable
using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Mochineko.Relent.Result
{
    internal sealed class AsyncFinalizePolicy
        : IAsyncTryPolicy
    {
        private readonly IAsyncTryPolicy tryPolicy;
        private readonly Func<UniTask> finalizer;

        public AsyncFinalizePolicy(
            IAsyncTryPolicy tryPolicy,
            Func<UniTask> finalizer)
        {
            this.tryPolicy = tryPolicy;
            this.finalizer = finalizer;
        }

        public async UniTask<IResult> ExecuteAsync(CancellationToken cancellationToken)
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

    internal sealed class AsyncFinalizePolicy<TResult>
        : IAsyncTryPolicy<TResult>
    {
        private readonly IAsyncTryPolicy<TResult> tryPolicy;
        private readonly Func<UniTask> finalizer;

        public AsyncFinalizePolicy(
            IAsyncTryPolicy<TResult> tryPolicy,
            Func<UniTask> finalizer)
        {
            this.tryPolicy = tryPolicy;
            this.finalizer = finalizer;
        }

        public async UniTask<IResult<TResult>> ExecuteAsync(CancellationToken cancellationToken)
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