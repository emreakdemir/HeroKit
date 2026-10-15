namespace HeroKit.Turkish;

/// <summary>
/// LINQ helpers that sort in Turkish alphabetical order with <see cref="TurkishStringComparer"/>.
/// </summary>
public static class TurkishSortingExtensions
{
    /// <summary>
    /// Sorts strings in Turkish alphabetical order.
    /// </summary>
    /// <remarks>Case-sensitive. To ignore case, use <c>source.OrderByTurkish(s =&gt; s, ignoreCase: true)</c>.</remarks>
    /// <param name="source">The strings to sort.</param>
    /// <returns>The sorted sequence.</returns>
    /// <example>
    /// <code>
    /// new[] { "şeker", "su", "çay", "cam" }.OrderByTurkish(); // cam, çay, su, şeker
    /// </code>
    /// </example>
    public static IOrderedEnumerable<string> OrderByTurkish(this IEnumerable<string> source) =>
        source.OrderBy(value => value, TurkishStringComparer.Instance);

    /// <summary>
    /// Sorts elements by a string key in Turkish alphabetical order.
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    /// <param name="source">The elements to sort.</param>
    /// <param name="keySelector">Selects the string to sort by.</param>
    /// <param name="ignoreCase">True to ignore case with Turkish rules.</param>
    /// <returns>The sorted sequence.</returns>
    /// <example>
    /// <code>
    /// customers.OrderByTurkish(c => c.Name).ThenByTurkish(c => c.City);
    /// </code>
    /// </example>
    public static IOrderedEnumerable<T> OrderByTurkish<T>(this IEnumerable<T> source, Func<T, string?> keySelector, bool ignoreCase = false) =>
        source.OrderBy(keySelector, Comparer(ignoreCase));

    /// <summary>
    /// Sorts elements by a string key in descending Turkish alphabetical order.
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    /// <param name="source">The elements to sort.</param>
    /// <param name="keySelector">Selects the string to sort by.</param>
    /// <param name="ignoreCase">True to ignore case with Turkish rules.</param>
    /// <returns>The sorted sequence.</returns>
    public static IOrderedEnumerable<T> OrderByTurkishDescending<T>(this IEnumerable<T> source, Func<T, string?> keySelector, bool ignoreCase = false) =>
        source.OrderByDescending(keySelector, Comparer(ignoreCase));

    /// <summary>
    /// Performs a subsequent sort by a string key in Turkish alphabetical order.
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    /// <param name="source">The already sorted elements.</param>
    /// <param name="keySelector">Selects the string to sort by.</param>
    /// <param name="ignoreCase">True to ignore case with Turkish rules.</param>
    /// <returns>The sorted sequence.</returns>
    public static IOrderedEnumerable<T> ThenByTurkish<T>(this IOrderedEnumerable<T> source, Func<T, string?> keySelector, bool ignoreCase = false) =>
        source.ThenBy(keySelector, Comparer(ignoreCase));

    private static TurkishStringComparer Comparer(bool ignoreCase) =>
        ignoreCase ? TurkishStringComparer.IgnoreCase : TurkishStringComparer.Instance;
}
