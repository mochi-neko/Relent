#nullable enable
using System;
using System.Threading;
using Cysharp.Threading.Tasks;

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

        /// <summary>
        /// Tries an asynchronous operation.
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        public static IUncertainAsyncTryPolicy TryAsync(Func<CancellationToken, UniTask> operation)
            => new UncertainAsyncTryPolicy(operation);

        /// <summary>
        /// Catches an exception and convert it to <see cref="IUncertainRetryableResult"/>.
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="messageProvider"></param>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TException"></typeparam>
        /// <returns></returns>
        public static IUncertainAsyncTryPolicy CatchAsRetryable<TException>(
            this IUncertainAsyncTryPolicy policy,
            Func<Exception, string> messageProvider)
            where TException : Exception
            => new UncertainAsyncCatchRetryablePolicy<TException>(policy, messageProvider);

        /// <summary>
        /// Catches an exception and convert it to <see cref="IUncertainFailureResult"/>.
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="messageProvider"></param>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TException"></typeparam>
        /// <returns></returns>
        public static IUncertainAsyncTryPolicy CatchAsFailure<TException>(
            this IUncertainAsyncTryPolicy policy,
            Func<Exception, string> messageProvider)
            where TException : Exception
            => new UncertainAsyncCatchFailurePolicy<TException>(policy, messageProvider);

        /// <summary>
        /// Finalizes an asynchronous operation.
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="finalizer"></param>
        /// <returns></returns>
        public static IUncertainAsyncTryPolicy Finalize(
            this IUncertainAsyncTryPolicy policy,
            Func<UniTask> finalizer)
            => new UncertainAsyncFinalizePolicy(policy, finalizer);

        /// <summary>
        /// Tries an asynchronous operation with value.
        /// </summary>
        /// <param name="operation"></param>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public static IUncertainAsyncTryPolicy<TResult> TryAsync<TResult>(
            Func<CancellationToken, UniTask<TResult>> operation)
            => new UncertainAsyncTryPolicy<TResult>(operation);

        /// <summary>
        /// Catches an exception and convert it to <see cref="IUncertainRetryableResult{TResult}"/>.
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="messageProvider"></param>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TException"></typeparam>
        /// <returns></returns>
        public static IUncertainAsyncTryPolicy<TResult> CatchAsRetryable<TResult, TException>(
            this IUncertainAsyncTryPolicy<TResult> policy,
            Func<Exception, string> messageProvider)
            where TException : Exception
            => new UncertainAsyncCatchRetryablePolicy<TResult, TException>(policy, messageProvider);

        /// <summary>
        /// Catches an exception and convert it to <see cref="IUncertainFailureResult{TResult}"/>.
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="messageProvider"></param>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TException"></typeparam>
        /// <returns></returns>
        public static IUncertainAsyncTryPolicy<TResult> CatchAsFailure<TResult, TException>(
            this IUncertainAsyncTryPolicy<TResult> policy,
            Func<Exception, string> messageProvider)
            where TException : Exception
            => new UncertainAsyncCatchFailurePolicy<TResult, TException>(policy, messageProvider);

        /// <summary>
        /// Finalizes an asynchronous operation with value.
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="finalizer"></param>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public static IUncertainAsyncTryPolicy<TResult> Finalize<TResult>(
            this IUncertainAsyncTryPolicy<TResult> policy,
            Func<UniTask> finalizer)
            => new UncertainAsyncFinalizePolicy<TResult>(policy, finalizer);
    }
}