#nullable enable
namespace Mochineko.Resilience.Timeout
{
    /// <summary>
    /// Defines a timeout policy that can be applied to an operation with no result value.
    /// </summary>
    public interface ITimeoutPolicy
        : IPolicy
    {
    }

    /// <summary>
    /// Defines a timeout policy that can be applied to an operation with result value.
    /// </summary>
    /// <typeparam name="TResult">Type of result value</typeparam>
    public interface ITimeoutPolicy<TResult>
        : IPolicy<TResult>
    {
    }
}