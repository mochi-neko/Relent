#nullable enable
using System;

namespace Mochineko.Relent.UncertainResult
{
    /// <summary>
    /// A factory of <see cref="IUncertainTryPolicy"/>.
    /// </summary>
    public static class UncertainTryFactory
    {
        /// <summary>
        /// Tries an operation.
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        public static IUncertainTryPolicy Try(Action operation)
            => new UncertainTryPolicy(operation);

        /// <summary>
        /// Catches an exception and convert it to <see cref="IUncertainRetryableResult"/>.
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="messageProvider"></param>
        /// <typeparam name="TException"></typeparam>
        /// <returns></returns>
        public static IUncertainTryPolicy CatchAsRetryable<TException>(
            this IUncertainTryPolicy policy,
            Func<Exception, string> messageProvider)
            where TException : Exception
            => new UncertainCatchRetryablePolicy<TException>(policy, messageProvider);
        
        /// <summary>
        /// Catches an exception and convert it to <see cref="IUncertainFailureResult"/>.
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="messageProvider"></param>
        /// <typeparam name="TException"></typeparam>
        /// <returns></returns>
        public static IUncertainTryPolicy CatchAsFailure<TException>(
            this IUncertainTryPolicy policy,
            Func<Exception, string> messageProvider)
            where TException : Exception
            => new UncertainCatchFailurePolicy<TException>(policy, messageProvider);
        
        /// <summary>
        /// Finalizes an operation.
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="finalizer"></param>
        /// <returns></returns>
        public static IUncertainTryPolicy Finalize(
            this IUncertainTryPolicy policy,
            Action finalizer)
            => new UncertainFinalizePolicy(policy, finalizer);
        
        /// <summary>
        /// Tries an operation with value.
        /// </summary>
        /// <param name="operation"></param>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public static IUncertainTryPolicy<TResult> Try<TResult>(Func<TResult> operation)
            => new UncertainTryPolicy<TResult>(operation);

        /// <summary>
        /// Catches an exception and convert it to <see cref="IUncertainRetryableResult{TResult}"/>.
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="messageProvider"></param>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TException"></typeparam>
        /// <returns></returns>
        public static IUncertainTryPolicy<TResult> CatchAsRetryable<TResult, TException>(
            this IUncertainTryPolicy<TResult> policy,
            Func<Exception, string> messageProvider)
            where TException : Exception
            => new UncertainCatchRetryablePolicy<TResult, TException>(policy, messageProvider);
        
        /// <summary>
        /// Catches an exception and convert it to <see cref="IUncertainFailureResult{TResult}"/>.
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="messageProvider"></param>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TException"></typeparam>
        /// <returns></returns>
        public static IUncertainTryPolicy<TResult> CatchAsFailure<TResult, TException>(
            this IUncertainTryPolicy<TResult> policy,
            Func<Exception, string> messageProvider)
            where TException : Exception
            => new UncertainCatchFailurePolicy<TResult, TException>(policy, messageProvider);
        
        /// <summary>
        /// Finalizes an operation.
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="finalizer"></param>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public static IUncertainTryPolicy<TResult> Finalize<TResult>(
            this IUncertainTryPolicy<TResult> policy,
            Action finalizer)
            => new UncertainFinalizePolicy<TResult>(policy, finalizer);
    }
}