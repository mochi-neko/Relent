#nullable enable
namespace Mochineko.Resilience
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