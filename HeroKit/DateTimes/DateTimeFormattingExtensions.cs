using System.Globalization;
using HeroKit.Strings;

namespace HeroKit.DateTimes;

/// <summary>
/// Provides extension methods for DateTime values.
/// </summary>
public static class DateTimeFormattingExtensions
{
    /// <summary>
    /// Converts a DateTime to its Turkish culture string representation with the specified format.
    /// </summary>
    /// <param name="datetime">The DateTime to format.</param>
    /// <param name="format">The format string.</param>
    /// <returns>The Turkish culture formatted string.</returns>
    public static string ToTrString(this DateTime datetime, string format = "dd MMM yyyy, dddd") => datetime.ToCultureString(new CultureInfo("tr-TR"), format);

    /// <summary>
    /// Converts a DateTime to a string representation with the specified format and culture.
    /// </summary>
    /// <param name="datetime">The DateTime to format.</param>
    /// <param name="formatProvider">The format provider specifying the culture.</param>
    /// <param name="format">The format string.</param>
    /// <returns>The formatted string.</returns>
    public static string ToCultureString(this DateTime datetime, CultureInfo formatProvider, string format = "dd MMM yyyy, dddd") => datetime.ToString(format, formatProvider);

    /// <summary>
    /// Converts a DateTime to a string representation suitable for a directory name with the specified format.
    /// </summary>
    /// <param name="datetime">The DateTime to format.</param>
    /// <param name="format">The format string.</param>
    /// <returns>The formatted string.</returns>
    public static string AsDirectoryName(this DateTime datetime, string format = "yyyy-MM-dd") => datetime.ToString(format, CultureInfo.InvariantCulture);

    /// <summary>
    /// Converts a DateTime to a string representation suitable for a file name.
    /// </summary>
    /// <param name="datetime">The DateTime to format.</param>
    /// <param name="baseFileName">The base file name to use.</param>
    /// <returns>The formatted file name string.</returns>
    public static string AsFileName(this DateTime datetime, string baseFileName = "")
    {
        if (baseFileName.IsNullOrEmpty())
            return datetime.ToString("yyyy-MM-dd-HH-mm-ss", CultureInfo.InvariantCulture);

        return $"{baseFileName.ToSlug()}-{datetime.ToString("yyyy-MM-dd-HH-mm-ss", CultureInfo.InvariantCulture)}";
    }
}