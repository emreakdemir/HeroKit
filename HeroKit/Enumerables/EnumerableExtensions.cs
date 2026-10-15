using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace HeroKit.Enumerables;

/// <summary>
/// Provides helpers for sequences that LINQ does not include.
/// </summary>
/// <remarks>
/// Methods that .NET already provides (Chunk, DistinctBy, MinBy, MaxBy, ...) are intentionally not duplicated.
/// </remarks>
public static class EnumerableExtensions
{
    /// <summary>
    /// Checks whether a sequence is null or has no elements.
    /// </summary>
    /// <remarks>
    /// Collections are checked by count. Other sequences are enumerated until the first element only.
    /// </remarks>
    /// <typeparam name="T">The element type.</typeparam>
    /// <param name="source">The sequence to check.</param>
    /// <returns>True if the sequence is null or empty; otherwise false.</returns>
    /// <example>
    /// <code>
    /// new List&lt;int&gt;().IsNullOrEmpty(); // true
    /// </code>
    /// </example>
    public static bool IsNullOrEmpty<T>([NotNullWhen(false)] this IEnumerable<T>? source)
    {
        if (source is null)
            return true;

        if (source.TryGetNonEnumeratedCount(out int count))
            return count == 0;

        using IEnumerator<T> enumerator = source.GetEnumerator();
        return !enumerator.MoveNext();
    }

    /// <summary>
    /// Checks whether a sequence has at least one element.
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    /// <param name="source">The sequence to check.</param>
    /// <returns>True if the sequence is not null and has an element; otherwise false.</returns>
    public static bool IsNotNullOrEmpty<T>([NotNullWhen(true)] this IEnumerable<T>? source) => !source.IsNullOrEmpty();

    /// <summary>
    /// Filters a sequence only when a condition is true.
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    /// <param name="source">The sequence to filter.</param>
    /// <param name="condition">When false, the sequence is returned unchanged.</param>
    /// <param name="predicate">The filter to apply when <paramref name="condition"/> is true.</param>
    /// <returns>The filtered or the original sequence.</returns>
    /// <example>
    /// <code>
    /// products.WhereIf(onlyInStock, p => p.Stock > 0);
    /// </code>
    /// </example>
    public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, bool condition, Func<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(predicate);

        return condition ? source.Where(predicate) : source;
    }

    /// <summary>
    /// Filters a query only when a condition is true. The filter is translated by the query provider, for example Entity Framework.
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    /// <param name="source">The query to filter.</param>
    /// <param name="condition">When false, the query is returned unchanged.</param>
    /// <param name="predicate">The filter to apply when <paramref name="condition"/> is true.</param>
    /// <returns>The filtered or the original query.</returns>
    public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, bool condition, Expression<Func<T, bool>> predicate)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(predicate);

        return condition ? source.Where(predicate) : source;
    }

    /// <summary>
    /// Returns one page of a sequence.
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    /// <param name="source">The sequence to page.</param>
    /// <param name="pageNumber">The 1-based page number.</param>
    /// <param name="pageSize">The number of elements per page.</param>
    /// <returns>The elements of the requested page. A page past the end is empty.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="pageNumber"/> or <paramref name="pageSize"/> is less than 1.</exception>
    /// <example>
    /// <code>
    /// Enumerable.Range(1, 10).Page(2, 3); // 4, 5, 6
    /// </code>
    /// </example>
    public static IEnumerable<T> Page<T>(this IEnumerable<T> source, int pageNumber, int pageSize)
    {
        ArgumentNullException.ThrowIfNull(source);

        return source.Skip(SkipCount(pageNumber, pageSize)).Take(pageSize);
    }

    /// <summary>
    /// Returns one page of a query. Skip and Take are translated by the query provider, for example Entity Framework.
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    /// <param name="source">The query to page. Order it first for stable pages.</param>
    /// <param name="pageNumber">The 1-based page number.</param>
    /// <param name="pageSize">The number of elements per page.</param>
    /// <returns>The query for the requested page.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="pageNumber"/> or <paramref name="pageSize"/> is less than 1.</exception>
    public static IQueryable<T> Page<T>(this IQueryable<T> source, int pageNumber, int pageSize)
    {
        ArgumentNullException.ThrowIfNull(source);

        return source.Skip(SkipCount(pageNumber, pageSize)).Take(pageSize);
    }

    /// <summary>
    /// Copies a sequence into a read-only list. Later changes to the source do not affect the result.
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    /// <param name="source">The sequence to copy.</param>
    /// <returns>A read-only list that cannot be cast back to a mutable collection.</returns>
    public static IReadOnlyList<T> ToReadOnlyList<T>(this IEnumerable<T> source)
    {
        ArgumentNullException.ThrowIfNull(source);

        return new ReadOnlyCollection<T>(source.ToArray());
    }

    private static int SkipCount(int pageNumber, int pageSize)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(pageNumber, 1);
        ArgumentOutOfRangeException.ThrowIfLessThan(pageSize, 1);

        // Pages past int.MaxValue elements are empty instead of overflowing.
        return (int)Math.Min((long)(pageNumber - 1) * pageSize, int.MaxValue);
    }
}
