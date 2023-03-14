#nullable enable

using System;
using System.Net;
using FluentAssertions;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Mochineko.HttpResult.Tests
{
    [TestFixture]
    internal sealed class HttpResultWithNoResultTest
    {
        [Test]
        [RequiresPlayMode(false)]
        public void SuccessTest()
        {
            Success(HttpResult.Ok(HttpStatusCode.OK))
                .Should().BeTrue();
            
            Success(HttpResult.Retry(HttpStatusCode.ServiceUnavailable, new Exception()))
                .Should().BeFalse();
            
            Success(HttpResult.Fail(HttpStatusCode.BadRequest, new Exception()))
                .Should().BeFalse();
        }
        
        [Test]
        [RequiresPlayMode(false)]
        public void RetryTest()
        {
            Retryable(HttpResult.Ok(HttpStatusCode.OK))
                .Should().BeFalse();
            
            Retryable(HttpResult.Retry(HttpStatusCode.ServiceUnavailable, new Exception()))
                .Should().BeTrue();
            
            Retryable(HttpResult.Fail(HttpStatusCode.BadRequest, new Exception()))
                .Should().BeFalse();
        }
        
        [Test]
        [RequiresPlayMode(false)]
        public void FailureTest()
        {
            Failure(HttpResult.Ok(HttpStatusCode.OK))
                .Should().BeFalse();
            
            Failure(HttpResult.Retry(HttpStatusCode.ServiceUnavailable, new Exception()))
                .Should().BeFalse();
            
            Failure(HttpResult.Fail(HttpStatusCode.BadRequest, new Exception()))
                .Should().BeTrue();
        }
        
        private bool Success(IHttpResult result)
        {
            return result switch
            {
                IHttpSuccessResult success => true,
                IHttpRetryableResult<Exception> retryable => false,
                IHttpFailureResult<Exception> failure => false,
                _ => throw new ArgumentOutOfRangeException(nameof(result))
            };
        }
        
        private bool Retryable(IHttpResult result)
        {
            return result switch
            {
                IHttpSuccessResult success => false,
                IHttpRetryableResult<Exception> retryable => true,
                IHttpFailureResult<Exception> failure => false,
                _ => throw new ArgumentOutOfRangeException(nameof(result))
            };
        }
        
        private bool Failure(IHttpResult result)
        {
            return result switch
            {
                IHttpSuccessResult success => false,
                IHttpRetryableResult<Exception> retryable => false,
                IHttpFailureResult<Exception> failure => true,
                _ => throw new ArgumentOutOfRangeException(nameof(result))
            };
        }
    }
}