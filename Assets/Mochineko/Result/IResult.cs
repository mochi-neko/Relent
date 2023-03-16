#nullable enable
namespace Mochineko.Result
{
    /// <summary>
    /// Defines a result.
    /// The type argument may seem to do nothing,
    ///  but prevents cast to wrong types.
    /// </summary>
    /// <typeparam name="TResult">Type of result value</typeparam>
    public interface IResult<TResult>
    {
        bool Success { get; }
        bool Failure { get; }
    }
}