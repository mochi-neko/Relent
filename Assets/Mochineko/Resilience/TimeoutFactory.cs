#nullable enable
using System;

namespace Mochineko.Resilience
{
    public static class TimeoutFactory
    {
        public static IPolicy<TResult> Timeout<TResult>(TimeSpan timeout)
            => new TimeoutPolicy<TResult>(timeout);
    }
}