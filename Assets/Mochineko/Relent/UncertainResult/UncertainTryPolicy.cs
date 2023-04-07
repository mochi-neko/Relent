#nullable enable
using System;

namespace Mochineko.Relent.UncertainResult
{
    internal sealed class UncertainTryPolicy
        : IUncertainTryPolicy
    {
        private readonly Action operation;

        public UncertainTryPolicy(Action operation)
        {
            this.operation = operation;
        }

        public IUncertainResult Execute()
        {
            operation.Invoke();
            return UncertainResultFactory.Succeed();
        }
    }

    internal sealed class UncertainTryPolicy<TResult>
        : IUncertainTryPolicy<TResult>
    {
        private readonly Func<TResult> operation;

        public UncertainTryPolicy(Func<TResult> operation)
        {
            this.operation = operation;
        }

        public IUncertainResult<TResult> Execute()
        {
            var result = operation.Invoke();
            return UncertainResultFactory.Succeed(result);
        }
    }
}