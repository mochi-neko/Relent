#nullable enable
using System;

namespace Mochineko.Relent.Result
{
    internal sealed class CatchPolicy<TException>
        : ITryPolicy
        where TException : Exception
    {
        private readonly ITryPolicy tryPolicy;
        private readonly Func<Exception, string> messageProvider;

        public CatchPolicy(
            ITryPolicy tryPolicy,
            Func<Exception, string> messageProvider)
        {
            this.tryPolicy = tryPolicy;
            this.messageProvider = messageProvider;
        }

        public IResult Execute()
        {
            try
            {
                return tryPolicy.Execute();
            }
            catch (TException exception)
            {
                return ResultFactory.Fail(messageProvider.Invoke(exception));
            }
        }
    }

    internal sealed class CatchPolicy<TResult, TException>
        : ITryPolicy<TResult>
        where TException : Exception
    {
        private readonly ITryPolicy<TResult> tryPolicy;
        private readonly Func<Exception, string> messageProvider;

        public CatchPolicy(
            ITryPolicy<TResult> tryPolicy,
            Func<Exception, string> messageProvider)
        {
            this.tryPolicy = tryPolicy;
            this.messageProvider = messageProvider;
        }

        public IResult<TResult> Execute()
        {
            try
            {
                return tryPolicy.Execute();
            }
            catch (TException exception)
            {
                return ResultFactory.Fail<TResult>(messageProvider.Invoke(exception));
            }
        }
    }
}