#nullable enable
using System;

namespace Mochineko.HttpResult
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