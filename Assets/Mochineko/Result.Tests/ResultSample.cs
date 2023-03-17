using System;
using FluentAssertions;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Mochineko.Result.Tests
{
    [TestFixture]
    internal class ResultSample
    {
        [Test]
        [RequiresPlayMode(false)]
        public void Sample()
        {
            var result = ReturnHalfNumberWhenInputIsEven(1);
            result.Success.Should().BeFalse();
            result.Failure.Should().BeTrue();

            if (result is ISuccessResult<int> _)
            {
                throw new Exception();
            }
            else if (result is IFailureResult<int> failure)
            {
                // Pass
                failure.Message.Should().Be("1 is not even.");
            }
            else
            {
                throw new Exception();
            }

            result = ReturnHalfNumberWhenInputIsEven(6);
            result.Success.Should().BeTrue();
            result.Failure.Should().BeFalse();

            if (result is ISuccessResult<int> success)
            {
                // Pass
                success.Result.Should().Be(3);
            }
            else if (result is IFailureResult<int> _)
            {
                throw new Exception();
            }
            else
            {
                throw new Exception();
            }
        }
        
        private IResult<int> ReturnHalfNumberWhenInputIsEven(int integer)
        {
            if (integer % 2 == 0)
            {
                return Result.Succeed<int>(integer / 2);
            }
            else
            {
                return Result.Fail<int>($"{integer} is not even.");
            }
        }
    }
}
