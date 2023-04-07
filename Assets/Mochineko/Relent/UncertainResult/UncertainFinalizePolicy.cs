#nullable enable
using System;

namespace Mochineko.Relent.UncertainResult
{
    internal sealed class UncertainFinalizePolicy
        : IUncertainTryPolicy
    {
        private readonly IUncertainTryPolicy tryPolicy;
        private readonly Action finalizer;

        public UncertainFinalizePolicy(
            IUncertainTryPolicy tryPolicy,
            Action finalizer)
        {
            this.tryPolicy = tryPolicy;
            this.finalizer = finalizer;
        }

        public IUncertainResult Execute()
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

    internal sealed class UncertainFinalizePolicy<TResult>
        : IUncertainTryPolicy<TResult>
    {
        private readonly IUncertainTryPolicy<TResult> tryPolicy;
        private readonly Action finalizer;

        public UncertainFinalizePolicy(
            IUncertainTryPolicy<TResult> tryPolicy,
            Action finalizer)
        {
            this.tryPolicy = tryPolicy;
            this.finalizer = finalizer;
        }

        public IUncertainResult<TResult> Execute()
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