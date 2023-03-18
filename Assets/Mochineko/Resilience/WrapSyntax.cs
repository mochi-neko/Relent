#nullable enable
namespace Mochineko.Resilience
{
    public static class WrapSyntax
    {
        public static IPolicy<TResult> Wrap<TResult>(
            this IPolicy<TResult> outerPolicy,
            IPolicy<TResult> innerPolicy)
            => new PolicyWrap<TResult>(innerPolicy, outerPolicy);
    }
}