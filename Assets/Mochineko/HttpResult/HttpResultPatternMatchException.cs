#nullable enable
namespace Mochineko.HttpResult
{
    /// <summary>
    /// An exception that is thrown when a HTTP result pattern match is failed.
    /// </summary>
    public sealed class HttpResultPatternMatchException
        : System.Exception
    {
        public HttpResultPatternMatchException(string message)
            : base(message)
        {
        }
    }
}