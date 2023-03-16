#nullable enable
using System;
using FluentAssertions;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Mochineko.Result.Tests
{
    [TestFixture]
    internal sealed class ResultTest
    {
        [Test]
        [RequiresPlayMode(false)]
        public void SuccessTest()
        {
            IResult<int> result = Result.Succeed(1);

            result.Success.Should().BeTrue();
            result.Failure.Should().BeFalse();
        }

        [Test]
        [RequiresPlayMode(false)]
        public void FailureTest()
        {
            IResult<int> result = Result.Fail<int>("Test");

            result.Success.Should().BeFalse();
            result.Failure.Should().BeTrue();
        }
        
        [Test]
        [RequiresPlayMode(false)]
        public void HappyPathTest()
        {
            IResult<int> result = Result.Succeed(1);

            if (result.Success)
            {
                // Pass
            }
            else
            {
                throw new Exception();
            }
        }

        [Test]
        [RequiresPlayMode(false)]
        public void ExceptionPathTest()
        {
            IResult<int> result = Result.Fail<int>("Test");

            if (result.Success)
            {
                throw new Exception();
            }
            else
            {
                // Pass
            }
        }

        [Test]
        [RequiresPlayMode(false)]
        public void HappyPathByPatternMatchingTest()
        {
            IResult<int> result = Result.Succeed(1);

            if (result is ISuccessResult<int> success)
            {
                // Pass
                success.Should().NotBeNull();
                success.Result.Should().Be(1);
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
            IResult<int> result = Result.Fail<int>("Test");

            if (result is ISuccessResult<int>)
            {
                throw new Exception();
            }
            else if (result is IFailureResult<int> failure)
            {
                // Pass
                failure.Message.Should().Be("Test");
            }
            else
            {
                throw new ResultPatternMatchException(nameof(result));
            }
        }
    }
}