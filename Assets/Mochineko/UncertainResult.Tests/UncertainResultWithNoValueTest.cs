#nullable enable
using System;
using FluentAssertions;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Mochineko.UncertainResult.Tests
{
    [TestFixture]
    internal sealed class UncertainResultWithNoValueTest
    {
        [Test]
        [RequiresPlayMode(false)]
        public void SuccessTest()
        {
            IUncertainResult result = UncertainResultFactory.Succeed();

            result.Success.Should().BeTrue();
            result.Retryable.Should().BeFalse();
            result.Failure.Should().BeFalse();
        }

        [Test]
        [RequiresPlayMode(false)]
        public void RetryTest()
        {
            IUncertainResult result = UncertainResultFactory.Retry("Test");

            result.Success.Should().BeFalse();
            result.Retryable.Should().BeTrue();
            result.Failure.Should().BeFalse();
        }

        [Test]
        [RequiresPlayMode(false)]
        public void FailureTest()
        {
            IUncertainResult result = UncertainResultFactory.Fail("Test");

            result.Success.Should().BeFalse();
            result.Retryable.Should().BeFalse();
            result.Failure.Should().BeTrue();
        }

        [Test]
        [RequiresPlayMode(false)]
        public void HappyPathTest()
        {
            IUncertainResult result = UncertainResultFactory.Succeed();

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
            IUncertainResult result = UncertainResultFactory.Retry("Test");

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
            IUncertainResult result = UncertainResultFactory.Fail("Test");

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
            IUncertainResult result = UncertainResultFactory.Succeed();

            if (result is IUncertainSuccessResult success)
            {
                // Pass
                success.Success.Should().BeTrue();
                success.Retryable.Should().BeFalse();
                success.Failure.Should().BeFalse();
            }
            else if (result is IUncertainRetryableResult retryable)
            {
                throw new Exception();
            }
            else if (result is IUncertainFailureResult failure)
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
            IUncertainResult result = UncertainResultFactory.Retry("Test");

            if (result is IUncertainSuccessResult success)
            {
                throw new Exception();
            }
            else if (result is IUncertainRetryableResult retryable)
            {
                // Pass
                retryable.Success.Should().BeFalse();
                retryable.Retryable.Should().BeTrue();
                retryable.Failure.Should().BeFalse();
                retryable.Message.Should().Be("Test");
            }
            else if (result is IUncertainFailureResult failure)
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
            IUncertainResult result = UncertainResultFactory.Fail("Test");

            if (result is IUncertainSuccessResult success)
            {
                throw new Exception();
            }
            else if (result is IUncertainRetryableResult retryable)
            {
                throw new Exception();
            }
            else if (result is IUncertainFailureResult failure)
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