#nullable enable
namespace Mochineko.HttpResult
{
    public interface IHttpSuccessResult
        : IHttpResult
    {
    }

    public interface IHttpSuccessResult<TResult>
        : IHttpResult<TResult>
    {
        TResult Result { get; }
    }
}