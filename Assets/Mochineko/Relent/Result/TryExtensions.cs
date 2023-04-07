#nullable enable
using System;

namespace Mochineko.Relent.Result
{
    public static class TryExtensions
    {
        public static ITryPolicy Try(Action operation)
            => new TryPolicy(operation);

        public static ITryPolicy Catch<TException>(
            this ITryPolicy policy,
            Func<Exception, string> messageProvider)
            where TException : Exception
            => new CatchPolicy<TException>(policy, messageProvider);
        
        public static ITryPolicy Finalize(
            this ITryPolicy policy,
            Action finalizer)
            => new FinalizePolicy(policy, finalizer);
        
        public static ITryPolicy<TResult> Try<TResult>(Func<TResult> operation)
            => new TryPolicy<TResult>(operation);

        public static ITryPolicy<TResult> Catch<TResult, TException>(
            this ITryPolicy<TResult> policy,
            Func<Exception, string> messageProvider)
            where TException : Exception
            => new CatchPolicy<TResult, TException>(policy, messageProvider);
        
        public static ITryPolicy<TResult> Finalize<TResult>(
            this ITryPolicy<TResult> policy,
            Action finalizer)
            => new FinalizePolicy<TResult>(policy, finalizer);
    }
}