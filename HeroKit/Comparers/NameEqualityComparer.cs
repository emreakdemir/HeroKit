namespace HeroKit.Comparers;

/// <summary>
/// Represents an equality comparer for comparing string values based on name comparison using a case-insensitive comparison.
/// </summary>
public class NameEqualityComparer : IEqualityComparer<string>
{
    /// <summary>
    /// Gets the default instance of <see cref="IEqualityComparer{T}"/> that uses the <see cref="NameEqualityComparer"/> implementation.
    /// </summary>
    /// <returns>The default instance of <see cref="IEqualityComparer{T}"/>.</returns>
    public static IEqualityComparer<string> Default => new NameEqualityComparer();

    /// <summary>
    /// Determines whether two specified strings have the same value, ignoring the case.
    /// </summary>
    /// <param name="source">The first string to compare.</param>
    /// <param name="control">The second string to compare.</param>
    /// <returns>
    /// true if the two strings are equal; otherwise, false.
    /// </returns>
    public bool Equals(string source, string control)
    {
        if (source == null && control == null)
            return true;

        if (source == null || control == null)
            return false;

        return source.Equals(control, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Returns a case-insensitive hash code for the specified string, consistent with <see cref="Equals(string, string)"/>.
    /// </summary>
    /// <param name="value">The string for which to get the hash code.</param>
    /// <returns>The hash code value for the <paramref name="value"/> parameter.</returns>
    public int GetHashCode(string value) => value is null ? 0 : StringComparer.OrdinalIgnoreCase.GetHashCode(value);
}
