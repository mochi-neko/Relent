#nullable enable
namespace Mochineko.Resilience.Timeout
{
    public interface ITimeoutPolicy
        : IPolicy
    {
    }

    public interface ITimeoutPolicy<TResult>
        : IPolicy<TResult>
    {
    }
}