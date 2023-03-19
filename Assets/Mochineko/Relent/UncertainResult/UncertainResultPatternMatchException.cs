#nullable enable
namespace Mochineko.Relent.UncertainResult
{
    /// <summary>
    /// An exception that is thrown when an uncertain result pattern match is failed.
    /// </summary>
    public sealed class UncertainResultPatternMatchException
        : System.Exception
    {
        public UncertainResultPatternMatchException(string message)
            : base(message)
        {
        }
    }
}