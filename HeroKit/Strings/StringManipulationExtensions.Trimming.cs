using System.Diagnostics.CodeAnalysis;

namespace HeroKit.Strings;

public static partial class StringManipulationExtensions
{
    /// <summary>
    /// Shortens a string to a maximum length and appends an ellipsis when it was cut.
    /// </summary>
    /// <remarks>
    /// The result, including the ellipsis, is never longer than <paramref name="maxLength"/>.
    /// The cut never splits a surrogate pair, so emoji and other characters outside the BMP stay intact.
    /// </remarks>
    /// <param name="value">The string to shorten.</param>
    /// <param name="maxLength">The maximum length of the result.</param>
    /// <param name="ellipsis">The text appended when the string is cut. Defaults to "…".</param>
    /// <returns>The original string if it fits; otherwise the shortened string. Null if <paramref name="value"/> is null.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="maxLength"/> is shorter than <paramref name="ellipsis"/>.</exception>
    /// <example>
    /// <code>
    /// "HeroKit extensions".Truncate(10);      // "HeroKit e…"
    /// "HeroKit".Truncate(10);                 // "HeroKit"
    /// </code>
    /// </example>
    [return: NotNullIfNotNull(nameof(value))]
    public static string? Truncate(this string? value, int maxLength, string ellipsis = "…")
    {
        ArgumentNullException.ThrowIfNull(ellipsis);
        ArgumentOutOfRangeException.ThrowIfLessThan(maxLength, ellipsis.Length);

        if (value is null || value.Length <= maxLength)
            return value;

        int cut = maxLength - ellipsis.Length;
        if (cut > 0 && char.IsHighSurrogate(value[cut - 1]))
            cut--;

        return string.Concat(value.AsSpan(0, cut), ellipsis);
    }

    /// <summary>
    /// Hides all but the last characters of a string, for example a card number, phone number or IBAN.
    /// </summary>
    /// <remarks>
    /// Whitespace is kept and not counted, so formatted values keep their layout: "TR33 0006 1005" becomes "**** **** 1005".
    /// </remarks>
    /// <param name="value">The string to mask.</param>
    /// <param name="visibleCount">The number of non-whitespace characters to keep visible at the end.</param>
    /// <param name="maskChar">The character that replaces hidden characters. Defaults to '*'.</param>
    /// <returns>The masked string, or null if <paramref name="value"/> is null.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="visibleCount"/> is negative.</exception>
    /// <example>
    /// <code>
    /// "5321234567".Mask();        // "******4567"
    /// "0532 123 45 67".Mask(2);   // "**** *** ** 67"
    /// </code>
    /// </example>
    [return: NotNullIfNotNull(nameof(value))]
    public static string? Mask(this string? value, int visibleCount = 4, char maskChar = '*')
    {
        ArgumentOutOfRangeException.ThrowIfNegative(visibleCount);

        if (string.IsNullOrEmpty(value))
            return value;

        int visibleFrom = value.Length;
        for (int seen = 0; visibleFrom > 0 && seen < visibleCount; visibleFrom--)
        {
            if (!char.IsWhiteSpace(value[visibleFrom - 1]))
                seen++;
        }

        return string.Create(value.Length, (value, visibleFrom, maskChar), static (destination, state) =>
        {
            for (int i = 0; i < state.value.Length; i++)
            {
                char c = state.value[i];
                destination[i] = i >= state.visibleFrom || char.IsWhiteSpace(c) ? c : state.maskChar;
            }
        });
    }

    /// <summary>
    /// Returns the first characters of a string. Shorter strings are returned whole.
    /// </summary>
    /// <param name="value">The source string.</param>
    /// <param name="length">The maximum number of characters to return.</param>
    /// <returns>The first <paramref name="length"/> characters, or null if <paramref name="value"/> is null.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is negative.</exception>
    [return: NotNullIfNotNull(nameof(value))]
    public static string? Left(this string? value, int length)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(length);

        return value is null || value.Length <= length ? value : value[..length];
    }

    /// <summary>
    /// Returns the last characters of a string. Shorter strings are returned whole.
    /// </summary>
    /// <param name="value">The source string.</param>
    /// <param name="length">The maximum number of characters to return.</param>
    /// <returns>The last <paramref name="length"/> characters, or null if <paramref name="value"/> is null.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is negative.</exception>
    [return: NotNullIfNotNull(nameof(value))]
    public static string? Right(this string? value, int length)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(length);

        return value is null || value.Length <= length ? value : value[^length..];
    }

    /// <summary>
    /// Removes all whitespace characters, including tabs, line breaks and non-breaking spaces.
    /// </summary>
    /// <param name="value">The source string.</param>
    /// <returns>The string without whitespace, or null if <paramref name="value"/> is null.</returns>
    /// <example>
    /// <code>
    /// "TR33 0006\t1005".RemoveWhitespace(); // "TR3300061005"
    /// </code>
    /// </example>
    [return: NotNullIfNotNull(nameof(value))]
    public static string? RemoveWhitespace(this string? value)
    {
        if (value is null)
            return null;

        int index = 0;
        Span<char> buffer = value.Length <= 256 ? stackalloc char[value.Length] : new char[value.Length];

        foreach (char c in value)
        {
            if (!char.IsWhiteSpace(c))
                buffer[index++] = c;
        }

        return index == value.Length ? value : new string(buffer[..index]);
    }
}
