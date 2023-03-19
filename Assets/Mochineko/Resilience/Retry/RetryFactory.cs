#nullable enable
using System;

namespace Mochineko.Resilience.Retry
{
    public static class RetryFactory
    {
        public static IRetryPolicy<TResult> Retry<TResult>(
            int retryCount)
            => new RetryPolicy<TResult>(retryCount);

        public static IRetryPolicy<TResult> RetryWithWait<TResult>(
            int retryCount, TimeSpan waitDuration)
            => new RetryPolicy<TResult>(retryCount, waitDuration);

        public static IRetryPolicy<TResult> RetryWithExponentialBackoff<TResult>(
            int retryCount,
            double baseNumber = 2d)
            => new RetryPolicy<TResult>(
                retryCount,
                durationProvider: retryAttempt => TimeSpan.FromSeconds(
                    Math.Pow(baseNumber, retryAttempt)));

        public static IRetryPolicy<TResult> RetryWithJitter<TResult>(
            int retryCount,
            double minimum, double maximum)
            => new RetryPolicy<TResult>(
                retryCount,
                durationProvider: retryAttempt => TimeSpan.FromSeconds(
                    GenerateRandomNumber(retryAttempt, minimum, maximum)));

        public static IRetryPolicy<TResult> RetryWithCustomDuration<TResult>(
            int retryCount,
            Func<int, TimeSpan> durationProvider) 
            => new RetryPolicy<TResult>(retryCount, durationProvider);

        private static double GenerateRandomNumber(int salt, double minimum, double maximum)
        {
            var random = new Random(Environment.TickCount | salt);
            return random.NextDouble() * (maximum - minimum) + minimum;
        }
    }
}