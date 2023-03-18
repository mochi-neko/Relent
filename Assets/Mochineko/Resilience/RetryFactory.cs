#nullable enable
using System;

namespace Mochineko.Resilience
{
    public static class RetryFactory
    {
        public static IPolicy<TResult> Retry<TResult>(int retryCount)
        {
            return new RetryPolicy<TResult>(retryCount);
        }
        
        public static IPolicy<TResult> RetryWithWait<TResult>(int retryCount, TimeSpan waitDuration)
        {
            return new RetryPolicy<TResult>(retryCount, waitDuration);
        }

        public static IPolicy<TResult> RetryWithExponentialBackoff<TResult>(int retryCount, double baseNumber = 2d)
        {
            return new RetryPolicy<TResult>(
                retryCount,
                durationProvider: retryAttempt => TimeSpan.FromSeconds(
                    Math.Pow(baseNumber, retryAttempt)));
        }
        
        public static IPolicy<TResult> RetryWithJitter<TResult>(int retryCount, double minimum, double maximum)
        {
            return new RetryPolicy<TResult>(
                retryCount,
                durationProvider: retryAttempt => TimeSpan.FromSeconds(
                    GenerateRandomNumber(retryAttempt, minimum, maximum)));
        }

        public static IPolicy<TResult> RetryWithCustomDuration<TResult>(int retryCount, Func<int, TimeSpan> durationProvider)
        {
            return new RetryPolicy<TResult>(retryCount, durationProvider);
        }
        
        private static double GenerateRandomNumber(int salt, double minimum, double maximum)
        {
            var random = new Random(Environment.TickCount | salt);
            return random.NextDouble() * (maximum - minimum) + minimum;
        }
    }
}