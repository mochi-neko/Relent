#nullable enable
using System;

namespace Mochineko.Relent.Result
{
    public static class ResultExtensions
    {
        public static TResult Unwrap<TResult>(this IResult<TResult> result)
        {
            if (result is ISuccessResult<TResult> success)
            {
                return success.Result;
            }
            else
            {
                throw new InvalidOperationException("Failed to unwrap result.");
            }
        }

        public static string ExtractMessage(this IResult result)
        {
            if (result is IFailureResult failure)
            {
                return failure.Message;
            }
            else
            {
                throw new InvalidOperationException("Failed to extract message from failure result.");
            }
        }

        public static string ExtractMessage<TResult>(this IResult<TResult> result)
        {
            if (result is IFailureResult<TResult> failure)
            {
                return failure.Message;
            }
            else
            {
                throw new InvalidOperationException("Failed to extract message from failure result.");
            }
        }

        public static IResult<TResult> ToResult<TResult>(this TResult result)
            => ResultFactory.Succeed(result);

        public static IResult Try<TException>(
            Action operation,
            Action? finalizer = null)
            where TException : Exception
        {
            try
            {
                operation.Invoke();
                return ResultFactory.Succeed();
            }
            catch (TException exception)
            {
                return ResultFactory.Fail(
                    $"Failed to execute operation because of {exception}.");
            }
            finally
            {
                finalizer?.Invoke();
            }
        }

        public static IResult Try<TException, TException2>(
            Action operation,
            Action? finalizer = null)
            where TException : Exception
            where TException2 : Exception
        {
            try
            {
                operation.Invoke();
                return ResultFactory.Succeed();
            }
            catch (TException exception)
            {
                return ResultFactory.Fail(
                    $"Failed to execute operation because of {exception}.");
            }
            catch (TException2 exception)
            {
                return ResultFactory.Fail(
                    $"Failed to execute operation because of {exception}.");
            }
            finally
            {
                finalizer?.Invoke();
            }
        }

        public static IResult Try<TException, TException2, TException3>(
            Action operation,
            Action? finalizer = null)
            where TException : Exception
            where TException2 : Exception
            where TException3 : Exception
        {
            try
            {
                operation.Invoke();
                return ResultFactory.Succeed();
            }
            catch (TException exception)
            {
                return ResultFactory.Fail(
                    $"Failed to execute operation because of {exception}.");
            }
            catch (TException2 exception)
            {
                return ResultFactory.Fail(
                    $"Failed to execute operation because of {exception}.");
            }
            catch (TException3 exception)
            {
                return ResultFactory.Fail(
                    $"Failed to execute operation because of {exception}.");
            }
            finally
            {
                finalizer?.Invoke();
            }
        }

        public static IResult<TResult> Try<TResult, TException>(
            Func<TResult> operation,
            Action? finalizer = null)
            where TException : Exception
        {
            try
            {
                var result = operation.Invoke();
                return ResultFactory.Succeed(result);
            }
            catch (TException exception)
            {
                return ResultFactory.Fail<TResult>(
                    $"Failed to execute operation because of {exception}.");
            }
            finally
            {
                finalizer?.Invoke();
            }
        }

        public static IResult<TResult> Try<TResult, TException, TException2>(
            Func<TResult> operation,
            Action? finalizer = null)
            where TException : Exception
            where TException2 : Exception
        {
            try
            {
                var result = operation.Invoke();
                return ResultFactory.Succeed(result);
            }
            catch (TException exception)
            {
                return ResultFactory.Fail<TResult>(
                    $"Failed to execute operation because of {exception}.");
            }
            catch (TException2 exception)
            {
                return ResultFactory.Fail<TResult>(
                    $"Failed to execute operation because of {exception}.");
            }
            finally
            {
                finalizer?.Invoke();
            }
        }

        public static IResult<TResult> Try<TResult, TException, TException2, TException3>(
            Func<TResult> operation,
            Action? finalizer = null)
            where TException : Exception
            where TException2 : Exception
            where TException3 : Exception
        {
            try
            {
                var result = operation.Invoke();
                return ResultFactory.Succeed(result);
            }
            catch (TException exception)
            {
                return ResultFactory.Fail<TResult>(
                    $"Failed to execute operation because of {exception}.");
            }
            catch (TException2 exception)
            {
                return ResultFactory.Fail<TResult>(
                    $"Failed to execute operation because of {exception}.");
            }
            catch (TException3 exception)
            {
                return ResultFactory.Fail<TResult>(
                    $"Failed to execute operation because of {exception}.");
            }
            finally
            {
                finalizer?.Invoke();
            }
        }


        public static IFailureTraceResult FailWithTrace(
            string message)
            => new FailureTraceResult(message);

        public static IFailureTraceResult<TResult> FailWithTrace<TResult>(
            string message)
            => new FailureTraceResult<TResult>(message);

        public static IFailureTraceResult Trace(
            this IFailureTraceResult result,
            string message)
        {
            result.AddTrace(message);
            return result;
        }

        public static IFailureTraceResult<TResult> Trace<TResult>(
            this IFailureTraceResult<TResult> result,
            string message)
        {
            result.AddTrace(message);
            return result;
        }
    }
}