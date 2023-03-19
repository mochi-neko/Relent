# Relent

The Relent is a library that provides explicit error handling without relying on null or exceptions and resilience for uncertain operations in Unity/C#.

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
- [Resilience](#uncertain-result)

## Result

[Result](https://github.com/mochi-neko/Relent/tree/main/Assets/Mochineko/Relent/Result)
 is a simple and explicit error handling module.

### Core specification

- [IResult](https://github.com/mochi-neko/Relent/blob/main/Assets/Mochineko/Relent/Result/IResult.cs)
- [ISuccessResult](https://github.com/mochi-neko/Relent/blob/main/Assets/Mochineko/Relent/Result/ISuccessResult.cs)
- [IFailureResult](https://github.com/mochi-neko/Relent/blob/main/Assets/Mochineko/Relent/Result/IFailureResult.cs)

### Usage

Let `MyOperation()` be your operation that can be failed with any exception,

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
        throw new Exception("Any exception");
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

Of course, you can define your operation without null or any exception at first like this:

```csharp
public IResult<MyObject> MyOperation()
{
    if (anyCondition)
    {
        return ResultFactory.Succeed(myObject);;
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
- [Sample codes](https://github.com/mochi-neko/Relent/blob/main/Assets/Mochineko/Relent/Result.Tests/ResultSample.cs)

### How to import by Unity Package Manager

Add this dependency to your `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.mochineko.relent.result": "https://github.com/mochi-neko/Relent.git?path=/Assets/Mochineko/Relent/Result#0.1.0",
    ...
  }
}
```

## Uncertain Result

[UncertainResult](https://github.com/mochi-neko/Relent/tree/main/Assets/Mochineko/Relent/UncertainResult)
 is an explicit error handling module for an uncertain operation that can be retryable failure.

### Core specification

- [IUncertainResult](https://github.com/mochi-neko/Relent/blob/main/Assets/Mochineko/Relent/UncertainResult/IUncertainResult.cs)
- [IUncertainSuccessResult](https://github.com/mochi-neko/Relent/blob/main/Assets/Mochineko/Relent/UncertainResult/IUncertainSuccessResult.cs)
- [IUncertainFailureResult](https://github.com/mochi-neko/Relent/blob/main/Assets/Mochineko/Relent/UncertainResult/IUncertainFailureResult.cs)

### Usage

### Samples



### How to import by Unity Package Manager

```json
{
  "dependencies": {
    "com.mochineko.relent.uncertain-result": "https://github.com/mochi-neko/Relent.git?path=/Assets/Mochineko/Relent/UncertainResult#0.1.0",
    ...
  }
}
```

## Resilience

[Resilience](https://github.com/mochi-neko/Relent/tree/main/Assets/Mochineko/Relent/Resilience)
 is a module that provides resilience for an uncertain operation.

### Core specification

- [IPolicy](https://github.com/mochi-neko/Relent/blob/main/Assets/Mochineko/Relent/Resilience/IPolicy.cs)

### Features

- [Retry]()
- [Timeout]()
- [Circuit Breaker]()
- [Bulkhead]()
- [Wrap]()

### Usage

### Samples

### How to import by Unity Package Manager

```json
{
  "dependencies": {
    "com.mochineko.relent.resilience": "https://github.com/mochi-neko/Relent.git?path=/Assets/Mochineko/Relent/Resilience#0.1.0",
    "com.mochineko.relent.uncertain-result": "https://github.com/mochi-neko/Relent.git?path=/Assets/Mochineko/Relent/UncertainResult#0.1.0",
    ...
  }
}
```

## Acknowledgments

This library is inspired by there posts and libraries:

- [HttpClient - Error handling, a test driven approach](https://josef.codes/httpclient-error-handling-a-test-driven-approach/)
- [Functional C#: Handling failures, input errors](https://enterprisecraftsmanship.com/posts/functional-c-handling-failures-input-errors/)
- [Polly](https://github.com/App-vNext/Polly)

## Changelog

See [CHANGELOG](https://github.com/mochi-neko/ChatGPT-API-unity/blob/main/CHANGELOG.md).

## 3rd Party Notices

See [NOTICE](https://github.com/mochi-neko/ChatGPT-API-unity/blob/main/NOTICE.md).

## License

[MIT License](https://github.com/mochi-neko/ChatGPT-API-unity/blob/main/LICENSE)