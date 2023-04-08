#nullable enable
namespace Mochineko.Relent.Result
{
    /// <summary>
    /// Defines a policy to try an operation.
    /// </summary>
    public interface ITryPolicy
    {
        /// <summary>
        /// Executes the operation.
        /// </summary>
        /// <returns></returns>
        IResult Execute();
    }
    
    /// <summary>
    /// Defines a policy to try an operation with value.
    /// </summary>
    /// <typeparam name="TResult">Type of result value</typeparam>
    public interface ITryPolicy<TResult>
    {
        /// <summary>
        /// Executes the operation.
        /// </summary>
        /// <returns></returns>
        IResult<TResult> Execute();
    }
}