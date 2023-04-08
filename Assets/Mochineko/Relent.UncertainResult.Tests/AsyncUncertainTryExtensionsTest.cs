#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using UnityEngine.TestTools;

#pragma warning disable CS1998

namespace Mochineko.Relent.UncertainResult.Tests
{
    [TestFixture]
    internal sealed class AsyncUncertainTryAsyncExtensionsTest
    {
        [Test]
        [RequiresPlayMode(false)]
        public async Task TryAsyncShouldSuccessWithNoException()
        {
            var result = await UncertainTryFactory
                .TryAsync(async _ =>
                {
                    // Do nothing.
                })
                .ExecuteAsync(CancellationToken.None);

            result.Success.Should().BeTrue();
        }

        [Test]
        [RequiresPlayMode(false)]
        public async Task TryAsyncWithValueShouldSuccessWithNoException()
        {
            var result = await UncertainTryFactory
                .TryAsync(async _ => 1)
                .ExecuteAsync(CancellationToken.None);

            result.Success.Should().BeTrue();
            result.Unwrap().Should().Be(1);
        }

        [Test]
        [RequiresPlayMode(false)]
        public async Task TryAsyncShouldCatchSpecifiedExceptionAsReTryAsyncable()
        {
            var result = await UncertainTryFactory
                .TryAsync(_ => { throw new NullReferenceException(); })
                .CatchAsRetryable<NullReferenceException>(_ => "Caught")
                .ExecuteAsync(CancellationToken.None);

            result.Retryable.Should().BeTrue();
            result.ExtractMessage().Should().Be("Caught\n");
        }

        [Test]
        [RequiresPlayMode(false)]
        public async Task TryAsyncWithValueShouldCatchSpecifiedExceptionAsReTryAsyncable()
        {
            var result = await UncertainTryFactory
                .TryAsync<int>(_ => { throw new NullReferenceException(); })
                .CatchAsRetryable<int, NullReferenceException>(_ => "Caught")
                .ExecuteAsync(CancellationToken.None);

            result.Retryable.Should().BeTrue();
            result.ExtractMessage().Should().Be("Caught\n");
        }

        [Test]
        [RequiresPlayMode(false)]
        public async Task TryAsyncShouldCatchSpecifiedExceptionAsFailure()
        {
            var result = await UncertainTryFactory
                .TryAsync(_ => { throw new NullReferenceException(); })
                .CatchAsFailure<NullReferenceException>(_ => "Caught")
                .ExecuteAsync(CancellationToken.None);

            result.Failure.Should().BeTrue();
            result.ExtractMessage().Should().Be("Caught\n");
        }

        [Test]
        [RequiresPlayMode(false)]
        public async Task TryAsyncWithValueShouldCatchSpecifiedExceptionAsFailure()
        {
            var result = await UncertainTryFactory
                .TryAsync<int>(_ => { throw new NullReferenceException(); })
                .CatchAsFailure<int, NullReferenceException>(_ => "Caught")
                .ExecuteAsync(CancellationToken.None);

            result.Failure.Should().BeTrue();
            result.ExtractMessage().Should().Be("Caught\n");
        }

        [Test]
        [RequiresPlayMode(false)]
        public async Task TryAsyncShouldNotCatchNoSpecifiedException()
        {
            Func<Task<IUncertainResult>> tryAsyncExtension = async () => await UncertainTryFactory
                .TryAsync(_ => throw new InvalidCastException())
                .CatchAsRetryable<ArgumentOutOfRangeException>(_ => "Failed")
                .CatchAsFailure<NullReferenceException>(_ => "Failed")
                .ExecuteAsync(CancellationToken.None);

            await tryAsyncExtension.Should().ThrowAsync<InvalidCastException>();
        }

