#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using FluentAssertions;
using Mochineko.Relent.Resilience.Bulkhead;
using Mochineko.Relent.UncertainResult;
using NUnit.Framework;
using UnityEngine.TestTools;
#pragma warning disable CS0618
#pragma warning disable CS4014

namespace Mochineko.Relent.Resilience.Tests
{
    [TestFixture]
    internal sealed class BulkheadTest
    {
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(5)]
        [TestCase(10)]
        [RequiresPlayMode(false)]
        public async Task PrimitiveBulkheadWithNoValueTest(int maxParallelization)
        {
            IBulkheadPolicy policy = BulkheadFactory.Bulkhead(maxParallelization);

            var taskList = new List<UniTask<IUncertainResult>>();
            for (var i = 0; i < maxParallelization + 1; i++)
            {
                var task = policy.ExecuteAsync(
                    execute: cancellationToken => WaitUtility.WaitAndSucceed(
                        TimeSpan.FromSeconds(0.1d),
                        cancellationToken),
                    cancellationToken: CancellationToken.None);

                taskList.Add(task);
            }

            policy.RemainingParallelizationCount.Should().Be(0,
                because: "Bulkhead is fulfilled.");

            await UniTask.Delay(TimeSpan.FromSeconds(0.11d));

            policy.RemainingParallelizationCount.Should().Be(maxParallelization - 1);

            for (var i = 0; i < maxParallelization; i++)
            {
                taskList[i].Status.Should().Be(UniTaskStatus.Succeeded);
            }

            taskList[maxParallelization].Status.Should().Be(UniTaskStatus.Pending);

            await UniTask.Delay(TimeSpan.FromSeconds(1d));

            taskList[maxParallelization].Status.Should().Be(UniTaskStatus.Succeeded);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(5)]
        [TestCase(10)]
        [RequiresPlayMode(false)]
        public async Task PrimitiveBulkheadTest(int maxParallelization)
        {
            IBulkheadPolicy<int> policy = BulkheadFactory.Bulkhead<int>(maxParallelization);

            var taskList = new List<UniTask<IUncertainResult<int>>>();
            for (var i = 0; i < maxParallelization + 1; i++)
            {
                var task = policy.ExecuteAsync(
                    execute: cancellationToken => WaitUtility.WaitAndSucceed(
                        TimeSpan.FromSeconds(0.1d),
                        cancellationToken,
                        1),
                    cancellationToken: CancellationToken.None);

                taskList.Add(task);
            }

            policy.RemainingParallelizationCount.Should().Be(0,
                because: "Bulkhead is fulfilled.");

            await UniTask.Delay(TimeSpan.FromSeconds(0.11d));

            policy.RemainingParallelizationCount.Should().Be(maxParallelization - 1);

            for (var i = 0; i < maxParallelization; i++)
            {
                taskList[i].Status.Should().Be(UniTaskStatus.Succeeded);
            }

            taskList[maxParallelization].Status.Should().Be(UniTaskStatus.Pending);

            await UniTask.Delay(TimeSpan.FromSeconds(1d));

            taskList[maxParallelization].Status.Should().Be(UniTaskStatus.Succeeded);
        }
    }
}