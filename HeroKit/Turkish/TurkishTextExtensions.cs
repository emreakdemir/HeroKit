using System.Globalization;

namespace HeroKit.Turkish;

/// <summary>
/// Provides casing and comparison helpers that follow Turkish rules for dotted and dotless I (İ/i, I/ı).
/// </summary>
public static class TurkishTextExtensions
{
    private static readonly CultureInfo TurkishCulture = CultureInfo.GetCultureInfo("tr-TR");

    /// <summary>
    /// Converts a string to title case with Turkish casing rules.
    /// </summary>
    /// <remarks>
    /// Words start after whitespace or a hyphen. The first letter of each word is upper-cased and the rest is lower-cased.
    /// Letters after an apostrophe stay lower case, so suffixes such as "'da" are kept: "istanbul'da" becomes "İstanbul'da".
    /// </remarks>
    /// <param name="value">The string to convert.</param>
    /// <returns>The title-cased string, or an empty string if <paramref name="value"/> is null.</returns>
    /// <example>
    /// <code>
    /// "istanbul ılıca".ToTitleCaseTr();  // "İstanbul Ilıca"
    /// "IŞIK ÇİÇEK".ToTitleCaseTr();      // "Işık Çiçek"
    /// </code>
    /// </example>
    public static string ToTitleCaseTr(this string? value)
    {
        if (string.IsNullOrEmpty(value))
            return string.Empty;

        return string.Create(value.Length, value, static (destination, source) =>
        {
            bool startOfWord = true;

            for (int i = 0; i < source.Length; i++)
            {
                char c = source[i];
                destination[i] = startOfWord ? char.ToUpper(c, TurkishCulture) : char.ToLower(c, TurkishCulture);
                startOfWord = char.IsWhiteSpace(c) || c == '-';
            }
        });
    }

    /// <summary>
    /// Compares two strings for equality, ignoring case with Turkish rules: "I" matches "ı" and "İ" matches "i".
    /// </summary>
    /// <param name="value">The first string.</param>
    /// <param name="other">The second string.</param>
    /// <returns>True if both strings are null or equal ignoring Turkish case; otherwise false.</returns>
    /// <example>
    /// <code>
    /// "IŞIK".EqualsTr("ışık");   // true
    /// "ISIK".EqualsTr("isik");   // false: I is the capital of ı, not i
    /// </code>
    /// </example>
    public static bool EqualsTr(this string? value, string? other) =>
        string.Compare(value, other, TurkishCulture, CompareOptions.IgnoreCase) == 0;
}
