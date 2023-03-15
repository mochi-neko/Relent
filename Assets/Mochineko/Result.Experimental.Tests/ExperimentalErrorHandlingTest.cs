#nullable enable
using System;
using FluentAssertions;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Mochineko.Result.Experimental.Tests
{
    [TestFixture]
    internal sealed class ExperimentalErrorHandlingTest
    {
        [Test]
        [RequiresPlayMode(false)]
        public void FailureWithErrorTest()
        {
            var result = ExperimentalResult.Fail("Test", new NullReferenceException());

            result.Success.Should().BeFalse();
            result.Failure.Should().BeTrue();
            result.FailureBy<ArgumentNullException>().Failure.Should().BeTrue();
            result.FailureBy<ArgumentOutOfRangeException>().Failure.Should().BeTrue();
            result.FailureBy<NullReferenceException>().Success.Should().BeTrue();
            result.FailureBy<Exception>().Success.Should().BeTrue();
        }
        
        [Test]
        [RequiresPlayMode(false)]
        public void ExceptionPathWithErrorTest()
        {
            var result = ExperimentalResult.Fail("Test", new ArgumentNullException());

            if (result.Success)
            {
                throw new Exception();
            }
            else if (result.FailureBy<InvalidOperationException>().Success)
            {
                throw new Exception();
            }
            else if (result.FailureBy<ArgumentNullException>().Success)
            {
                // Pass
            }
            else if (result.FailureBy<Exception>().Success)
            {
                throw new Exception();
            }
            else
            {
                throw new Exception();
            }
        }
        
        [Test]
        [RequiresPlayMode(false)]
        public void ExceptionPathByPatternMatchingWithErrorTest()
        {
            var result = ExperimentalResult.Fail("Test", new OperationCanceledException());

            if (result is ISuccessResult)
            {
                throw new Exception();
            }
            else if (result is IFailureResultWithError<InvalidCastException>)
            {
                throw new Exception();
            }
            else if (result is IFailureResultWithError<OperationCanceledException> failure)
            {
                // Pass
                failure.Message.Should().Be("Test");
                failure.Error.GetType().Should().Be(typeof(OperationCanceledException));
            }
            else
            {
                throw new Exception();
            }
        }
        
        [Test]
        [RequiresPlayMode(false)]
        public void CanExcludeInternalChainFailureTest()
        {
            var result = AFunctionCanReturnFailure();

            if (result is ISuccessResult)
            {
                throw new Exception();
            }
            else if (result is IChainedFailureResult chainedFailure)
            {
                chainedFailure.Message.Should().Be("Chained failure");
                chainedFailure.Inner.Message.Should().Be("Internal failure");

                if (chainedFailure.Inner is IFailureResultWithError<NotImplementedException> internalFailure)
                {
                    internalFailure.Error.GetType().Should().Be(typeof(NotImplementedException));
                }
                else
                {
                    throw new Exception();
                }
            }
            else
            {
                throw new Exception();
            }
        }
        
        [Test]
        [RequiresPlayMode(false)]
        public void ExceptionPathWithChainedTest()
        {
            var result = AFunctionCanReturnFailure();

            if (result.Success)
            {
                throw new Exception();
            }
            else if (result.ChainedFailure().Success)
            {
                // Pass
            }
            else
            {
                throw new Exception();
            }
        }
        
        private IResult AFunctionCanReturnFailure()
        {
            var internalResult = AnInternalFunctionCanReturnFailure();

            if (internalResult.Success)
            {
                throw new Exception();
            }
            else if (internalResult is IFailureResultWithError<NotImplementedException> internalFailure)
            {
                return ExperimentalResult.ChainFail(
                    message: "Chained failure",
                    inner: internalFailure);
            }
            else
            {
                throw new Exception();
            }
        }

        private IResult AnInternalFunctionCanReturnFailure()
        {
            return ExperimentalResult.Fail("Internal failure", new NotImplementedException());
        }
    }
}