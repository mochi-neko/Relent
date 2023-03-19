#nullable enable
using System;

namespace Mochineko.Relent.Result
{
    internal sealed class SuccessResult
        : ISuccessResult
    {
        public bool Success => true;
        public bool Failure => false;
    }
    
    internal sealed class SuccessResult<TResult>
        : ISuccessResult<TResult>
    {
        public bool Success => true;
        public bool Failure => false;
        public TResult Result { get; }
        
        public SuccessResult(TResult result)
        {
            Result = result ?? throw new ArgumentNullException(nameof(result));
        }
    }
}