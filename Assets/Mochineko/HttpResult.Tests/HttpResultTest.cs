#nullable enable
using System;
using FluentAssertions;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Mochineko.HttpResult.Tests
{
    [TestFixture]
    internal sealed class HttpResultTest
    {
        [Test]
        [RequiresPlayMode(false)]
        public void SuccessTest()
        {
            var result = HttpResult.Ok("Test");

            result.Success.Should().BeTrue();
            result.Retryable.Should().BeFalse();
            result.Failure.Should().BeFalse();
        }

        [Test]
        [RequiresPlayMode(false)]
        public void RetryTest()
        {
            var result = HttpResult.Retry<string>("Test");

            result.Success.Should().BeFalse();
            result.Retryable.Should().BeTrue();
            result.Failure.Should().BeFalse();
        }

        [Test]
        [RequiresPlayMode(false)]
        public void FailureTest()
        {
            var result = HttpResult.Fail<string>("Test");

            result.Success.Should().BeFalse();
            result.Retryable.Should().BeFalse();
            result.Failure.Should().BeTrue();
        }

        [Test]
        [RequiresPlayMode(false)]
        public void HappyPathTest()
        {
            var result = HttpResult.Ok("Test");

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
            var result = HttpResult.Retry<string>("Test");

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
            var result = HttpResult.Fail<string>("Test");

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
            var result = HttpResult.Ok<string>("Test");

            if (result is IHttpSuccessResult<string> success)
            {
                // Pass
                success.Success.Should().BeTrue();
                success.Retryable.Should().BeFalse();
                success.Failure.Should().BeFalse();
                success.Result.Should().Be("Test");
            }
            else if (result is IHttpRetryableResult<string> retryable)
            {
                throw new Exception();
            }
            else if (result is IHttpFailureResult<string> failure)
            {
                throw new Exception();
            }
            else
            {
                throw new Exception();
            }
        }

        [Test]
        [RequiresPlayMode(false)]
        public void RetryablePathByPatternMatchingTest()
        {
            var result = HttpResult.Retry<string>("Test");

            if (result is IHttpSuccessResult<string> success)
            {
                throw new Exception();
            }
            else if (result is IHttpRetryableResult<string> retryable)
            {
                // Pass
                retryable.Success.Should().BeFalse();
                retryable.Retryable.Should().BeTrue();
                retryable.Failure.Should().BeFalse();
                retryable.Message.Should().Be("Test");
            }
            else if (result is IHttpFailureResult<string> failure)
            {
                throw new Exception();
            }
            else
            {
                throw new Exception();
            }
        }

        [Test]
        [RequiresPlayMode(false)]
        public void ExceptionPathByPatternMatchingTest()
        {
            var result = HttpResult.Fail<string>("Test");

            if (result is IHttpSuccessResult<string> success)
            {
                throw new Exception();
            }
            else if (result is IHttpRetryableResult<string> retryable)
            {
                throw new Exception();
            }
            else if (result is IHttpFailureResult<string> failure)
            {
                // Pass
                failure.Success.Should().BeFalse();
                failure.Retryable.Should().BeFalse();
                failure.Failure.Should().BeTrue();
                failure.Message.Should().Be("Test");
            }
            else
            {
                throw new Exception();
            }
        }
    }
}