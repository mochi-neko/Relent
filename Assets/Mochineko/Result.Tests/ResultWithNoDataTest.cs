#nullable enable
using System;
using FluentAssertions;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Mochineko.Result.Tests
{
    [TestFixture]
    internal sealed class ResultWithNoDataTest
    {
        [Test]
        [RequiresPlayMode(false)]
        public void SuccessTest()
        {
            var result = Result.Ok();

            result.Success.Should().BeTrue();
            result.Failure.Should().BeFalse();
        }

        [Test]
        [RequiresPlayMode(false)]
        public void FailureTest()
        {
            var result = Result.Fail("Test");

            result.Success.Should().BeFalse();
            result.Failure.Should().BeTrue();
        }
        
        [Test]
        [RequiresPlayMode(false)]
        public void HappyPathTest()
        {
            var result = Result.Ok();

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
            var result = Result.Fail("Test");

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
            var result = Result.Ok();

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
            var result = Result.Fail("Test");

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