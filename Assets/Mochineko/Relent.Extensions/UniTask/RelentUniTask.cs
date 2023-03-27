#nullable enable
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Mochineko.Relent.Result;

namespace Mochineko.Relent.Extensions.UniTask
{
    public static class RelentUniTask
    {
        /// <summary>
        /// Delays execution for the specified time.
        /// </summary>
        /// <param name="delayTimeSpan"></param>
        /// <param name="ignoreTimeScale"></param>
        /// <param name="delayTiming"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async UniTask<IResult> Delay(
            TimeSpan delayTimeSpan,
            bool ignoreTimeScale = false,
            PlayerLoopTiming delayTiming = PlayerLoopTiming.Update,
            CancellationToken cancellationToken = default)
        {
            try
            {
                await Cysharp.Threading.Tasks.UniTask.Delay(
                    delayTimeSpan,
                    ignoreTimeScale,
                    delayTiming,
                    cancellationToken);

                return ResultFactory.Succeed();
            }
            catch (OperationCanceledException exception)
            {
                return ResultFactory.Fail(
                    $"Failed to delay because operation was cancelled by -> {exception}.");
            }
        }

        /// <summary>
        /// Switches execution to the main thread.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async UniTask<IResult> SwitchToMainThread(
            CancellationToken cancellationToken)
        {
            try
            {
                await Cysharp.Threading.Tasks.UniTask.SwitchToMainThread(cancellationToken);

                return ResultFactory.Succeed();
            }
            catch (OperationCanceledException exception)
            {
                return ResultFactory.Fail(
                    $"Failed to switch to main thread because operation was cancelled by -> {exception}.");
            }
        }
    }
}
