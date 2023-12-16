using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace HeroKit.Strings;

/// <summary>
/// Provides extension methods for string manipulations.
/// </summary>
public static class StringManipulationExtensions
{
    /// <summary>
    /// Converts a string into a URL-friendly slug.
    /// </summary>
    /// <param name="value">The string to convert.</param>
    /// <returns>The URL-friendly slug.</returns>
    public static string ToSlug(this string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;

        string normalizedValue = value.RemoveAccent().ClearTurkishChars().ToLowerInvariant();
        normalizedValue = Regex.Replace(normalizedValue, @"[^a-z0-9\s-]", "");
        normalizedValue = Regex.Replace(normalizedValue, @"\s+", " ").Trim();
        normalizedValue = normalizedValue[..(normalizedValue.Length <= 200 ? normalizedValue.Length : 200)].Trim();
        normalizedValue = Regex.Replace(normalizedValue, @"\s", "-");
        normalizedValue = Regex.Replace(normalizedValue, "-+", "-");

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
        var stringBuilder = new StringBuilder();

        IEnumerable<char> nonSpacingMarks = normalizedString.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark);
        stringBuilder.Append(nonSpacingMarks.ToArray());

        return stringBuilder.ToString();
    }

    /// <summary>
    /// Replaces Turkish characters with their non-accented equivalents in a string.
    /// </summary>
    /// <param name="value">The string to replace Turkish characters in.</param>
    /// <returns>The string with Turkish characters replaced.</returns>
    public static string ClearTurkishChars(this string value)
    {
        var sb = new StringBuilder(value);
        sb = sb.Replace("ı", "i")
               .Replace("ğ", "g")
               .Replace("ü", "u")
               .Replace("ş", "s")
               .Replace("ö", "o")
               .Replace("ç", "c")
               .Replace("İ", "I")
               .Replace("Ğ", "G")
               .Replace("Ü", "U")
               .Replace("Ş", "S")
               .Replace("Ö", "O")
               .Replace("Ç", "C");

        return sb.ToString();
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
        const string pattern = "[A-Z]";
        const string dash = "-";
        return Regex.Replace(input, pattern, m => ((m.Index == 0) ? string.Empty : dash) + m.Value.ToLowerInvariant());
    }
    
    /// <summary>
    /// Converts a string to uppercase using the Turkish culture.
    /// </summary>
    /// <param name="input">The string to convert.</param>
    /// <returns>The uppercase representation of the input string.</returns>
    public static string ToUpperTr(this string input) => input.ToUpper(new CultureInfo("tr-TR"));

    /// <summary>
    /// Converts the input string to lowercase using the Turkish culture.
    /// </summary>
    /// <param name="input">The string to convert.</param>
    /// <returns>The converted lowercase string.</returns>
    public static string ToLowerTr(this string input) => input.ToLower(new CultureInfo("tr-TR"));
}