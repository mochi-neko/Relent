#nullable enable
using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Mochineko.Relent.UncertainResult
{
    internal sealed class UncertainAsyncCatchRetryablePolicy<TException>
        : IUncertainAsyncTryPolicy
        where TException : Exception
    {
        private readonly IUncertainAsyncTryPolicy tryPolicy;
        private readonly Func<Exception, string> messageProvider;

        public UncertainAsyncCatchRetryablePolicy(
            IUncertainAsyncTryPolicy tryPolicy,
            Func<Exception, string> messageProvider)
        {
            this.tryPolicy = tryPolicy;
            this.messageProvider = messageProvider;
        }

        public async UniTask<IUncertainResult> ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await tryPolicy.ExecuteAsync(cancellationToken);
            }
            catch (TException exception)
            {
                return UncertainResults.RetryWithTrace(messageProvider.Invoke(exception));
            }
        }
    }

    internal sealed class UncertainAsyncCatchRetryablePolicy<TResult, TException>
        : IUncertainAsyncTryPolicy<TResult>
        where TException : Exception
    {
        private readonly IUncertainAsyncTryPolicy<TResult> tryPolicy;
        private readonly Func<Exception, string> messageProvider;

        public UncertainAsyncCatchRetryablePolicy(
            IUncertainAsyncTryPolicy<TResult> tryPolicy,
            Func<Exception, string> messageProvider)
        {
            this.tryPolicy = tryPolicy;
            this.messageProvider = messageProvider;
        }

        public async UniTask<IUncertainResult<TResult>> ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await tryPolicy.ExecuteAsync(cancellationToken);
            }
            catch (TException exception)
            {
                return UncertainResults.RetryWithTrace<TResult>(messageProvider.Invoke(exception));
            }
        }
    }
}