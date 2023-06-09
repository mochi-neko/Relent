#nullable enable
using System;
using FluentAssertions;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Mochineko.Relent.Result.Tests
{
    [TestFixture]
    internal sealed class ResultExtensionsTest
    {
        [Test]
        [RequiresPlayMode(false)]
        public void UnwrapShouldSuccessForSuccessResult()
        {
            IResult<int> result = Results.Succeed(1);

            result.Unwrap().Should().Be(1);
        }

        [Test]
        [RequiresPlayMode(false)]
        public void UnwrapShouldFailForFailureResult()
        {
            Func<int> unwrap = Results.Fail<int>("Fail").Unwrap;

            unwrap.Should().Throw<InvalidOperationException>();
        }

        [Test]
        [RequiresPlayMode(false)]
        public void ExtractMessageShouldSuccessForFailureResult()
        {
            IResult<int> result = Results.Fail<int>("message");

            result.ExtractMessage().Should().Be("message");
        }

        [Test]
        [RequiresPlayMode(false)]
        public void ExtractMessageShouldFailForSuccessResult()
        {
            Func<string> extract = () => Results.Succeed(1).ExtractMessage();

            extract.Should().Throw<InvalidOperationException>();
        }

        [Test]
        [RequiresPlayMode(false)]
        public void ToResultShouldHoldValue()
        {
            var value = 1;
            var result = value.ToResult();
            result.Unwrap().Should().Be(value);
        }
    }
}