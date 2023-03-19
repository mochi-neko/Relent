#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using Mochineko.Result;
using Mochineko.UncertainResult;

namespace Mochineko.Resilience.Wrap
{
    internal sealed class PolicyWrap<TResult>
        : IPolicy<TResult>
    {
        private readonly IPolicy<TResult> innerPolicy;
        private readonly IPolicy<TResult> outerPolicy;

        public PolicyWrap(
            IPolicy<TResult> innerPolicy,
            IPolicy<TResult> outerPolicy)
        {
            this.innerPolicy = innerPolicy;
            this.outerPolicy = outerPolicy;
        }

        public async Task<IResult<TResult>> ExecuteAsync(
            Func<CancellationToken, Task<IUncertainResult<TResult>>> execute,
            CancellationToken cancellationToken)
            => await outerPolicy.ExecuteAsync(
                execute: async innerCancellationToken =>
                {
                    var result = await innerPolicy.ExecuteAsync(execute, innerCancellationToken);
                    if (result is ISuccessResult<TResult> success)
                    {
                        return UncertainResultFactory.Succeed(success.Result);
                    }
                    else if (result is IFailureResult<TResult> failure)
                    {
                        return UncertainResultFactory.Retry<TResult>(failure.Message);
                    }
                    else
                    {
                        throw new ResultPatternMatchException(nameof(result));
                    }
                },
                cancellationToken);
    }
}