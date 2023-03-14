#nullable enable
using System;
using System.Net;
using FluentAssertions;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Mochineko.HttpResult.Tests
{
    [TestFixture]
    internal sealed class HttpResultWithResultTest
    {
        [Test]
        [RequiresPlayMode(false)]
        public void SuccessTest()
        {
            IHttpResult<int> result = HttpResult.Ok(HttpStatusCode.OK, 1);

            Success(result).Should().Be(1);
        }
        
        private int Success(IHttpResult<int> result)
        {
            return result switch
            {
                IHttpSuccessResult<int> success => success.Result,
                IHttpRetryableResult<int, Exception> retryable => default,
                IHttpFailureResult<int, Exception> failure => default,
                _ => throw new ArgumentOutOfRangeException(nameof(result))
            };
        }
        
        private int Retryable(IHttpResult<int> result)
        {
            return result switch
            {
                IHttpSuccessResult<int> success => default,
                IHttpRetryableResult<int, Exception> retryable => -1,
                IHttpFailureResult<int, Exception> failure => default,
                _ => throw new ArgumentOutOfRangeException(nameof(result))
            };
        }
        
        private int Failure(IHttpResult<int> result)
        {
            return result switch
            {
                IHttpSuccessResult<int> success => default,
                IHttpRetryableResult<int, Exception> retryable => default,
                IHttpFailureResult<int, Exception> failure => -1,
                _ => throw new ArgumentOutOfRangeException(nameof(result))
            };
        }
    }
}