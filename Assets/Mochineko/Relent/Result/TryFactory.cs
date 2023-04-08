#nullable enable
using System;

namespace Mochineko.Relent.Result
{
    /// <summary>
    /// A factory of <see cref="ITryPolicy"/> instances.
    /// </summary>
    public static class TryFactory
    {
        /// <summary>
        /// Tries an operation.
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        public static ITryPolicy Try(Action operation)
            => new TryPolicy(operation);

        /// <summary>
        /// Catch an exception and convert it to <see cref="IFailureResult"/>.
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="messageProvider"></param>
        /// <typeparam name="TException"></typeparam>
        /// <returns></returns>
        public static ITryPolicy Catch<TException>(
            this ITryPolicy policy,
            Func<Exception, string> messageProvider)
            where TException : Exception
            => new CatchPolicy<TException>(policy, messageProvider);
        
        /// <summary>
        /// Finalizes an operation.
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="finalizer"></param>
        /// <returns></returns>
        public static ITryPolicy Finalize(
            this ITryPolicy policy,
            Action finalizer)
            => new FinalizePolicy(policy, finalizer);
        
        /// <summary>
        /// Tries an operation with value.
        /// </summary>
        /// <param name="operation"></param>
        /// <typeparam name="TResult">Type of result value</typeparam>
        /// <returns></returns>
        public static ITryPolicy<TResult> Try<TResult>(Func<TResult> operation)
            => new TryPolicy<TResult>(operation);

        /// <summary>
        /// Catch an exception and convert it to <see cref="IFailureResult{TResult}"/>.
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="messageProvider"></param>
        /// <typeparam name="TResult">Type of result value</typeparam>
        /// <typeparam name="TException"></typeparam>
        /// <returns></returns>
        public static ITryPolicy<TResult> Catch<TResult, TException>(
            this ITryPolicy<TResult> policy,
            Func<Exception, string> messageProvider)
            where TException : Exception
            => new CatchPolicy<TResult, TException>(policy, messageProvider);
        
        /// <summary>
        /// Finalizes an operation with value.
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="finalizer"></param>
        /// <typeparam name="TResult">Type of result value</typeparam>
        /// <returns></returns>
        public static ITryPolicy<TResult> Finalize<TResult>(
            this ITryPolicy<TResult> policy,
            Action finalizer)
            => new FinalizePolicy<TResult>(policy, finalizer);
    }
}