#nullable enable
namespace Mochineko.Resilience.Wrap
{
    public static class WrapSyntax
    {
        public static IPolicy Wrap(
            this IPolicy outerPolicy,
            IPolicy innerPolicy)
            => new PolicyWrap(innerPolicy, outerPolicy);
        
        public static IPolicy<TResult> Wrap<TResult>(
            this IPolicy<TResult> outerPolicy,
            IPolicy<TResult> innerPolicy)
            => new PolicyWrap<TResult>(innerPolicy, outerPolicy);
    }
}