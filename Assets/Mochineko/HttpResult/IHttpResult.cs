#nullable enable
using System.Net;

namespace Mochineko.HttpResult
{
    public interface IHttpResult
    {
        HttpStatusCode StatusCode { get; }
    }
    
    public interface IHttpResult<TResult>
    {
        HttpStatusCode StatusCode { get; }
    }
}