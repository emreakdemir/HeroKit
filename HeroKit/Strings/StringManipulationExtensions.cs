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
}