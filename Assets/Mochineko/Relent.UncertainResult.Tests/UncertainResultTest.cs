#nullable enable
using System;
using FluentAssertions;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Mochineko.Relent.UncertainResult.Tests
{
    [TestFixture]
    internal sealed class UncertainResultTest
    {
        [Test]
        [RequiresPlayMode(false)]
        public void SuccessTest()
        {
            IUncertainResult<string> result = UncertainResults.Succeed("Test");

            result.Success.Should().BeTrue();
            result.Retryable.Should().BeFalse();
            result.Failure.Should().BeFalse();
        }

        [Test]
        [RequiresPlayMode(false)]
        public void RetryTest()
        {
            IUncertainResult<string> result = UncertainResults.Retry<string>("Test");

            result.Success.Should().BeFalse();
            result.Retryable.Should().BeTrue();
            result.Failure.Should().BeFalse();
        }

        [Test]
        [RequiresPlayMode(false)]
        public void FailureTest()
        {
            IUncertainResult<string> result = UncertainResults.Fail<string>("Test");

            result.Success.Should().BeFalse();
            result.Retryable.Should().BeFalse();
            result.Failure.Should().BeTrue();
        }

        [Test]
        [RequiresPlayMode(false)]
        public void HappyPathTest()
        {
            IUncertainResult<string> result = UncertainResults.Succeed("Test");

            if (result.Success)
            {
                // Pass
            }
            else if (result.Retryable)
            {
                throw new Exception();
            }
            else // Failure
            {
                throw new Exception();
            }
        }

        [Test]
        [RequiresPlayMode(false)]
        public void RetryablePathTest()
        {
            IUncertainResult<string> result = UncertainResults.Retry<string>("Test");

            if (result.Success)
            {
                throw new Exception();
            }
            else if (result.Retryable)
            {
                // Pass
            }
            else // Failure
            {
                throw new Exception();
            }
        }

        [Test]
        [RequiresPlayMode(false)]
        public void ExceptionPathTest()
        {
            IUncertainResult<string> result = UncertainResults.Fail<string>("Test");

            if (result.Success)
            {
                throw new Exception();
            }
            else if (result.Retryable)
            {
                throw new Exception();
            }
            else // Failure
            {
                // Pass
            }
        }

        [Test]
        [RequiresPlayMode(false)]
        public void HappyPathByPatternMatchingTest()
        {
            IUncertainResult<string> result = UncertainResults.Succeed<string>("Test");

            if (result is IUncertainSuccessResult<string> success)
            {
                // Pass
                success.Success.Should().BeTrue();
                success.Retryable.Should().BeFalse();
                success.Failure.Should().BeFalse();
                success.Result.Should().Be("Test");
            }
            else if (result is IUncertainRetryableResult<string> retryable)
            {
                throw new Exception();
            }
            else if (result is IUncertainFailureResult<string> failure)
            {
                throw new Exception();
            }
            else
            {
                throw new UncertainResultPatternMatchException(nameof(result));
            }
        }

        [Test]
        [RequiresPlayMode(false)]
        public void RetryablePathByPatternMatchingTest()
        {
            IUncertainResult<string> result = UncertainResults.Retry<string>("Test");

            if (result is IUncertainSuccessResult<string> success)
            {
                throw new Exception();
            }
            else if (result is IUncertainRetryableResult<string> retryable)
            {
                // Pass
                retryable.Success.Should().BeFalse();
                retryable.Retryable.Should().BeTrue();
                retryable.Failure.Should().BeFalse();
                retryable.Message.Should().Be("Test");
            }
            else if (result is IUncertainFailureResult<string> failure)
            {
                throw new Exception();
            }
            else
            {
                throw new UncertainResultPatternMatchException(nameof(result));
            }
        }

        [Test]
        [RequiresPlayMode(false)]
        public void ExceptionPathByPatternMatchingTest()
        {
            IUncertainResult<string> result = UncertainResults.Fail<string>("Test");

            if (result is IUncertainSuccessResult<string> success)
            {
                throw new Exception();
            }
            else if (result is IUncertainRetryableResult<string> retryable)
            {
                throw new Exception();
            }
            else if (result is IUncertainFailureResult<string> failure)
            {
                // Pass
                failure.Success.Should().BeFalse();
                failure.Retryable.Should().BeFalse();
                failure.Failure.Should().BeTrue();
                failure.Message.Should().Be("Test");
            }
            else
            {
                throw new UncertainResultPatternMatchException(nameof(result));
            }
        }
    }
}