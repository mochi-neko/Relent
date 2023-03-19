#nullable enable
namespace Mochineko.Resilience.Retry
{
    public interface IRetryPolicy
        : IPolicy
    {
        int RetryCount { get; }
    }
    
    public interface IRetryPolicy<TResult>
        : IPolicy<TResult>
    {
        int RetryCount { get; }
    }
}