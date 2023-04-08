#nullable enable
using System;

namespace Mochineko.Relent.UncertainResult
{
    internal sealed class UncertainCatchRetryablePolicy<TException>
        : IUncertainTryPolicy
        where TException : Exception
    {
        private readonly IUncertainTryPolicy tryPolicy;
        private readonly Func<Exception, string> messageProvider;

        public UncertainCatchRetryablePolicy(
            IUncertainTryPolicy tryPolicy,
            Func<Exception, string> messageProvider)
        {
            this.tryPolicy = tryPolicy;
            this.messageProvider = messageProvider;
        }

        public IUncertainResult Execute()
        {
            try
            {
                return tryPolicy.Execute();
            }
            catch (TException exception)
            {
                return UncertainResults.Retry(messageProvider.Invoke(exception));
            }
        }
    }

    internal sealed class UncertainCatchRetryablePolicy<TResult, TException>
        : IUncertainTryPolicy<TResult>
        where TException : Exception
    {
        private readonly IUncertainTryPolicy<TResult> tryPolicy;
        private readonly Func<Exception, string> messageProvider;

        public UncertainCatchRetryablePolicy(
            IUncertainTryPolicy<TResult> tryPolicy,
            Func<Exception, string> messageProvider)
        {
            this.tryPolicy = tryPolicy;
            this.messageProvider = messageProvider;
        }

        public IUncertainResult<TResult> Execute()
        {
            try
            {
                return tryPolicy.Execute();
            }
            catch (TException exception)
            {
                return UncertainResults.Retry<TResult>(messageProvider.Invoke(exception));
            }
        }
    }
}