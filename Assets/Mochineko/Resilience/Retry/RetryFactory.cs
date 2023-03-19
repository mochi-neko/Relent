#nullable enable
using System;

namespace Mochineko.Resilience.Retry
{
    public static class RetryFactory
    {
        public static IRetryPolicy<TResult> Retry<TResult>(
            int maxRetryCount)
            => new RetryPolicy<TResult>(maxRetryCount);

        public static IRetryPolicy<TResult> RetryWithWait<TResult>(
            int maxRetryCount, TimeSpan interval)
            => new RetryPolicy<TResult>(maxRetryCount, interval);

        public static IRetryPolicy<TResult> RetryWithExponentialBackoff<TResult>(
            int maxRetryCount,
            double baseNumber = 2d)
            => new RetryPolicy<TResult>(
                maxRetryCount,
                intervalProvider: retryAttempt => TimeSpan.FromSeconds(
                    Math.Pow(baseNumber, retryAttempt)));

        public static IRetryPolicy<TResult> RetryWithJitter<TResult>(
            int maxRetryCount,
            double minimum, double maximum)
            => new RetryPolicy<TResult>(
                maxRetryCount,
                intervalProvider: retryAttempt => TimeSpan.FromSeconds(
                    GenerateRandomNumber(retryAttempt, minimum, maximum)));

        public static IRetryPolicy<TResult> RetryWithCustomDuration<TResult>(
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