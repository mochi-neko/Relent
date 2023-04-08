#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using UnityEngine.TestTools;
#pragma warning disable CS1998

namespace Mochineko.Relent.Result.Tests
{
    [TestFixture]
    internal sealed class AsyncTryExtensionsTest
    {
        [Test]
        [RequiresPlayMode(false)]
        public async Task AsyncTryShouldSuccessWithNoException()
        {
            var result = await TryFactory
                .TryAsync(async _ =>
                {
                    return await UniTask.FromResult(1);
                })
                .ExecuteAsync(CancellationToken.None);

            result.Success.Should().BeTrue();
        }

        [Test]
        [RequiresPlayMode(false)]
        public async Task AsyncTryWithValueShouldSuccessWithNoException()
        {
            var result = await TryFactory
                .TryAsync(async _ => 1)
                .ExecuteAsync(CancellationToken.None);

            result.Success.Should().BeTrue();
            result.Unwrap().Should().Be(1);
        }

        [Test]
        [RequiresPlayMode(false)]
        public async Task AsyncTryShouldCatchSpecifiedException()
        {
            var result = await TryFactory
                .TryAsync(async _ => { throw new NullReferenceException(); })
                .Catch<NullReferenceException>(_ => "Caught")
                .ExecuteAsync(CancellationToken.None);

            result.Failure.Should().BeTrue();
            result.ExtractMessage().Should().Be("Caught\n");
        }

        [Test]
        [RequiresPlayMode(false)]
        public async Task AsyncTryWithValueShouldCatchSpecifiedException()
        {
            var result = await TryFactory
                .TryAsync<int>(_ => { throw new NullReferenceException(); })
                .Catch<int, NullReferenceException>(_ => "Caught")
                .ExecuteAsync(CancellationToken.None);

            result.Failure.Should().BeTrue();
            result.ExtractMessage().Should().Be("Caught\n");
        }

        [Test]
        [RequiresPlayMode(false)]
        public async Task AsyncTryShouldNotCatchNoSpecifiedException()
        {
            Func<Task<IResult>> tryExtension = async () => await TryFactory
                .TryAsync(_ => throw new InvalidCastException())
                .Catch<NullReferenceException>(_ => "Failed")
                .ExecuteAsync(CancellationToken.None);

            await tryExtension.Should().ThrowAsync<InvalidCastException>();
        }

        [Test]
        [RequiresPlayMode(false)]
        public async Task AsyncTryWithValueShouldNotCatchNoSpecifiedException()
        {
            Func<Task<IResult<int>>> tryExtension = async () => await TryFactory
                .TryAsync<int>(_ => throw new InvalidCastException())
                .Catch<int, NullReferenceException>(_ => "Failed")
                .ExecuteAsync(CancellationToken.None);

            await tryExtension.Should().ThrowAsync<InvalidCastException>();
        }

        [Test]
        [RequiresPlayMode(false)]
        public async Task AsyncTryShouldCatchSpecifiedFrontException()
        {
            var result = await TryFactory
                .TryAsync(_
                    => throw new ArgumentOutOfRangeException())
                .Catch<ArgumentOutOfRangeException>(_ => "Caught")
                .Catch<NullReferenceException>(_ => "Failed")
                .ExecuteAsync(CancellationToken.None);

            result.Failure.Should().BeTrue();
            result.ExtractMessage().Should().Be("Caught\n");
        }

        [Test]
        [RequiresPlayMode(false)]
        public async Task AsyncTryWithValueShouldCatchSpecifiedFrontException()
        {
            var result = await TryFactory
                .TryAsync<int>(_
                    => throw new ArgumentOutOfRangeException())
                .Catch<int, ArgumentOutOfRangeException>(_ => "Caught")
                .Catch<int, NullReferenceException>(_ => "Failed")
                .ExecuteAsync(CancellationToken.None);

            result.Failure.Should().BeTrue();
            result.ExtractMessage().Should().Be("Caught\n");
        }


        [Test]
        [RequiresPlayMode(false)]
        public async Task AsyncTryShouldCatchSpecifiedBackException()
        {
            var result = await TryFactory
                .TryAsync(_
                    => throw new ArgumentOutOfRangeException())
                .Catch<NullReferenceException>(_ => "Failed")
                .Catch<ArgumentOutOfRangeException>(_ => "Caught")
                .ExecuteAsync(CancellationToken.None);

            result.Failure.Should().BeTrue();
            result.ExtractMessage().Should().Be("Caught\n");
        }

        [Test]
        [RequiresPlayMode(false)]
        public async Task AsyncTryWithValueShouldCatchSpecifiedBackException()
        {
            var result = await TryFactory
                .TryAsync<int>(_
                    => throw new ArgumentOutOfRangeException())
                .Catch<int, NullReferenceException>(_ => "Failed")
                .Catch<int, ArgumentOutOfRangeException>(_ => "Caught")
                .ExecuteAsync(CancellationToken.None);

            result.Failure.Should().BeTrue();
            result.ExtractMessage().Should().Be("Caught\n");
        }

        [Test]
        [RequiresPlayMode(false)]
        public async Task AsyncTryShouldCatchSpecifiedMediumException()
        {
            var result = await TryFactory
                .TryAsync(_
                    => throw new ArgumentOutOfRangeException())
                .Catch<NullReferenceException>(_ => "Failed")
                .Catch<ArgumentOutOfRangeException>(_ => "Caught")
                .Catch<ArgumentNullException>(_ => "Failed")
                .ExecuteAsync(CancellationToken.None);

            result.Failure.Should().BeTrue();
            result.ExtractMessage().Should().Be("Caught\n");
        }

        [Test]
        [RequiresPlayMode(false)]
        public async Task AsyncTryWithValueShouldCatchSpecifiedMediumException()
        {
            var result = await TryFactory
                .TryAsync<int>(_
                    => throw new ArgumentOutOfRangeException())
                .Catch<int, NullReferenceException>(_ => "Failed")
                .Catch<int, ArgumentOutOfRangeException>(_ => "Caught")
                .Catch<int, ArgumentNullException>(_ => "Failed")
                .ExecuteAsync(CancellationToken.None);

            result.Failure.Should().BeTrue();
            result.ExtractMessage().Should().Be("Caught\n");
        }

        [Test]
        [RequiresPlayMode(false)]
        public async Task AsyncTryShouldExecuteFinalizer()
        {
            var finalized = false;
            var result = await TryFactory
                .TryAsync(_ => throw new NullReferenceException())
                .Catch<NullReferenceException>(_ => "Caught")
                .Finalize(async () => finalized = true)
                .ExecuteAsync(CancellationToken.None);

            result.Failure.Should().BeTrue();
            result.ExtractMessage().Should().Be("Caught\n");
            finalized.Should().BeTrue();
        }

        [Test]
        [RequiresPlayMode(false)]
        public async Task AsyncTryWithValueShouldExecuteFinalizer()
        {
            var finalized = false;
            var result = await TryFactory
                .TryAsync<int>(_ => throw new NullReferenceException())
                .Catch<int, NullReferenceException>(_ => "Caught")
                .Finalize(async () => finalized = true)
                .ExecuteAsync(CancellationToken.None);

            result.Failure.Should().BeTrue();
            result.ExtractMessage().Should().Be("Caught\n");
            finalized.Should().BeTrue();
        }
    }
}