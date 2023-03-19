#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using Mochineko.Relent.UncertainResult;

namespace Mochineko.Relent.Resilience
{
    /// <summary>
    /// Utilities for waiting.
    /// </summary>
    public static class WaitUtility
    {
        /// <summary>
        /// Waits for the specified time as <see cref="IUncertainResult"/>.
        /// </summary>
        /// <param name="waitTime"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Waits for the specified <see cref="SemaphoreSlim"/> as <see cref="IUncertainResult"/>.
        /// </summary>
        /// <param name="semaphoreSlim"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
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