#nullable enable
using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Mochineko.Relent.UncertainResult
{
    internal sealed class UncertainAsyncCatchFailurePolicy<TException>
        : IUncertainAsyncTryPolicy
        where TException : Exception
    {
        private readonly IUncertainAsyncTryPolicy tryPolicy;
        private readonly Func<Exception, string> messageProvider;

        public UncertainAsyncCatchFailurePolicy(
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
                return UncertainResults.FailWithTrace(messageProvider.Invoke(exception));
            }
        }
    }

    internal sealed class UncertainAsyncCatchFailurePolicy<TResult, TException>
        : IUncertainAsyncTryPolicy<TResult>
        where TException : Exception
    {
        private readonly IUncertainAsyncTryPolicy<TResult> tryPolicy;
        private readonly Func<Exception, string> messageProvider;

        public UncertainAsyncCatchFailurePolicy(
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
                return UncertainResults.FailWithTrace<TResult>(messageProvider.Invoke(exception));
            }
        }
    }
}