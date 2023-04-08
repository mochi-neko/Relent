#nullable enable
using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Mochineko.Relent.UncertainResult
{
    internal sealed class UncertainAsyncTryPolicy
        : IUncertainAsyncTryPolicy
    {
        private readonly Func<CancellationToken, UniTask> operation;

        public UncertainAsyncTryPolicy(Func<CancellationToken, UniTask> operation)
        {
            this.operation = operation;
        }

        public async UniTask<IUncertainResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            await operation.Invoke(cancellationToken);
            return UncertainResults.Succeed();
        }
    }

    internal sealed class UncertainAsyncTryPolicy<TResult>
        : IUncertainAsyncTryPolicy<TResult>
    {
        private readonly Func<CancellationToken, UniTask<TResult>> operation;

        public UncertainAsyncTryPolicy(Func<CancellationToken, UniTask<TResult>> operation)
        {
            this.operation = operation;
        }

        public async UniTask<IUncertainResult<TResult>> ExecuteAsync(CancellationToken cancellationToken)
        {
            var result = await operation.Invoke(cancellationToken);
            return UncertainResults.Succeed(result);
        }
    }
}