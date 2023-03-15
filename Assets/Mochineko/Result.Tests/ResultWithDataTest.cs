#nullable enable
using System;
using FluentAssertions;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Mochineko.Result.Tests
{
    [TestFixture]
    internal sealed class ResultWithDataTest
    {
        [Test]
        [RequiresPlayMode(false)]
        public void SuccessTest()
        {
            var result = Result.Ok(true);

            result.Success.Should().BeTrue();
            result.Failure.Should().BeFalse();
        }

        [Test]
        [RequiresPlayMode(false)]
        public void FailureTest()
        {
            var result = Result.Fail<bool>("Test");

            result.Success.Should().BeFalse();
            result.Failure.Should().BeTrue();
        }
        
        [Test]
        [RequiresPlayMode(false)]
        public void HappyPathTest()
        {
            var result = Result.Ok(true);

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
            var result = Result.Fail<bool>("Test");

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
            var result = Result.Ok(true);

            if (result is ISuccessResult<bool> success)
            {
                // Pass
                success.Should().NotBeNull();
                success.Result.Should().BeTrue();
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
            var result = Result.Fail<bool>("Test");

            if (result is ISuccessResult<bool>)
            {
                throw new Exception();
            }
            else if (result is IFailureResult<bool> failure)
            {
                // Pass
                failure.Message.Should().Be("Test");
            }
            else
            {
                throw new Exception();
            }
        }
        
        [Test]
        [RequiresPlayMode(false)]
        public void UnwrapSuccessResultTest()
        {
            var result = Result.Ok(true);

            result.Unwrap().Should().BeTrue();
        }
        
        [Test]
        [RequiresPlayMode(false)]
        public void FailedToUnwrapFailureResultTest()
        {
            var result = Result.Fail<bool>("Test");
            Func<bool> unwrap = () => result.Unwrap();

            unwrap.Should().Throw<FailedToUnwrapResultException>();
        }
    }
}