#nullable enable
using System;

namespace Mochineko.Relent.Result
{
    internal sealed class TryPolicy
        : ITryPolicy
    {
        private readonly Action operation;

        public TryPolicy(Action operation)
        {
            this.operation = operation;
        }

        public IResult Execute()
        {
            operation.Invoke();
            return ResultFactory.Succeed();
        }
    }
    
    internal sealed class TryPolicy<TResult>
        : ITryPolicy<TResult>
    {
        private readonly Func<TResult> operation;

        public TryPolicy(Func<TResult> operation)
        {
            this.operation = operation;
        }

        public IResult<TResult> Execute()
        {
            var result = operation.Invoke();
            return ResultFactory.Succeed(result);
        }
    }
}