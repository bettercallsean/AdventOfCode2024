namespace AdventOfCode.Utilities.ExtensionMethods;

public static class IEnumerableExtensions
{
    public static IEnumerable<IEnumerable<T>> Permute<T>(this IEnumerable<T> values) => values.SelectMany(x => Permute(new[] { new[] { x } }, values, values.Count() - 1));
    public static IEnumerable<IEnumerable<T>> Permute<T>(this IEnumerable<T> values, int permutations) => values.SelectMany(x => Permute(new[] { new[] { x } }, values, permutations - 1));
    private static IEnumerable<IEnumerable<T>> Permute<T>(IEnumerable<IEnumerable<T>> current, IEnumerable<T> values, int count) => (count == 1) ? Permute(current, values) : Permute(Permute(current, values), values, --count);
    private static IEnumerable<IEnumerable<T>> Permute<T>(IEnumerable<IEnumerable<T>> current, IEnumerable<T> values) => current.SelectMany(x => values.Select(y => x.Concat(new[] { y })));
}