#nullable enable
namespace Mochineko.Relent.Result
{
    /// <summary>
    /// An exception that is thrown when a result pattern match is failed.
    /// </summary>
    public sealed class ResultPatternMatchException
        : System.Exception
    {
        public ResultPatternMatchException(string message)
            : base(message)
        {
        }
    }
}