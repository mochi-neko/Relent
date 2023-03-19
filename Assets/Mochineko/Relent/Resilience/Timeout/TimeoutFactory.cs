#nullable enable
using System;

namespace Mochineko.Relent.Resilience.Timeout
{
    /// <summary>
    /// A factory of timeout policies.
    /// </summary>
    public static class TimeoutFactory
    {
        /// <summary>
        /// Creates a timeout policy that cancels the operation when the specified timeout is exceeded.
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static ITimeoutPolicy Timeout(TimeSpan timeout)
            => new TimeoutPolicy(timeout);
        
        /// <summary>
        /// Creates a timeout policy that cancels the operation when the specified timeout is exceeded.
        /// </summary>
        /// <param name="timeout"></param>
        /// <typeparam name="TResult">Type of result value</typeparam>
        /// <returns></returns>
        public static ITimeoutPolicy<TResult> Timeout<TResult>(TimeSpan timeout)
            => new TimeoutPolicy<TResult>(timeout);
    }
}