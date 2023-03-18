#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using Mochineko.Result;
using Mochineko.UncertainResult;

namespace Mochineko.Resilience
{
    public static class CertainWait
    {
        public static async Task<IResult> Wait(
            TimeSpan waitTime,
            CancellationToken cancellationToken)
        {
            try
            {
                await Task.Delay(waitTime, cancellationToken);

                return ResultFactory.Succeed();
            }
            catch (OperationCanceledException exception)
            {
                return ResultFactory.Fail(
                    $"Cancelled to wait because operation was cancelled with exception:{exception}.");
            }
            catch (Exception exception)
            {
                return ResultFactory.Fail(
                    $"Cancelled to wait because of unhandled exception:{exception}.");
            }
        }
    }
}