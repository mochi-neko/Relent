#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using Mochineko.Result;

namespace Mochineko.Resilience
{
    public static class WaitUtility
    {
        public static async Task<IResult> WaitAsync(
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
                    $"Cancelled to wait delay because operation was cancelled with exception:{exception}.");
            }
            catch (Exception exception)
            {
                return ResultFactory.Fail(
                    $"Cancelled to wait delay because of unhandled exception:{exception}.");
            }
        }

        public static async Task<IResult> WaitAsync(
            SemaphoreSlim semaphoreSlim,
            CancellationToken cancellationToken)
        {
            try
            {
                await semaphoreSlim.WaitAsync(cancellationToken);

                return ResultFactory.Succeed();
            }
            catch (OperationCanceledException exception)
            {
                return ResultFactory.Fail(
                    $"Cancelled to wait semaphore because operation was cancelled with exception:{exception}.");
            }
            catch (Exception exception)
            {
                return ResultFactory.Fail(
                    $"Cancelled to wait semaphore because of unhandled exception:{exception}.");
            }
        }
    }
}