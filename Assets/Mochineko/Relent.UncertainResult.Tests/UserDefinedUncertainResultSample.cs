#nullable enable
using System;
using FluentAssertions;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Mochineko.Relent.UncertainResult.Tests
{
    [TestFixture]
    internal sealed class UserDefinedUncertainResultSample
    {
        [Test]
        [RequiresPlayMode(false)]
        public void Sample()
        {
            IUncertainResult<string> result = new MyUncertainFailureResult<string>("Test", "000");

            result.Success.Should().BeFalse();
            result.Retryable.Should().BeFalse();
            result.Failure.Should().BeTrue();

            if (result is IUncertainSuccessResult<string>)
            {
                throw new Exception();
            }
            else if (result is IUncertainRetryableResult<string>)
            {
                throw new Exception();
            }
            else if (result is MyUncertainFailureResult<string> myFailure)
            {
                myFailure.Message.Should().Be("Test");
                myFailure.ErrorCode.Should().Be("000");
            }
            else
            {
                throw new UncertainResultPatternMatchException(nameof(result));
            }
        }
        
        internal sealed class MyUncertainFailureResult<TResult>
            : IUncertainFailureResult<TResult>
        {
            public bool Success => false;
            public bool Retryable => false;
            public bool Failure => true;
            public string Message { get; }
            public string ErrorCode { get; }

            public MyUncertainFailureResult(string message, string errorCode)
            {
                Message = message;
                ErrorCode = errorCode;
            }
        }
    }
}