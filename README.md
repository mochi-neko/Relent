# Relent

The Relent is a library that provides explicit error handling
 without relying on null or exceptions
 and resilience for uncertain operations in Unity/C#.

## Features

1. You don't need to rely on null and exceptions to handle errors.
2. You can distinguish expected errors and unexpected (fatal) errors.
3. You are forced to think handle failures.
4. It makes the code a lot easier to read.
5. You obtain resilience for uncertain operations e.g. HTTP communication.

## Modules

This library contains these modules:

- [Result](#result)
- [Uncertain Result](#uncertain-result)
- [Resilience](#resilience)

## Result

[Result](https://github.com/mochi-neko/Relent/tree/main/Assets/Mochineko/Relent/Result)
 is a simple and explicit error handling module.

### How to import by Unity Package Manager

Add this dependency to your `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.mochineko.relent.result": "https://github.com/mochi-neko/Relent.git?path=/Assets/Mochineko/Relent/Result#0.1.2",
    ...
  }
}
```

### Core specification

- [IResult](https://github.com/mochi-neko/Relent/blob/main/Assets/Mochineko/Relent/Result/IResult.cs)
- [ISuccessResult](https://github.com/mochi-neko/Relent/blob/main/Assets/Mochineko/Relent/Result/ISuccessResult.cs)
- [IFailureResult](https://github.com/mochi-neko/Relent/blob/main/Assets/Mochineko/Relent/Result/IFailureResult.cs)

### Usage

Let `MyOperation()` be your operation that can be failed
 with any exception,

```csharp
public MyObject MyOperation()
{
    // do something that can throw exception
    if (anyCondition)
    {
        return new MyObject();
    }
    else
    {
        throw new HandledException("Any handled exception");
    }   
}
```

you can wrap result by using `IResult<TResult>` like this:

```csharp
public IResult<MyObject> MyOperationByResult()
{
    try
    {
        var myObject = MyOperation();
        
        return ResultFactory.Succeed(myObject);
    }
    // Catch any exception what you want to handle.
    catch (HandledException exception)
    {
        return ResultFactory.Fail<MyObject>(
            $"Why did it fail? {exception.Message}");
    }
    catch (Exception exception)
    {
        // Panic! Unexpected exception what you don't want to handle.
        throw;
    }
}
```

then you can handle the result like this:

```csharp
public void MyMethod()
{
    IResult<MyObject> result = MyOperationByResult();
    
    if (result is ISuccessResult<MyObject> successResult)
    {
        // do something with successResult.Result
    }
    else if (result is IFailureResult failureResult)
    {
        // do something with failureResult.Message
    }
}
```

Once you have wrapped the result,
 you don't need to worry about null and exceptions
 expect for the fatal exception that you don't want to handle.

Of course, you can define your operation
 without null or any exception at first like this:

```csharp
public IResult<MyObject> MyOperation()
{
    if (anyCondition)
    {
        return ResultFactory.Succeed(myObject);
    }
    else
    {
        return ResultFactory.Fail<MyObject>(
            "Why did it fail?");
    }   
}
```

### Samples

- [Test codes](https://github.com/mochi-neko/Relent/tree/main/Assets/Mochineko/Relent/Result.Tests)
- [Sample](https://github.com/mochi-neko/Relent/blob/main/Assets/Mochineko/Relent/Result.Tests/ResultSample.cs)

## Uncertain Result

[UncertainResult](https://github.com/mochi-neko/Relent/tree/main/Assets/Mochineko/Relent/UncertainResult)
 is an explicit error handling module
 for an uncertain operation that can be retryable failure,
 e.g. [HTTP communication for a WebAPI](https://github.com/mochi-neko/Relent/blob/main/Assets/Mochineko/Relent/UncertainResult.Tests/MockWebAPI.cs).

### How to import by Unity Package Manager

```json
{
  "dependencies": {
    "com.mochineko.relent.uncertain-result": "https://github.com/mochi-neko/Relent.git?path=/Assets/Mochineko/Relent/UncertainResult#0.1.2",
    ...
  }
}
```

### Core specification

- [IUncertainResult](https://github.com/mochi-neko/Relent/blob/main/Assets/Mochineko/Relent/UncertainResult/IUncertainResult.cs)
- [IUncertainSuccessResult](https://github.com/mochi-neko/Relent/blob/main/Assets/Mochineko/Relent/UncertainResult/IUncertainSuccessResult.cs)
- [IUncertainFailureResult](https://github.com/mochi-neko/Relent/blob/main/Assets/Mochineko/Relent/UncertainResult/IUncertainFailureResult.cs)

### Usage

Let `MyOperation()` be your uncertain operation that can be failed
 and be retryable with any exception,

```csharp
public MyObject MyOperation()
{
    // do something that can throw exception
    if (anyCondition)
    {
        return new MyObject();
    }
    else if (anyOtherCondition)
    {
        throw new RetryableException("Any retryable exception");
    }
    else
    {
        throw new HandledException("Any exception");
    }   
}
```

you can wrap result by using `IUncertainResult<TResult>` like this:

```csharp
public IUncertainResult<MyObject> MyOperationByResult()
{
    try
    {
        var myObject = MyOperation();
        
        return ResultFactory.Succeed(myObject);
    }
    // Catch any exception what you want to handle as retryable.
    catch (RetryableException exception)
    {
        return ResultFactory.Retry<MyObject>(
            $"Why did it fail? {exception.Message}");
    }
    // Catch any exception what you want to handle as failure.
    catch (HandledException exception)
    {
        return ResultFactory.Fail<MyObject>(
            $"Why did it fail? {exception.Message}");
    }
    catch (Exception exception)
    {
        // Panic! Unexpected exception what you don't want to handle.
        throw;
    }
}
```

then you can handle the result like this:

```csharp
public void MyMethod()
{
    IUncertaionResult<MyObject> result = MyOperationByResult();
    
    if (result is IUncertaionSuccessResult<MyObject> successResult)
    {
        // do something with successResult.Result
    }
    else if (result is IUncertaionRetryResult retryableResult)
    {
        // do something with retryResult.Message
        // can retry operation
    }
    else if (result is IUncertaionFailureResult failureResult)
    {
        // do something with failureResult.Message
    }
}
```

You can retry operation
 when the result can cast `IUncertainRetryResult`.

Also you can use `switch` syntax like this:

```csharp
public void MyMethod()
{
    IUncertaionResult<MyObject> result = MyOperationByResult();
    
    switch (result)
    {
        case IUncertaionSuccessResult<MyObject> successResult:
            // do something with successResult.Result
            break;
        case IUncertaionRetryResult retryableResult:
            // do something with retryResult.Message
            // can retry operation
            break;
        case IUncertaionFailureResult failureResult:
            // do something with failureResult.Message
            break;
    }
}
```

Once you have wrapped the result,
 you don't need to worry about null and exceptions
 expect for the fatal exception that you don't want to handle.

Of course, you can define your operation
 without null or any exception at first like this:

```csharp
public IResult<MyObject> MyOperation()
{
    if (anyCondition)
    {
        return ResultFactory.Succeed(myObject);
    }
    else if (anyOtherCondition)
    {
        return ResultFactory.Retry<MyObject>(
            "Why did it fail?");
    }
    else
    {
        return ResultFactory.Fail<MyObject>(
            "Why did it fail?");
    }   
}
```

### Samples

- [Test codes](https://github.com/mochi-neko/Relent/tree/main/Assets/Mochineko/Relent/UncertainResult.Tests)
- [Sample with HttpClient](https://github.com/mochi-neko/Relent/blob/main/Assets/Mochineko/Relent/UncertainResult.Tests/HttpClientTest.cs)
  - [A sample of a WebAPI communication implementation](https://github.com/mochi-neko/Relent/blob/main/Assets/Mochineko/Relent/UncertainResult.Tests/MockWebAPI.cs) 
- [Sample with a result what you define](https://github.com/mochi-neko/Relent/blob/main/Assets/Mochineko/Relent/UncertainResult.Tests/UserDefinedUncertainResultSample.cs)

## Resilience

[Resilience](https://github.com/mochi-neko/Relent/tree/main/Assets/Mochineko/Relent/Resilience)
 is a module that provides resilience for an uncertain operation
 caused by unpredictable factors,
 e.g. [HTTP communication for a WebAPI](https://github.com/mochi-neko/Relent/blob/main/Assets/Mochineko/Relent/Resilience.Tests/HttpClientResilienceSample.cs).

It depends on [UncertainResult](#uncertain-result).

### Why don't I use [Polly](https://github.com/App-vNext/Polly)?

The timeout of Polly relies on `OperationCanceledException`
 thrown by linked `CancellationToken`
 (user cancellation token and timeout cancellation token)
 in an operation.

When you use [UncertainResult](#uncertain-result),
 we want to catch `OperationCanceledException`
 as retryable result,
 then we cannot cancel by timeout.

### How to import by Unity Package Manager

```json
{
  "dependencies": {
    "com.mochineko.relent.resilience": "https://github.com/mochi-neko/Relent.git?path=/Assets/Mochineko/Relent/Resilience#0.1.2",
    "com.mochineko.relent.uncertain-result": "https://github.com/mochi-neko/Relent.git?path=/Assets/Mochineko/Relent/UncertainResult#0.1.2",
    ...
  }
}
```

### Core specification

- [IPolicy](https://github.com/mochi-neko/Relent/blob/main/Assets/Mochineko/Relent/Resilience/IPolicy.cs)

### Features

- [Retry](https://github.com/mochi-neko/Relent/blob/main/Assets/Mochineko/Relent/Resilience/Retry)
- [Timeout](https://github.com/mochi-neko/Relent/blob/main/Assets/Mochineko/Relent/Resilience/Timeout)
- [Circuit Breaker](https://github.com/mochi-neko/Relent/blob/main/Assets/Mochineko/Relent/Resilience/CircuitBreaker)
- [Bulkhead](https://github.com/mochi-neko/Relent/blob/main/Assets/Mochineko/Relent/Resilience/Bulkhead)
- [Wrap](https://github.com/mochi-neko/Relent/tree/main/Assets/Mochineko/Relent/Resilience/Wrap)

### Usage

Use some policies what you want to use.

#### Retry

The retry policy is a policy that retries an operation
 when the operation returns an uncertain result
 that can cast `IUncertainRetryResult`.

See [test codes](https://github.com/mochi-neko/Relent/blob/main/Assets/Mochineko/Relent/Resilience.Tests/RetryTest.cs).

#### Timeout

The timeout policy is a policy
 that cancels an operation
 when the operation takes too long.

See [test codes](https://github.com/mochi-neko/Relent/blob/main/Assets/Mochineko/Relent/Resilience.Tests/TimeoutTest.cs).

#### Circuit Breaker

The circuit breaker policy is a policy
 that breaks an operation
 when the operation returns continuous uncertain results
 that can cast `IUncertainRetryResult`.

See [test codes](https://github.com/mochi-neko/Relent/blob/main/Assets/Mochineko/Relent/Resilience.Tests/CircuitBreakerTest.cs).

#### Bulkhead

The bulkhead policy is a policy
 that limits the number of operations
 that can be executed at the same time.

See [test codes](https://github.com/mochi-neko/Relent/blob/main/Assets/Mochineko/Relent/Resilience.Tests/BulkheadTest.cs).

#### Wrap

The wrap policy is a policy
 that can combine some policies.

See [test codes](https://github.com/mochi-neko/Relent/blob/main/Assets/Mochineko/Relent/Resilience.Tests/WrapTest.cs).

### Samples

- [Test codes](https://github.com/mochi-neko/Relent/tree/main/Assets/Mochineko/Relent/Resilience.Tests)
- [Sample with HttpClient](https://github.com/mochi-neko/Relent/blob/main/Assets/Mochineko/Relent/Resilience.Tests/HttpClientResilienceSample.cs)

### Extensions

- for [Newtonsoft.Json](https://github.com/mochi-neko/Relent/tree/main/Assets/Mochineko/Relent.Extensions/Newtonsoft.Json)

## Acknowledgments

This library is inspired by there posts and libraries:

- [HttpClient - Error handling, a test driven approach](https://josef.codes/httpclient-error-handling-a-test-driven-approach/)
- [Functional C#: Handling failures, input errors](https://enterprisecraftsmanship.com/posts/functional-c-handling-failures-input-errors/)
- [Polly](https://github.com/App-vNext/Polly)
- [Result in Rust](https://doc.rust-lang.org/std/result/enum.Result.html)
- [anyhow::Result in Rust](https://docs.rs/anyhow/latest/anyhow/type.Result.html)

## Changelog

See [CHANGELOG](https://github.com/mochi-neko/ChatGPT-API-unity/blob/main/CHANGELOG.md).

## 3rd Party Notices

See [NOTICE](https://github.com/mochi-neko/ChatGPT-API-unity/blob/main/NOTICE.md).

## License

[MIT License](https://github.com/mochi-neko/ChatGPT-API-unity/blob/main/LICENSE)
