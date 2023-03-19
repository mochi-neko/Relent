#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using Mochineko.UncertainResult;

namespace Mochineko.Resilience
{
    public static class WaitUtility
    {
        public static async Task<IUncertainResult> WaitAsync(
            TimeSpan waitTime,
            CancellationToken cancellationToken)
        {
            try
            {
                await Task.Delay(waitTime, cancellationToken);

                return UncertainResultFactory.Succeed();
            }
            catch (OperationCanceledException exception)
            {
                return UncertainResultFactory.Retry(
                    $"Cancelled to wait delay because operation was cancelled with exception:{exception}.");
            }
            catch (Exception exception)
            {
                return UncertainResultFactory.Fail(
                    $"Failed to wait delay because of an unhandled exception:{exception}.");
            }
        }

        public static async Task<IUncertainResult> WaitAsync(
            SemaphoreSlim semaphoreSlim,
            CancellationToken cancellationToken)
        {
            try
            {
                await semaphoreSlim.WaitAsync(cancellationToken);

                return UncertainResultFactory.Succeed();
            }
            catch (OperationCanceledException exception)
            {
                return UncertainResultFactory.Retry(
                    $"Cancelled to wait semaphore because operation was cancelled with exception:{exception}.");
            }
            catch (Exception exception)
            {
                return UncertainResultFactory.Fail(
                    $"Failed to wait semaphore because of unhandled exception:{exception}.");
            }
        }
    }
}