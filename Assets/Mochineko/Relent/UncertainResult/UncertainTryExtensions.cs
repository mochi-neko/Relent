#nullable enable
using System;

namespace Mochineko.Relent.UncertainResult
{
    public static class UncertainTryExtensions
    {
        public static IUncertainTryPolicy Try(Action operation)
            => new UncertainTryPolicy(operation);

        public static IUncertainTryPolicy CatchAsRetryable<TException>(
            this IUncertainTryPolicy policy,
            Func<Exception, string> messageProvider)
            where TException : Exception
            => new UncertainCatchRetryablePolicy<TException>(policy, messageProvider);
        
        public static IUncertainTryPolicy CatchAsFailure<TException>(
            this IUncertainTryPolicy policy,
            Func<Exception, string> messageProvider)
            where TException : Exception
            => new UncertainCatchFailurePolicy<TException>(policy, messageProvider);
        
        public static IUncertainTryPolicy Finalize(
            this IUncertainTryPolicy policy,
            Action finalizer)
            => new UncertainFinalizePolicy(policy, finalizer);
        
        public static IUncertainTryPolicy<TResult> Try<TResult>(Func<TResult> operation)
            => new UncertainTryPolicy<TResult>(operation);

        public static IUncertainTryPolicy<TResult> CatchAsRetryable<TResult, TException>(
            this IUncertainTryPolicy<TResult> policy,
            Func<Exception, string> messageProvider)
            where TException : Exception
            => new UncertainCatchRetryablePolicy<TResult, TException>(policy, messageProvider);
        
        public static IUncertainTryPolicy<TResult> CatchAsFailure<TResult, TException>(
            this IUncertainTryPolicy<TResult> policy,
            Func<Exception, string> messageProvider)
            where TException : Exception
            => new UncertainCatchFailurePolicy<TResult, TException>(policy, messageProvider);
        
        public static IUncertainTryPolicy<TResult> Finalize<TResult>(
            this IUncertainTryPolicy<TResult> policy,
            Action finalizer)
            => new UncertainFinalizePolicy<TResult>(policy, finalizer);
    }
}