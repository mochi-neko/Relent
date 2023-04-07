#nullable enable
using System;
using FluentAssertions;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Mochineko.Relent.Result.Tests
{
    [TestFixture]
    internal sealed class TryExtensionsTest
    {
        [Test]
        [RequiresPlayMode(false)]
        public void TryShouldSuccessWithNoException()
        {
            var result = TryExtensions
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
            var result = TryExtensions
                .Try(() => 1)
                .Execute();

            result.Success.Should().BeTrue();
            result.Unwrap().Should().Be(1);
        }

        [Test]
        [RequiresPlayMode(false)]
        public void TryShouldCatchSpecifiedException()
        {
            var result = TryExtensions
                .Try(() => { throw new NullReferenceException(); })
                .Catch<NullReferenceException>(_ => "Caught")
                .Execute();

            result.Failure.Should().BeTrue();
            result.ExtractMessage().Should().Be("Caught");
        }

        [Test]
        [RequiresPlayMode(false)]
        public void TryWithValueShouldCatchSpecifiedException()
        {
            var result = TryExtensions
                .Try<int>(() => { throw new NullReferenceException(); })
                .Catch<int, NullReferenceException>(_ => "Caught")
                .Execute();

            result.Failure.Should().BeTrue();
            result.ExtractMessage().Should().Be("Caught");
        }

        [Test]
        [RequiresPlayMode(false)]
        public void TryShouldNotCatchNoSpecifiedException()
        {
            Func<IResult> tryExtension = () => TryExtensions
                .Try(() => throw new InvalidCastException())
                .Catch<NullReferenceException>(_ => "Failed")
                .Execute();

            tryExtension.Should().Throw<InvalidCastException>();
        }

        [Test]
        [RequiresPlayMode(false)]
        public void TryWithValueShouldNotCatchNoSpecifiedException()
        {
            Func<IResult<int>> tryExtension = () => TryExtensions
                .Try<int>(() => throw new InvalidCastException())
                .Catch<int, NullReferenceException>(_ => "Failed")
                .Execute();

            tryExtension.Should().Throw<InvalidCastException>();
        }

        [Test]
        [RequiresPlayMode(false)]
        public void TryShouldCatchSpecifiedFrontException()
        {
            var result = TryExtensions
                .Try(()
                    => throw new ArgumentOutOfRangeException())
                .Catch<ArgumentOutOfRangeException>(_ => "Caught")
                .Catch<NullReferenceException>(_ => "Failed")
                .Execute();

            result.Failure.Should().BeTrue();
            result.ExtractMessage().Should().Be("Caught");
        }

        [Test]
        [RequiresPlayMode(false)]
        public void TryWithValueShouldCatchSpecifiedFrontException()
        {
            var result = TryExtensions
                .Try<int>(()
                    => throw new ArgumentOutOfRangeException())
                .Catch<int, ArgumentOutOfRangeException>(_ => "Caught")
                .Catch<int, NullReferenceException>(_ => "Failed")
                .Execute();

            result.Failure.Should().BeTrue();
            result.ExtractMessage().Should().Be("Caught");
        }


        [Test]
        [RequiresPlayMode(false)]
        public void TryShouldCatchSpecifiedBackException()
        {
            var result = TryExtensions
                .Try(()
                    => throw new ArgumentOutOfRangeException())
                .Catch<NullReferenceException>(_ => "Failed")
                .Catch<ArgumentOutOfRangeException>(_ => "Caught")
                .Execute();

            result.Failure.Should().BeTrue();
            result.ExtractMessage().Should().Be("Caught");
        }

        [Test]
        [RequiresPlayMode(false)]
        public void TryWithValueShouldCatchSpecifiedBackException()
        {
            var result = TryExtensions
                .Try<int>(()
                    => throw new ArgumentOutOfRangeException())
                .Catch<int, NullReferenceException>(_ => "Failed")
                .Catch<int, ArgumentOutOfRangeException>(_ => "Caught")
                .Execute();

            result.Failure.Should().BeTrue();
            result.ExtractMessage().Should().Be("Caught");
        }

        [Test]
        [RequiresPlayMode(false)]
        public void TryShouldCatchSpecifiedMediumException()
        {
            var result = TryExtensions
                .Try(()
                    => throw new ArgumentOutOfRangeException())
                .Catch<NullReferenceException>(_ => "Failed")
                .Catch<ArgumentOutOfRangeException>(_ => "Caught")
                .Catch<ArgumentNullException>(_ => "Failed")
                .Execute();

            result.Failure.Should().BeTrue();
            result.ExtractMessage().Should().Be("Caught");
        }

        [Test]
        [RequiresPlayMode(false)]
        public void TryWithValueShouldCatchSpecifiedMediumException()
        {
            var result = TryExtensions
                .Try<int>(()
                    => throw new ArgumentOutOfRangeException())
                .Catch<int, NullReferenceException>(_ => "Failed")
                .Catch<int, ArgumentOutOfRangeException>(_ => "Caught")
                .Catch<int, ArgumentNullException>(_ => "Failed")
                .Execute();

            result.Failure.Should().BeTrue();
            result.ExtractMessage().Should().Be("Caught");
        }

        [Test]
        [RequiresPlayMode(false)]
        public void TryShouldExecuteFinalizer()
        {
            var finalized = false;
            var result = TryExtensions
                .Try(() => throw new NullReferenceException())
                .Catch<NullReferenceException>(_ => "Caught")
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
            var result = TryExtensions
                .Try<int>(() => throw new NullReferenceException())
                .Catch<int, NullReferenceException>(_ => "Caught")
                .Finalize(() => finalized = true)
                .Execute();

            result.Failure.Should().BeTrue();
            result.ExtractMessage().Should().Be("Caught");
            finalized.Should().BeTrue();
        }
    }
}