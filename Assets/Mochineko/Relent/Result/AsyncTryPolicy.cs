#nullable enable
using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Mochineko.Relent.Result
{
    internal sealed class AsyncTryPolicy
        : IAsyncTryPolicy
    {
        private readonly Func<CancellationToken, UniTask> operation;

        public AsyncTryPolicy(Func<CancellationToken, UniTask> operation)
        {
            this.operation = operation;
        }

        public async UniTask<IResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            await operation.Invoke(cancellationToken);
            return Results.Succeed();
        }
    }

    internal sealed class AsyncTryPolicy<TResult>
        : IAsyncTryPolicy<TResult>
    {
        private readonly Func<CancellationToken, UniTask<TResult>> operation;

        public AsyncTryPolicy(Func<CancellationToken, UniTask<TResult>> operation)
        {
            this.operation = operation;
        }

        public async UniTask<IResult<TResult>> ExecuteAsync(CancellationToken cancellationToken)
        {
            var result = await operation.Invoke(cancellationToken);
            return Results.Succeed(result);
        }
    }
}