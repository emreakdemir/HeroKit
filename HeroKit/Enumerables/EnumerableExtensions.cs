namespace HeroKit.Enumerables;

/// <summary>
/// Provides extension methods for manipulating enumerable collections.
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// Partitions a given list into a sequence of smaller lists.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="source">The list to partition.</param>
    /// <param name="size">The maximum size of each partition.</param>
    /// <returns>A sequence of smaller lists.</returns>
    public static IEnumerable<List<T>> Partition<T>(this IList<T> source, int size)
    {
        for (var i = 0; i < Math.Ceiling(source.Count / (double)size); i++)
            yield return [..source.Skip(size * i).Take(size)];
    }
}