#nullable enable
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Mochineko.Relent.UncertainResult.Tests
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
            result.Success.Should().BeTrue();
            result.Unwrap().Should().Be("Hello, world!");
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
            result.Retryable.Should().BeTrue();
            result.ExtractMessage().Should()
                .StartWith(
                    "Retryable because operation was cancelled during calling the API:System.Threading.Tasks.TaskCanceledException: A task was canceled.");
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
            result.Retryable.Should().BeTrue();
            result.ExtractMessage().Should()
                .StartWith("Retryable because operation was cancelled before calling the API.");
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
            result.Retryable.Should().BeTrue();
            result.ExtractMessage().Should()
                .StartWith($"Retryable because the API returned status code:({(int)statusCode}){statusCode}.");
        }

        [Test]
        [RequiresPlayMode(false)]
        public async Task RetryableByHttpRequestExceptionTest()
        {
            using var httpClient = new HttpClient(
                new MockedHttpMessageHandler(cancellationToken
                    => throw new HttpRequestException()));

            var result = await MockWebAPI.GetAsync(httpClient, DummyUrl, CancellationToken.None);
            result.Retryable.Should().BeTrue();
            result.ExtractMessage().Should()
                .StartWith("Retryable because HttpRequestException was thrown during calling the API:");
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
            result.Failure.Should().BeTrue();
            result.ExtractMessage().Should()
                .StartWith($"Failed because the API returned status code:({(int)statusCode}){statusCode}.");
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
            result.Failure.Should().BeTrue();
            result.ExtractMessage().Should()
                .StartWith($"Failed because an unhandled exception was thrown when calling the API:");
        }
    }
}