        [Test]
        [RequiresPlayMode(false)]
        public async Task TryAsyncWithValueShouldNotCatchNoSpecifiedException()
        {
            Func<Task<IUncertainResult<int>>> tryAsyncExtension = async () => await UncertainTryFactory
                .TryAsync<int>(_ => throw new InvalidCastException())
                .CatchAsRetryable<int, ArgumentOutOfRangeException>(_ => "Failed")
                .CatchAsFailure<int, NullReferenceException>(_ => "Failed")
                .ExecuteAsync(CancellationToken.None);

            await tryAsyncExtension.Should().ThrowAsync<InvalidCastException>();
        }

        [Test]
        [RequiresPlayMode(false)]
        public async Task TryAsyncShouldCatchSpecifiedFrontExceptionAsReTryAsyncable()
        {
            var result = await UncertainTryFactory
                .TryAsync(_ => throw new ArgumentOutOfRangeException())
                .CatchAsRetryable<ArgumentOutOfRangeException>(_ => "Caught")
                .CatchAsRetryable<NullReferenceException>(_ => "Failed")
                .ExecuteAsync(CancellationToken.None);

            result.Retryable.Should().BeTrue();
            result.ExtractMessage().Should().Be("Caught\n");
        }

        [Test]
        [RequiresPlayMode(false)]
        public async Task TryAsyncWithValueShouldCatchSpecifiedFrontExceptionAsReTryAsyncable()
        {
            var result = await UncertainTryFactory
                .TryAsync<int>(_
                    => throw new ArgumentOutOfRangeException())
                .CatchAsRetryable<int, ArgumentOutOfRangeException>(_ => "Caught")
                .CatchAsRetryable<int, NullReferenceException>(_ => "Failed")
                .ExecuteAsync(CancellationToken.None);

            result.Retryable.Should().BeTrue();
            result.ExtractMessage().Should().Be("Caught\n");
        }


        [Test]
        [RequiresPlayMode(false)]
        public async Task TryAsyncShouldCatchSpecifiedBackExceptionAsReTryAsyncable()
        {
            var result = await UncertainTryFactory
                .TryAsync(_
                    => throw new ArgumentOutOfRangeException())
                .CatchAsRetryable<NullReferenceException>(_ => "Failed")
                .CatchAsRetryable<ArgumentOutOfRangeException>(_ => "Caught")
                .ExecuteAsync(CancellationToken.None);

            result.Retryable.Should().BeTrue();
            result.ExtractMessage().Should().Be("Caught\n");
        }

        [Test]
        [RequiresPlayMode(false)]
        public async Task TryAsyncWithValueShouldCatchSpecifiedBackExceptionAsReTryAsyncable()
        {
            var result = await UncertainTryFactory
                .TryAsync<int>(_
                    => throw new ArgumentOutOfRangeException())
                .CatchAsRetryable<int, NullReferenceException>(_ => "Failed")
                .CatchAsRetryable<int, ArgumentOutOfRangeException>(_ => "Caught")
                .ExecuteAsync(CancellationToken.None);

            result.Retryable.Should().BeTrue();
            result.ExtractMessage().Should().Be("Caught\n");
        }

        [Test]
        [RequiresPlayMode(false)]
        public async Task TryAsyncShouldCatchSpecifiedFrontException()
        {
            var result = await UncertainTryFactory
                .TryAsync(_
                    => throw new ArgumentOutOfRangeException())
                .CatchAsFailure<ArgumentOutOfRangeException>(_ => "Caught")
                .CatchAsFailure<NullReferenceException>(_ => "Failed")
                .ExecuteAsync(CancellationToken.None);

            result.Failure.Should().BeTrue();
            result.ExtractMessage().Should().Be("Caught\n");
        }

        [Test]
        [RequiresPlayMode(false)]
        public async Task TryAsyncWithValueShouldCatchSpecifiedFrontException()
        {
            var result = await UncertainTryFactory
                .TryAsync<int>(_
                    => throw new ArgumentOutOfRangeException())
                .CatchAsFailure<int, ArgumentOutOfRangeException>(_ => "Caught")
                .CatchAsFailure<int, NullReferenceException>(_ => "Failed")
                .ExecuteAsync(CancellationToken.None);

            result.Failure.Should().BeTrue();
            result.ExtractMessage().Should().Be("Caught\n");
        }


