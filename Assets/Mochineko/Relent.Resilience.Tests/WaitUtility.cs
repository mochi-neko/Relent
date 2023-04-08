#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using Mochineko.Relent.UncertainResult;

namespace Mochineko.Relent.Resilience.Tests
{
    internal static class WaitUtility
    {
        public static async Task<IUncertainResult> WaitAndSucceed(
            TimeSpan waitTime,
            CancellationToken cancellationToken)
        {
            try
            {
                await Task.Delay(waitTime, cancellationToken);

                return UncertainResults.Succeed();
            }
            catch (OperationCanceledException exception)
            {
                return UncertainResults.Retry(
                    $"Cancelled to wait because operation was cancelled with exception:{exception}.");
            }
        }
        
        public static async Task<IUncertainResult<TResult>> WaitAndSucceed<TResult>(
            TimeSpan waitTime,
            CancellationToken cancellationToken,
            TResult successResult)
        {
            try
            {
                await Task.Delay(waitTime, cancellationToken);

                return UncertainResults.Succeed(successResult);
            }
            catch (OperationCanceledException exception)
            {
                return UncertainResults.Retry<TResult>(
                    $"Cancelled to wait because operation was cancelled with exception:{exception}.");
            }
        }
        
        public static async Task<IUncertainResult> WaitAndRetry(
            TimeSpan waitTime,
            CancellationToken cancellationToken)
        {
            try
            {
                await Task.Delay(waitTime, cancellationToken);

                return UncertainResults.Retry(
                    "Retryable after wait.");
            }
            catch (OperationCanceledException exception)
            {
                return UncertainResults.Retry(
                    $"Cancelled to wait because operation was cancelled with exception:{exception}.");
            }
        }
        
        public static async Task<IUncertainResult<TResult>> WaitAndRetry<TResult>(
            TimeSpan waitTime,
            CancellationToken cancellationToken)
        {
            try
            {
                await Task.Delay(waitTime, cancellationToken);

                return UncertainResults.Retry<TResult>(
                    "Retryable after wait.");
            }
            catch (OperationCanceledException exception)
            {
                return UncertainResults.Retry<TResult>(
                    $"Cancelled to wait because operation was cancelled with exception:{exception}.");
            }
        }
    }
}