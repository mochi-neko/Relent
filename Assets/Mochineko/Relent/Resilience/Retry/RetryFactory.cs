#nullable enable
using System;

namespace Mochineko.Relent.Resilience.Retry
{
    /// <summary>
    /// A factory of retry policies.
    /// </summary>
    public static class RetryFactory
    {
        /// <summary>
        /// Creates a retry policy that retries the operation up to the specified number of times.
        /// </summary>
        /// <param name="maxRetryCount"></param>
        /// <returns></returns>
        public static IRetryPolicy Retry(
            int maxRetryCount)
            => new RetryPolicy(maxRetryCount);
        
        /// <summary>
        /// Creates a retry policy that retries the operation up to the specified number of times.
        /// </summary>
        /// <param name="maxRetryCount"></param>
        /// <typeparam name="TResult">Type of result value</typeparam>
        /// <returns></returns>
        public static IRetryPolicy<TResult> Retry<TResult>(
            int maxRetryCount)
            => new RetryPolicy<TResult>(maxRetryCount);

        /// <summary>
        /// Creates a retry policy that retries the operation up to the specified number of times with the specified interval.
        /// </summary>
        /// <param name="maxRetryCount"></param>
        /// <param name="interval"></param>
        /// <returns></returns>
        public static IRetryPolicy RetryWithInterval(
            int maxRetryCount, TimeSpan interval)
            => new RetryPolicy(maxRetryCount, interval);
        
        /// <summary>
        /// Creates a retry policy that retries the operation up to the specified number of times with the specified interval.
        /// </summary>
        /// <param name="maxRetryCount"></param>
        /// <param name="interval"></param>
        /// <typeparam name="TResult">Type of result value</typeparam>
        /// <returns></returns>
        public static IRetryPolicy<TResult> RetryWithInterval<TResult>(
            int maxRetryCount, TimeSpan interval)
            => new RetryPolicy<TResult>(maxRetryCount, interval);

        /// <summary>
        /// Creates a retry policy that retries the operation up to the specified exponential backoff.
        /// </summary>
        /// <param name="maxRetryCount"></param>
        /// <param name="factor"></param>
        /// <param name="baseNumber"></param>
        /// <returns></returns>
        public static IRetryPolicy RetryWithExponentialBackoff(
            int maxRetryCount,
            double factor = 1d,
            double baseNumber = 2d)
            => new RetryPolicy(
                maxRetryCount,
                intervalProvider: retryAttempt => TimeSpan.FromSeconds(
                    factor * Math.Pow(baseNumber, retryAttempt)));
        
        /// <summary>
        /// Creates a retry policy that retries the operation up to the specified exponential backoff.
        /// </summary>
        /// <param name="maxRetryCount"></param>
        /// <param name="factor"></param>
        /// <param name="baseNumber"></param>
        /// <typeparam name="TResult">Type of result value</typeparam>
        /// <returns></returns>
        public static IRetryPolicy<TResult> RetryWithExponentialBackoff<TResult>(
            int maxRetryCount,
            double factor = 1d,
            double baseNumber = 2d)
            => new RetryPolicy<TResult>(
                maxRetryCount,
                intervalProvider: retryAttempt => TimeSpan.FromSeconds(
                    factor * Math.Pow(baseNumber, retryAttempt)));

        /// <summary>
        /// Creates a retry policy that retries the operation up to random jitter in the specified range.
        /// </summary>
        /// <param name="maxRetryCount"></param>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        /// <returns></returns>
        public static IRetryPolicy RetryWithJitter(
            int maxRetryCount,
            double minimum, double maximum)
            => new RetryPolicy(
                maxRetryCount,
                intervalProvider: retryAttempt => TimeSpan.FromSeconds(
                    GenerateRandomNumber(retryAttempt, minimum, maximum)));
        
        /// <summary>
        /// Creates a retry policy that retries the operation up to random jitter in the specified range.
        /// </summary>
        /// <param name="maxRetryCount"></param>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        /// <typeparam name="TResult">Type of result value</typeparam>
        /// <returns></returns>
        public static IRetryPolicy<TResult> RetryWithJitter<TResult>(
            int maxRetryCount,
            double minimum, double maximum)
            => new RetryPolicy<TResult>(
                maxRetryCount,
                intervalProvider: retryAttempt => TimeSpan.FromSeconds(
                    GenerateRandomNumber(retryAttempt, minimum, maximum)));

        /// <summary>
        /// Creates a retry policy that retries the operation up to the specified custom interval.
        /// </summary>
        /// <param name="maxRetryCount"></param>
        /// <param name="intervalProvider"></param>
        /// <returns></returns>
        public static IRetryPolicy RetryWithCustomInterval(
            int maxRetryCount,
            Func<int, TimeSpan> intervalProvider) 
            => new RetryPolicy(maxRetryCount, intervalProvider);
        
        /// <summary>
        /// Creates a retry policy that retries the operation up to the specified custom interval.
        /// </summary>
        /// <param name="maxRetryCount"></param>
        /// <param name="intervalProvider"></param>
        /// <typeparam name="TResult">Type of result value</typeparam>
        /// <returns></returns>
        public static IRetryPolicy<TResult> RetryWithCustomInterval<TResult>(
            int maxRetryCount,
            Func<int, TimeSpan> intervalProvider) 
            => new RetryPolicy<TResult>(maxRetryCount, intervalProvider);

        private static double GenerateRandomNumber(int salt, double minimum, double maximum)
        {
            var random = new Random(Environment.TickCount | salt);
            return random.NextDouble() * (maximum - minimum) + minimum;
        }
    }
}