#nullable enable
using System;
using FluentAssertions;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Mochineko.Relent.UncertainResult.Tests
{
    [TestFixture]
    internal sealed class UncertainTryExtensionsTest
    {
        [Test]
        [RequiresPlayMode(false)]
        public void TryShouldSuccessWithNoException()
        {
            var result = UncertainTryFactory
                .Try(() =>
                {
                    // Do nothing.
                })
                .Execute();

            result.Success.Should().BeTrue();
        }

        [Test]
        [RequiresPlayMode(false)]
        public void TryWithValueShouldSuccessWithNoException()
        {
            var result = UncertainTryFactory
                .Try(() => 1)
                .Execute();

            result.Success.Should().BeTrue();
            result.Unwrap().Should().Be(1);
        }
        
        [Test]
        [RequiresPlayMode(false)]
        public void TryShouldCatchSpecifiedExceptionAsRetryable()
        {
            var result = UncertainTryFactory
                .Try(() => { throw new NullReferenceException(); })
                .CatchAsRetryable<NullReferenceException>(_ => "Caught")
                .Execute();

            result.Retryable.Should().BeTrue();
            result.ExtractMessage().Should().Be("Caught");
        }

        [Test]
        [RequiresPlayMode(false)]
        public void TryWithValueShouldCatchSpecifiedExceptionAsRetryable()
        {
            var result = UncertainTryFactory
                .Try<int>(() => { throw new NullReferenceException(); })
                .CatchAsRetryable<int, NullReferenceException>(_ => "Caught")
                .Execute();

            result.Retryable.Should().BeTrue();
            result.ExtractMessage().Should().Be("Caught");
        }

        [Test]
        [RequiresPlayMode(false)]
        public void TryShouldCatchSpecifiedExceptionAsFailure()
        {
            var result = UncertainTryFactory
                .Try(() => { throw new NullReferenceException(); })
                .CatchAsFailure<NullReferenceException>(_ => "Caught")
                .Execute();

            result.Failure.Should().BeTrue();
            result.ExtractMessage().Should().Be("Caught");
        }

        [Test]
        [RequiresPlayMode(false)]
        public void TryWithValueShouldCatchSpecifiedExceptionAsFailure()
        {
            var result = UncertainTryFactory
                .Try<int>(() => { throw new NullReferenceException(); })
                .CatchAsFailure<int, NullReferenceException>(_ => "Caught")
                .Execute();

            result.Failure.Should().BeTrue();
            result.ExtractMessage().Should().Be("Caught");
        }

        [Test]
        [RequiresPlayMode(false)]
        public void TryShouldNotCatchNoSpecifiedException()
        {
            Func<IUncertainResult> tryExtension = () => UncertainTryFactory
                .Try(() => throw new InvalidCastException())
                .CatchAsRetryable<ArgumentOutOfRangeException>(_ => "Failed")
                .CatchAsFailure<NullReferenceException>(_ => "Failed")
                .Execute();

            tryExtension.Should().Throw<InvalidCastException>();
        }

        [Test]
        [RequiresPlayMode(false)]
        public void TryWithValueShouldNotCatchNoSpecifiedException()
        {
            Func<IUncertainResult<int>> tryExtension = () => UncertainTryFactory
                .Try<int>(() => throw new InvalidCastException())
                .CatchAsRetryable<int, ArgumentOutOfRangeException>(_ => "Failed")
                .CatchAsFailure<int, NullReferenceException>(_ => "Failed")
                .Execute();

            tryExtension.Should().Throw<InvalidCastException>();
        }

        [Test]
        [RequiresPlayMode(false)]
        public void TryShouldCatchSpecifiedFrontExceptionAsRetryable()
        {
            var result = UncertainTryFactory
                .Try(()
                    => throw new ArgumentOutOfRangeException())
                .CatchAsRetryable<ArgumentOutOfRangeException>(_ => "Caught")
                .CatchAsRetryable<NullReferenceException>(_ => "Failed")
                .Execute();

            result.Retryable.Should().BeTrue();
            result.ExtractMessage().Should().Be("Caught");
        }

        [Test]
        [RequiresPlayMode(false)]
        public void TryWithValueShouldCatchSpecifiedFrontExceptionAsRetryable()
        {
            var result = UncertainTryFactory
                .Try<int>(()
                    => throw new ArgumentOutOfRangeException())
                .CatchAsRetryable<int, ArgumentOutOfRangeException>(_ => "Caught")
                .CatchAsRetryable<int, NullReferenceException>(_ => "Failed")
                .Execute();

            result.Retryable.Should().BeTrue();
            result.ExtractMessage().Should().Be("Caught");
        }


        [Test]
        [RequiresPlayMode(false)]
        public void TryShouldCatchSpecifiedBackExceptionAsRetryable()
        {
            var result = UncertainTryFactory
                .Try(()
                    => throw new ArgumentOutOfRangeException())
                .CatchAsRetryable<NullReferenceException>(_ => "Failed")
                .CatchAsRetryable<ArgumentOutOfRangeException>(_ => "Caught")
                .Execute();

            result.Retryable.Should().BeTrue();
            result.ExtractMessage().Should().Be("Caught");
        }

        [Test]
        [RequiresPlayMode(false)]
        public void TryWithValueShouldCatchSpecifiedBackExceptionAsRetryable()
        {
            var result = UncertainTryFactory
                .Try<int>(()
                    => throw new ArgumentOutOfRangeException())
                .CatchAsRetryable<int, NullReferenceException>(_ => "Failed")
                .CatchAsRetryable<int, ArgumentOutOfRangeException>(_ => "Caught")
                .Execute();

            result.Retryable.Should().BeTrue();
            result.ExtractMessage().Should().Be("Caught");
        }
        
        [Test]
        [RequiresPlayMode(false)]
        public void TryShouldCatchSpecifiedFrontException()
        {
            var result = UncertainTryFactory
                .Try(()
                    => throw new ArgumentOutOfRangeException())
                .CatchAsFailure<ArgumentOutOfRangeException>(_ => "Caught")
                .CatchAsFailure<NullReferenceException>(_ => "Failed")
                .Execute();

            result.Failure.Should().BeTrue();
            result.ExtractMessage().Should().Be("Caught");
        }

        [Test]
        [RequiresPlayMode(false)]
        public void TryWithValueShouldCatchSpecifiedFrontException()
        {
            var result = UncertainTryFactory
                .Try<int>(()
                    => throw new ArgumentOutOfRangeException())
                .CatchAsFailure<int, ArgumentOutOfRangeException>(_ => "Caught")
                .CatchAsFailure<int, NullReferenceException>(_ => "Failed")
                .Execute();

            result.Failure.Should().BeTrue();
            result.ExtractMessage().Should().Be("Caught");
        }


        [Test]
        [RequiresPlayMode(false)]
        public void TryShouldCatchSpecifiedBackException()
        {
            var result = UncertainTryFactory
                .Try(()
                    => throw new ArgumentOutOfRangeException())
                .CatchAsFailure<NullReferenceException>(_ => "Failed")
                .CatchAsFailure<ArgumentOutOfRangeException>(_ => "Caught")
                .Execute();

            result.Failure.Should().BeTrue();
            result.ExtractMessage().Should().Be("Caught");
        }

        [Test]
        [RequiresPlayMode(false)]
        public void TryWithValueShouldCatchSpecifiedBackException()
        {
            var result = UncertainTryFactory
                .Try<int>(()
                    => throw new ArgumentOutOfRangeException())
                .CatchAsFailure<int, NullReferenceException>(_ => "Failed")
                .CatchAsFailure<int, ArgumentOutOfRangeException>(_ => "Caught")
                .Execute();

            result.Failure.Should().BeTrue();
            result.ExtractMessage().Should().Be("Caught");
        }

        [Test]
        [RequiresPlayMode(false)]
        public void TryShouldCatchSpecifiedMediumException()
        {
            var result = UncertainTryFactory
                .Try(()
                    => throw new ArgumentOutOfRangeException())
                .CatchAsFailure<NullReferenceException>(_ => "Failed")
                .CatchAsRetryable<ArgumentOutOfRangeException>(_ => "Caught")
                .CatchAsFailure<ArgumentNullException>(_ => "Failed")
                .Execute();

            result.Retryable.Should().BeTrue();
            result.ExtractMessage().Should().Be("Caught");
        }

        [Test]
        [RequiresPlayMode(false)]
        public void TryWithValueShouldCatchSpecifiedMediumException()
        {
            var result = UncertainTryFactory
                .Try<int>(()
                    => throw new ArgumentOutOfRangeException())
                .CatchAsFailure<int, NullReferenceException>(_ => "Failed")
                .CatchAsRetryable<int, ArgumentOutOfRangeException>(_ => "Caught")
                .CatchAsFailure<int, ArgumentNullException>(_ => "Failed")
                .Execute();

            result.Retryable.Should().BeTrue();
            result.ExtractMessage().Should().Be("Caught");
        }

        [Test]
        [RequiresPlayMode(false)]
        public void TryShouldExecuteFinalizer()
        {
            var finalized = false;
            var result = UncertainTryFactory
                .Try(() => throw new NullReferenceException())
                .CatchAsFailure<NullReferenceException>(_ => "Caught")
                .CatchAsRetryable<ArgumentOutOfRangeException>(_ => "Failed")
                .Finalize(() => finalized = true)
                .Execute();

            result.Failure.Should().BeTrue();
            result.ExtractMessage().Should().Be("Caught");
            finalized.Should().BeTrue();
        }

        [Test]
        [RequiresPlayMode(false)]
        public void TryWithValueShouldExecuteFinalizer()
        {
            var finalized = false;
            var result = UncertainTryFactory
                .Try<int>(() => throw new NullReferenceException())
                .CatchAsFailure<int, NullReferenceException>(_ => "Caught")
                .CatchAsRetryable<int, ArgumentOutOfRangeException>(_ => "Failed")
                .Finalize(() => finalized = true)
                .Execute();

            result.Failure.Should().BeTrue();
            result.ExtractMessage().Should().Be("Caught");
            finalized.Should().BeTrue();
        }
    }
}