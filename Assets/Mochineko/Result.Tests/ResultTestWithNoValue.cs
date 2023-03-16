#nullable enable
using System;
using FluentAssertions;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Mochineko.Result.Tests
{
    [TestFixture]
    internal sealed class ResultTestWithNoValue
    {
        [Test]
        [RequiresPlayMode(false)]
        public void SuccessTest()
        {
            IResult result = Result.Succeed();

            result.Success.Should().BeTrue();
            result.Failure.Should().BeFalse();
        }

        [Test]
        [RequiresPlayMode(false)]
        public void FailureTest()
        {
            IResult result = Result.Fail("Test");

            result.Success.Should().BeFalse();
            result.Failure.Should().BeTrue();
        }
        
        [Test]
        [RequiresPlayMode(false)]
        public void HappyPathTest()
        {
            IResult result = Result.Succeed();

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
            IResult result = Result.Fail("Test");

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
            IResult result = Result.Succeed();

            if (result is ISuccessResult success)
            {
                // Pass
                success.Should().NotBeNull();
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
            IResult result = Result.Fail("Test");

            if (result is ISuccessResult)
            {
                throw new Exception();
            }
            else if (result is IFailureResult failure)
            {
                // Pass
                failure.Message.Should().Be("Test");
            }
            else
            {
                throw new Exception();
            }
        }
    }
}