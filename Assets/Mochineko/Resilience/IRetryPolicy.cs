#nullable enable
namespace Mochineko.Resilience
{
    public interface IRetryPolicy<TResult>
        : IPolicy<TResult>
    {
        int RetryCount { get; }
    }
}