        [Test]
        [RequiresPlayMode(false)]
        public async Task TryAsyncShouldCatchSpecifiedBackException()
        {
            var result = await UncertainTryFactory
                .TryAsync(_
                    => throw new ArgumentOutOfRangeException())
                .CatchAsFailure<NullReferenceException>(_ => "Failed")
                .CatchAsFailure<ArgumentOutOfRangeException>(_ => "Caught")
                .ExecuteAsync(CancellationToken.None);

            result.Failure.Should().BeTrue();
            result.ExtractMessage().Should().Be("Caught\n");
        }

        [Test]
        [RequiresPlayMode(false)]
        public async Task TryAsyncWithValueShouldCatchSpecifiedBackException()
        {
            var result = await UncertainTryFactory
                .TryAsync<int>(_
                    => throw new ArgumentOutOfRangeException())
                .CatchAsFailure<int, NullReferenceException>(_ => "Failed")
                .CatchAsFailure<int, ArgumentOutOfRangeException>(_ => "Caught")
                .ExecuteAsync(CancellationToken.None);

            result.Failure.Should().BeTrue();
            result.ExtractMessage().Should().Be("Caught\n");
        }

        [Test]
        [RequiresPlayMode(false)]
        public async Task TryAsyncShouldCatchSpecifiedMediumException()
        {
            var result = await UncertainTryFactory
                .TryAsync(_
                    => throw new ArgumentOutOfRangeException())
                .CatchAsFailure<NullReferenceException>(_ => "Failed")
                .CatchAsRetryable<ArgumentOutOfRangeException>(_ => "Caught")
                .CatchAsFailure<ArgumentNullException>(_ => "Failed")
                .ExecuteAsync(CancellationToken.None);

            result.Retryable.Should().BeTrue();
            result.ExtractMessage().Should().Be("Caught\n");
        }

        [Test]
        [RequiresPlayMode(false)]
        public async Task TryAsyncWithValueShouldCatchSpecifiedMediumException()
        {
            var result = await UncertainTryFactory
                .TryAsync<int>(_
                    => throw new ArgumentOutOfRangeException())
                .CatchAsFailure<int, NullReferenceException>(_ => "Failed")
                .CatchAsRetryable<int, ArgumentOutOfRangeException>(_ => "Caught")
                .CatchAsFailure<int, ArgumentNullException>(_ => "Failed")
                .ExecuteAsync(CancellationToken.None);

            result.Retryable.Should().BeTrue();
            result.ExtractMessage().Should().Be("Caught\n");
        }

        [Test]
        [RequiresPlayMode(false)]
        public async Task TryAsyncShouldExecuteFinalizer()
        {
            var finalized = false;
            var result = await UncertainTryFactory
                .TryAsync(_ => throw new NullReferenceException())
                .CatchAsFailure<NullReferenceException>(_ => "Caught")
                .CatchAsRetryable<ArgumentOutOfRangeException>(_ => "Failed")
                .Finalize(async () => finalized = true)
                .ExecuteAsync(CancellationToken.None);

            result.Failure.Should().BeTrue();
            result.ExtractMessage().Should().Be("Caught\n");
            finalized.Should().BeTrue();
        }

        [Test]
        [RequiresPlayMode(false)]
        public async Task TryAsyncWithValueShouldExecuteFinalizer()
        {
            var finalized = false;
            var result = await UncertainTryFactory
                .TryAsync<int>(_ => throw new NullReferenceException())
                .CatchAsFailure<int, NullReferenceException>(_ => "Caught")
                .CatchAsRetryable<int, ArgumentOutOfRangeException>(_ => "Failed")
                .Finalize(async () => finalized = true)
                .ExecuteAsync(CancellationToken.None);

            result.Failure.Should().BeTrue();
            result.ExtractMessage().Should().Be("Caught\n");
            finalized.Should().BeTrue();
        }
    }
}