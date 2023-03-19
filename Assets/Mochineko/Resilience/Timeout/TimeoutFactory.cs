#nullable enable
using System;

namespace Mochineko.Resilience.Timeout
{
    public static class TimeoutFactory
    {
        public static ITimeoutPolicy<TResult> Timeout<TResult>(TimeSpan timeout)
            => new TimeoutPolicy<TResult>(timeout);
    }
}