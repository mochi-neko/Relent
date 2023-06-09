#nullable enable
using System;
using FluentAssertions;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Mochineko.Relent.UncertainResult.Tests
{
    [TestFixture]
    internal sealed class UncertainResultExtensionsTest
    {
        [Test]
        [RequiresPlayMode(false)]
        public void UnwrapShouldSuccessForSuccessResult()
        {
            var result = UncertainResults.Succeed(1);

            result.Unwrap().Should().Be(1);
        }
        
        [Test]
        [RequiresPlayMode(false)]
        public void UnwrapShouldFailForRetryableResult()
        {
            Func<int> unwrap = UncertainResults.Retry<int>("Retryable")
                .Unwrap;

            unwrap.Should().Throw<InvalidOperationException>();
        }


        [Test]
        [RequiresPlayMode(false)]
        public void UnwrapShouldFailForFailureResult()
        {
            Func<int> unwrap = UncertainResults.Fail<int>("Fail")
                .Unwrap;

            unwrap.Should().Throw<InvalidOperationException>();
        }
        
        [Test]
        [RequiresPlayMode(false)]
        public void ExtractMessageShouldSuccessForRetryableResult()
        {
            var result = UncertainResults.Retry<int>("message");

            result.ExtractMessage().Should().Be("message");
        }

        [Test]
        [RequiresPlayMode(false)]
        public void ExtractMessageShouldSuccessForFailureResult()
        {
            var result = UncertainResults.Fail<int>("message");

            result.ExtractMessage().Should().Be("message");
        }

        [Test]
        [RequiresPlayMode(false)]
        public void ExtractMessageShouldFailForSuccessResult()
        {
            Func<string> extract = () => UncertainResults.Succeed(1)
                .ExtractMessage();

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