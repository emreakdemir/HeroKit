using System.Buffers;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace HeroKit.Strings;

/// <summary>
/// Provides extension methods for string manipulations.
/// </summary>
public static partial class StringManipulationExtensions
{
    private static readonly CultureInfo TurkishCulture = CultureInfo.GetCultureInfo("tr-TR");

    private static readonly SearchValues<char> TurkishChars = SearchValues.Create("ığüşöçİĞÜŞÖÇ");

    [GeneratedRegex(@"[^a-z0-9\s-]")]
    private static partial Regex InvalidSlugCharsRegex();

    [GeneratedRegex(@"\s+")]
    private static partial Regex WhitespaceRunRegex();

    [GeneratedRegex(@"\s")]
    private static partial Regex WhitespaceRegex();

    [GeneratedRegex("-+")]
    private static partial Regex DashRunRegex();

    [GeneratedRegex("[A-Z]")]
    private static partial Regex UpperCaseLetterRegex();

    /// <summary>
    /// Converts a string into a URL-friendly slug.
    /// </summary>
    /// <param name="value">The string to convert.</param>
    /// <returns>The URL-friendly slug.</returns>
    public static string ToSlug(this string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;

        string normalizedValue = value.RemoveAccent().ClearTurkishChars().ToLowerInvariant();
        normalizedValue = InvalidSlugCharsRegex().Replace(normalizedValue, "");
        normalizedValue = WhitespaceRunRegex().Replace(normalizedValue, " ").Trim();
        normalizedValue = normalizedValue[..Math.Min(normalizedValue.Length, 200)].Trim();
        normalizedValue = WhitespaceRegex().Replace(normalizedValue, "-");
        normalizedValue = DashRunRegex().Replace(normalizedValue, "-");

        return normalizedValue;
    }

    /// <summary>
    /// Removes accents from a string.
    /// </summary>
    /// <param name="value">The string to remove accents from.</param>
    /// <returns>The string without accents.</returns>
    public static string RemoveAccent(this string value)
    {
        string normalizedString = value.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder(normalizedString.Length);

        foreach (char c in normalizedString)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                stringBuilder.Append(c);
        }

        return stringBuilder.ToString();
    }

    /// <summary>
    /// Replaces Turkish characters with their non-accented equivalents in a string.
    /// </summary>
    /// <param name="value">The string to replace Turkish characters in.</param>
    /// <returns>The string with Turkish characters replaced.</returns>
    public static string ClearTurkishChars(this string? value)
    {
        if (value is null)
            return string.Empty;

        if (value.AsSpan().IndexOfAny(TurkishChars) < 0)
            return value;

        return string.Create(value.Length, value, static (destination, source) =>
        {
            for (int i = 0; i < source.Length; i++)
            {
                destination[i] = source[i] switch
                {
                    'ı' => 'i',
                    'ğ' => 'g',
                    'ü' => 'u',
                    'ş' => 's',
                    'ö' => 'o',
                    'ç' => 'c',
                    'İ' => 'I',
                    'Ğ' => 'G',
                    'Ü' => 'U',
                    'Ş' => 'S',
                    'Ö' => 'O',
                    'Ç' => 'C',
                    var c => c,
                };
            }
        });
    }

    /// <summary>
    /// Replaces the format items in a specified string with the string representation of specified objects.
    /// </summary>
    /// <param name="value">A composite format string.</param>
    /// <param name="args">An array of objects to format.</param>
    /// <returns>A copy of value in which the format items have been replaced by the string representations of the corresponding objects in <paramref name="args"/>.</returns>
    public static string Format(this string value, params object[] args) => string.Format(value, args);

    /// <summary>
    /// Converts the input string to dash case.
    /// </summary>
    /// <param name="input">The input string.</param>
    /// <returns>The string converted to dash case.</returns>
    public static string ToDashCase(this string input)
    {
        return UpperCaseLetterRegex().Replace(input, m => (m.Index == 0 ? string.Empty : "-") + m.Value.ToLowerInvariant());
    }
    
    /// <summary>
    /// Converts a string to uppercase using the Turkish culture.
    /// </summary>
    /// <param name="input">The string to convert.</param>
    /// <returns>The uppercase representation of the input string.</returns>
    public static string ToUpperTr(this string input) => input.ToUpper(TurkishCulture);

    /// <summary>
    /// Converts the input string to lowercase using the Turkish culture.
    /// </summary>
    /// <param name="input">The string to convert.</param>
    /// <returns>The converted lowercase string.</returns>
    public static string ToLowerTr(this string input) => input.ToLower(TurkishCulture);
}