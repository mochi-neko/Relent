#nullable enable
namespace Mochineko.HttpResult
{
    /// <summary>
    /// Defines a success result of HTTP communication with value.
    /// </summary>
    /// <typeparam name="TResult">Type of result value</typeparam>
    public interface IHttpSuccessResult<TResult>
        : IHttpResult<TResult>
    {
        TResult Result { get; }
    }
}