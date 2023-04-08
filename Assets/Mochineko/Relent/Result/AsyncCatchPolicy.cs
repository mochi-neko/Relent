#nullable enable
using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Mochineko.Relent.Result
{
    internal sealed class AsyncCatchPolicy<TException>
        : IAsyncTryPolicy
        where TException : Exception
    {
        private readonly IAsyncTryPolicy tryPolicy;
        private readonly Func<Exception, string> messageProvider;

        public AsyncCatchPolicy(
            IAsyncTryPolicy tryPolicy,
            Func<Exception, string> messageProvider)
        {
            this.tryPolicy = tryPolicy;
            this.messageProvider = messageProvider;
        }

        public async UniTask<IResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await tryPolicy.ExecuteAsync(cancellationToken);
            }
            catch (TException exception)
            {
                return Results.FailWithTrace(messageProvider.Invoke(exception));
            }
        }
    }

    internal sealed class AsyncCatchPolicy<TResult, TException>
        : IAsyncTryPolicy<TResult>
        where TException : Exception
    {
        private readonly IAsyncTryPolicy<TResult> tryPolicy;
        private readonly Func<Exception, string> messageProvider;

        public AsyncCatchPolicy(
            IAsyncTryPolicy<TResult> tryPolicy,
            Func<Exception, string> messageProvider)
        {
            this.tryPolicy = tryPolicy;
            this.messageProvider = messageProvider;
        }

        public async UniTask<IResult<TResult>> ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await tryPolicy.ExecuteAsync(cancellationToken);
            }
            catch (TException exception)
            {
                return Results.FailWithTrace<TResult>(messageProvider.Invoke(exception));
            }
        }
    }
}