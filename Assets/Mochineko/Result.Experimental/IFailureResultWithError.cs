#nullable enable
using System;

namespace Mochineko.Result.Experimental
{
    public interface IFailureResultWithError<TResult, out TError>
        : IFailureResult<TResult>
        where TError : Exception
    {
        TError Error { get; }
    }
}