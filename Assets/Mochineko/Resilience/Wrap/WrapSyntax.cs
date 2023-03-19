#nullable enable
namespace Mochineko.Resilience.Wrap
{
    /// <summary>
    /// A syntax for wrapping policies.
    /// </summary>
    public static class WrapSyntax
    {
        /// <summary>
        /// Wraps the inner policy with the outer policy.
        /// </summary>
        /// <param name="outerPolicy"></param>
        /// <param name="innerPolicy"></param>
        /// <returns></returns>
        public static IPolicy Wrap(
            this IPolicy outerPolicy,
            IPolicy innerPolicy)
            => new PolicyWrap(innerPolicy, outerPolicy);
        
        /// <summary>
        /// Wraps the inner policy with the outer policy.
        /// </summary>
        /// <param name="outerPolicy"></param>
        /// <param name="innerPolicy"></param>
        /// <typeparam name="TResult">Type of result value</typeparam>
        /// <returns></returns>
        public static IPolicy<TResult> Wrap<TResult>(
            this IPolicy<TResult> outerPolicy,
            IPolicy<TResult> innerPolicy)
            => new PolicyWrap<TResult>(innerPolicy, outerPolicy);
    }
}