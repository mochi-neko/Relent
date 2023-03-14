#nullable enable
using System;

namespace Mochineko.HttpResult
{
    public interface IHttpRetryableResult<out TReason>
        : IHttpResult
        where TReason : Exception
    {
        TReason Reason { get; }
    }
    
    public interface IHttpRetryableResult<TResult, out TReason>
        : IHttpResult<TResult>
        where TReason : Exception
    {
        TReason Reason { get; }
    }
}