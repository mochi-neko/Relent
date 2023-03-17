#nullable enable
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Mochineko.HttpResult.Tests
{
    [TestFixture]
    internal sealed class HttpClientTest
    {
        private const string DummyUrl = "http://localhost:8080";

        [Test]
        [RequiresPlayMode(false)]
        public async Task SuccessTest()
        {
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("Hello, world!")
            };
            using var httpClient = new HttpClient(
                new MockedHttpMessageHandler(responseMessage));

            var result = await MockWebAPI.GetAsync(httpClient, DummyUrl, CancellationToken.None);
            if (result is IHttpSuccessResult<string> success)
            {
                // Happy path
                success.Result.Should().Be("Hello, world!");
            }
            else if (result is IHttpRetryableResult<string> retryable)
            {
                throw new Exception();
            }
            else if (result is IHttpFailureResult<string> failure)
            {
                throw new Exception();
            }
            else
            {
                // Unexpected
                throw new HttpResultPatternMatchException(nameof(result));
            }
        }

        [Test]
        [RequiresPlayMode(false)]
        public async Task RetryableByUserTimeoutCancellationTest()
        {
            using var httpClient = new HttpClient(
                new MockedHttpMessageHandler(async cancellationToken =>
                {
                    await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);

                    throw new InvalidOperationException();
                }));

            using var timeoutCancellationTokenSource = new CancellationTokenSource();
            timeoutCancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(0.1d));

            var result = await MockWebAPI.GetAsync(httpClient, DummyUrl, timeoutCancellationTokenSource.Token);
            if (result is IHttpSuccessResult<string> success)
            {
                throw new Exception();
            }
            else if (result is IHttpRetryableResult<string> retryable)
            {
                retryable.Message.Should()
                    .StartWith(
                        "Retryable because operation was cancelled during calling the API:System.Threading.Tasks.TaskCanceledException: A task was canceled.");
            }
            else if (result is IHttpFailureResult<string> failure)
            {
                throw new Exception();
            }
            else
            {
                // Unexpected
                throw new HttpResultPatternMatchException(nameof(result));
            }
        }

        [Test]
        [RequiresPlayMode(false)]
        public async Task RetryableByAlreadyCancelledTest()
        {
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("Hello, world!")
            };
            using var httpClient = new HttpClient(
                new MockedHttpMessageHandler(async cancellationToken =>
                {
                    await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken);

                    return responseMessage ?? throw new InvalidOperationException();
                }));

            using var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            var result = await MockWebAPI.GetAsync(httpClient, DummyUrl, cancellationTokenSource.Token);
            if (result is IHttpSuccessResult<string> success)
            {
                throw new Exception();
            }
            else if (result is IHttpRetryableResult<string> retryable)
            {
                retryable.Message.Should()
                    .StartWith("Retryable because operation was cancelled before calling the API.");
            }
            else if (result is IHttpFailureResult<string> failure)
            {
                throw new Exception();
            }
            else
            {
                // Unexpected
                throw new HttpResultPatternMatchException(nameof(result));
            }
        }

        [TestCase(HttpStatusCode.TooManyRequests)]
        [TestCase(HttpStatusCode.InternalServerError)]
        [TestCase(HttpStatusCode.BadGateway)]
        [TestCase(HttpStatusCode.ServiceUnavailable)]
        [RequiresPlayMode(false)]
        public async Task RetryableByStatusCodeTest(HttpStatusCode statusCode)
        {
            var responseMessage = new HttpResponseMessage(statusCode)
            {
                Content = new StringContent("You can retry this request.")
            };
            using var httpClient = new HttpClient(new MockedHttpMessageHandler(responseMessage));

            var result = await MockWebAPI.GetAsync(httpClient, DummyUrl, CancellationToken.None);
            if (result is IHttpSuccessResult<string> success)
            {
                throw new Exception();
            }
            else if (result is IHttpRetryableResult<string> retryable)
            {
                retryable.Message.Should()
                    .Be($"Retryable because the API returned status code:({(int)statusCode}){statusCode}.");
            }
            else if (result is IHttpFailureResult<string> failure)
            {
                throw new Exception();
            }
            else
            {
                // Unexpected
                throw new HttpResultPatternMatchException(nameof(result));
            }
        }

        [Test]
        [RequiresPlayMode(false)]
        public async Task RetryableByHttpRequestExceptionTest()
        {
            using var httpClient = new HttpClient(
                new MockedHttpMessageHandler(cancellationToken
                    => throw new HttpRequestException()));

            var result = await MockWebAPI.GetAsync(httpClient, DummyUrl, CancellationToken.None);
            if (result is IHttpSuccessResult<string> success)
            {
                throw new Exception();
            }
            else if (result is IHttpRetryableResult<string> retryable)
            {
                retryable.Message.Should().StartWith(
                    "Retryable because HttpRequestException was thrown during calling the API:");
            }
            else if (result is IHttpFailureResult<string> failure)
            {
                throw new Exception();
            }
            else
            {
                // Unexpected
                throw new HttpResultPatternMatchException(nameof(result));
            }
        }

        [TestCase(HttpStatusCode.BadRequest)]
        [TestCase(HttpStatusCode.Unauthorized)]
        [TestCase(HttpStatusCode.Forbidden)]
        [TestCase(HttpStatusCode.NotFound)]
        [RequiresPlayMode(false)]
        public async Task FailureByStatusCodeTest(HttpStatusCode statusCode)
        {
            var responseMessage = new HttpResponseMessage(statusCode)
            {
                Content = new StringContent("You can retry this request.")
            };
            using var httpClient = new HttpClient(new MockedHttpMessageHandler(responseMessage));

            var result = await MockWebAPI.GetAsync(httpClient, DummyUrl, CancellationToken.None);
            if (result is IHttpSuccessResult<string> success)
            {
                throw new Exception();
            }
            else if (result is IHttpRetryableResult<string> retryable)
            {
                throw new Exception();
            }
            else if (result is IHttpFailureResult<string> failure)
            {
                failure.Message.Should()
                    .Be($"Failed because the API returned status code:({(int)statusCode}){statusCode}.");
            }
            else
            {
                // Unexpected
                throw new HttpResultPatternMatchException(nameof(result));
            }
        }

        [Test]
        [RequiresPlayMode(false)]
        public async Task FailureByUnhandledExceptionTest()
        {
            LogAssert.ignoreFailingMessages = true;

            using var httpClient = new HttpClient(
                new MockedHttpMessageHandler(cancellationToken
                    => throw new Exception()));

            var result = await MockWebAPI.GetAsync(httpClient, DummyUrl, CancellationToken.None);
            if (result is IHttpSuccessResult<string> success)
            {
                throw new Exception();
            }
            else if (result is IHttpRetryableResult<string> retryable)
            {
                throw new Exception();
            }
            else if (result is IHttpFailureResult<string> failure)
            {
                failure.Message.Should()
                    .StartWith($"Failed because an unhandled exception was thrown when calling the API:");
            }
            else
            {
                // Unexpected
                throw new HttpResultPatternMatchException(nameof(result));
            }
        }
    }
}