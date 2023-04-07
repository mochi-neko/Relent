#nullable enable
using System;

namespace Mochineko.Relent.Result
{
    internal sealed class FinalizePolicy
        : ITryPolicy
    {
        private readonly ITryPolicy tryPolicy;
        private readonly Action finalizer;

        public FinalizePolicy(
            ITryPolicy tryPolicy,
            Action finalizer)
        {
            this.tryPolicy = tryPolicy;
            this.finalizer = finalizer;
        }

        public IResult Execute()
        {
            try
            {
                return tryPolicy.Execute();
            }
            finally
            {
                finalizer.Invoke();
            }
        }
    }
    
    internal sealed class FinalizePolicy<TResult>
        : ITryPolicy<TResult>
    {
        private readonly ITryPolicy<TResult> tryPolicy;
        private readonly Action finalizer;

        public FinalizePolicy(
            ITryPolicy<TResult> tryPolicy,
            Action finalizer)
        {
            this.tryPolicy = tryPolicy;
            this.finalizer = finalizer;
        }

        public IResult<TResult> Execute()
        {
            try
            {
                return tryPolicy.Execute();
            }
            finally
            {
                finalizer.Invoke();
            }
        }
    }
}