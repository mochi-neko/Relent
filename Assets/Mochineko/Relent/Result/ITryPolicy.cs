#nullable enable
namespace Mochineko.Relent.Result
{
    public interface ITryPolicy
    {
        IResult Execute();
    }
    
    public interface ITryPolicy<TResult>
    {
        IResult<TResult> Execute();
    }
}