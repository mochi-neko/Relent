#nullable enable
using System;
using FluentAssertions;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Mochineko.Relent.Result.Tests
{
    [TestFixture]
    internal sealed class ResultTestWithNoValue
    {
        [Test]
        [RequiresPlayMode(false)]
        public void SuccessTest()
        {
            IResult result = Results.Succeed();

            result.Success.Should().BeTrue();
            result.Failure.Should().BeFalse();
        }

        [Test]
        [RequiresPlayMode(false)]
        public void FailureTest()
        {
            IResult result = Results.Fail("Test");

            result.Success.Should().BeFalse();
            result.Failure.Should().BeTrue();
        }
        
        [Test]
        [RequiresPlayMode(false)]
        public void HappyPathTest()
        {
            IResult result = Results.Succeed();

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
            IResult result = Results.Fail("Test");

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
            IResult result = Results.Succeed();

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
            IResult result = Results.Fail("Test");

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
                throw new ResultPatternMatchException(nameof(result));
            }
        }
        
        [Test]
        [RequiresPlayMode(false)]
        public void TraceFailureShouldStackMessages()
        {
            var result1 = Results.FailWithTrace("message1.");
            var result2 = result1.Trace("message2.");
            var result3 = result2.Trace("message3.");
            var result4 = result3.Trace("message4.");

            result4.ExtractMessage()
                .Should().Be("message1.\n" +
                             "message2.\n" +
                             "message3.\n" +
                             "message4.\n");
        }
    }
}