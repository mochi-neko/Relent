#nullable enable
namespace Mochineko.Relent.UncertainResult
{
    public interface IUncertainTryPolicy
    {
        IUncertainResult Execute();
    }

    public interface IUncertainTryPolicy<TResult>
    {
        IUncertainResult<TResult> Execute();
    }
}