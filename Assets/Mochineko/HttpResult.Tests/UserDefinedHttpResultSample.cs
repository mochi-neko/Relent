#nullable enable
using System;
using FluentAssertions;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Mochineko.HttpResult.Tests
{
    [TestFixture]
    internal sealed class UserDefinedHttpResultSample
    {
        [Test]
        [RequiresPlayMode(false)]
        public void Sample()
        {
            IHttpResult<string> result = new MyHttpFailureResult<string>("Test", "000");

            result.Success.Should().BeFalse();
            result.Retryable.Should().BeFalse();
            result.Failure.Should().BeTrue();

            if (result is IHttpSuccessResult<string>)
            {
                throw new Exception();
            }
            else if (result is IHttpRetryableResult<string>)
            {
                throw new Exception();
            }
            else if (result is MyHttpFailureResult<string> myFailure)
            {
                myFailure.Message.Should().Be("Test");
                myFailure.ErrorCode.Should().Be("000");
            }
            else
            {
                throw new HttpResultPatternMatchException(nameof(result));
            }
        }
        
        internal sealed class MyHttpFailureResult<TResult>
            : IHttpFailureResult<TResult>
        {
            public bool Success => false;
            public bool Retryable => false;
            public bool Failure => true;
            public string Message { get; }
            public string ErrorCode { get; }

            public MyHttpFailureResult(string message, string errorCode)
            {
                Message = message;
                ErrorCode = errorCode;
            }
        }
    }
}