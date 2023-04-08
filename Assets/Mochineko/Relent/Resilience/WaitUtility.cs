#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
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
        public static async UniTask<IUncertainResult> WaitAsync(
            TimeSpan waitTime,
            CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return UncertainResults.RetryWithTrace(
                    $"Operation has been already cancelled.");
            }

            try
            {
                await UniTask.Delay(waitTime, cancellationToken: cancellationToken);

                return UncertainResults.Succeed();
            }
            catch (OperationCanceledException exception)
            {
                return UncertainResults.RetryWithTrace(
                    $"Cancelled to wait delay because operation was cancelled because of {exception}.");
            }
        }

        /// <summary>
        /// Waits for the specified <see cref="SemaphoreSlim"/> as <see cref="IUncertainResult"/>.
        /// </summary>
        /// <param name="semaphoreSlim"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async UniTask<IUncertainResult> WaitAsync(
            SemaphoreSlim semaphoreSlim,
            CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return UncertainResults.RetryWithTrace(
                    $"Operation has been already cancelled.");
            }

            try
            {
                await semaphoreSlim.WaitAsync(cancellationToken);

                return UncertainResults.Succeed();
            }
            catch (OperationCanceledException exception)
            {
                return UncertainResults.RetryWithTrace(
                    $"Cancelled to wait semaphore because operation was cancelled because of:{exception}.");
            }
        }
    }
}