#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using Mochineko.UncertainResult;

namespace Mochineko.Resilience.Tests
{
    internal static class WaitUtility
    {
        public static async Task<IUncertainResult<TResult>> WaitAndSucceed<TResult>(
            TimeSpan waitTime,
            CancellationToken cancellationToken,
            TResult successResult)
        {
            try
            {
                await Task.Delay(waitTime, cancellationToken);

                return UncertainResultFactory.Succeed(successResult);
            }
            catch (OperationCanceledException exception)
            {
                return UncertainResultFactory.Retry<TResult>(
                    $"Cancelled to wait because operation was cancelled with exception:{exception}.");
            }
            catch (Exception exception)
            {
                return UncertainResultFactory.Fail<TResult>(
                    $"Cancelled to wait because of unhandled exception:{exception}.");
            }
        }
        
        public static async Task<IUncertainResult<TResult>> WaitAndRetry<TResult>(
            TimeSpan waitTime,
            CancellationToken cancellationToken)
        {
            try
            {
                await Task.Delay(waitTime, cancellationToken);

                return UncertainResultFactory.Retry<TResult>(
                    "Retryable after wait.");
            }
            catch (OperationCanceledException exception)
            {
                return UncertainResultFactory.Retry<TResult>(
                    $"Cancelled to wait because operation was cancelled with exception:{exception}.");
            }
            catch (Exception exception)
            {
                return UncertainResultFactory.Fail<TResult>(
                    $"Cancelled to wait because of unhandled exception:{exception}.");
            }
        }
    }
}