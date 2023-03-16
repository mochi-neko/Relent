#nullable enable
using System;

namespace Mochineko.Result
{
    public sealed class FailedToUnwrapResultException
        : Exception
    {
        internal FailedToUnwrapResultException(string message)
            : base(message)
        {
        }
    }
}