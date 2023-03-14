#nullable enable
using System;

namespace Mochineko.HttpResult
{
    public interface IHttpFailureResult<out TReason>
        : IHttpResult
        where TReason : Exception
    {
        TReason Reason { get; }
    }

    public interface IHttpFailureResult<TResult, out TReason>
        : IHttpResult<TResult>
        where TReason : Exception
    {
        TReason Reason { get; }
    }
}