using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace HeroKit.Strings;

/// <summary>
/// Converts strings to other types.
/// </summary>
/// <remarks>
/// Every conversion comes in two forms. <c>ToX</c> is lenient: it returns a default value for null, empty or invalid input and never throws.
/// <c>TryToX</c> reports success: it returns false for null, empty or invalid input and never throws.
/// Parsing uses the invariant culture unless a format provider is passed.
/// </remarks>
public static class StringConversionExtensions
{
    /// <summary>
    /// Converts a string to any type that implements <see cref="IParsable{TSelf}"/>, such as int, decimal, DateTime, DateOnly or Guid.
    /// </summary>
    /// <typeparam name="T">The target type.</typeparam>
    /// <param name="value">The string to convert.</param>
    /// <param name="defaultValue">The value returned when the string is null, empty or invalid.</param>
    /// <param name="provider">The format provider. Defaults to <see cref="CultureInfo.InvariantCulture"/>.</param>
    /// <returns>The converted value, or <paramref name="defaultValue"/>.</returns>
    /// <example>
    /// <code>
    /// "42".To&lt;int&gt;();                 // 42
    /// "2026-03-07".To&lt;DateOnly&gt;();    // 2026-03-07
    /// "x".To(defaultValue: -1);       // -1
    /// </code>
    /// </example>
    public static T To<T>(this string? value, T defaultValue = default!, IFormatProvider? provider = null)
        where T : IParsable<T>
        => value.TryTo<T>(out T? result, provider) ? result : defaultValue;

    /// <summary>
    /// Tries to convert a string to any type that implements <see cref="IParsable{TSelf}"/>.
    /// </summary>
    /// <typeparam name="T">The target type.</typeparam>
    /// <param name="value">The string to convert.</param>
    /// <param name="result">The converted value when the method returns true; otherwise the default value of <typeparamref name="T"/>.</param>
    /// <param name="provider">The format provider. Defaults to <see cref="CultureInfo.InvariantCulture"/>.</param>
    /// <returns>True if the string was converted; otherwise false.</returns>
    /// <example>
    /// <code>
    /// if ("42".TryTo(out int number)) { /* number == 42 */ }
    /// </code>
    /// </example>
    public static bool TryTo<T>(this string? value, [MaybeNullWhen(false)] out T result, IFormatProvider? provider = null)
        where T : IParsable<T>
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            result = default;
            return false;
        }

        return T.TryParse(value.Trim(), provider ?? CultureInfo.InvariantCulture, out result);
    }

    /// <summary>
    /// Converts a string to a nullable value. Returns null when the string is null, empty or invalid.
    /// </summary>
    /// <typeparam name="T">The target value type.</typeparam>
    /// <param name="value">The string to convert.</param>
    /// <param name="provider">The format provider. Defaults to <see cref="CultureInfo.InvariantCulture"/>.</param>
    /// <returns>The converted value, or null.</returns>
    /// <example>
    /// <code>
    /// "42".ToNullable&lt;int&gt;();   // 42
    /// "abc".ToNullable&lt;int&gt;();  // null
    /// </code>
    /// </example>
    public static T? ToNullable<T>(this string? value, IFormatProvider? provider = null)
        where T : struct, IParsable<T>
        => value.TryTo(out T result, provider) ? result : null;

    /// <summary>
    /// Converts a string to an integer.
    /// </summary>
    /// <param name="value">The string to convert.</param>
    /// <param name="defaultValue">The value returned when the string is null, empty or invalid.</param>
    /// <returns>The integer, or <paramref name="defaultValue"/>.</returns>
    public static int ToInt(this string? value, int defaultValue = 0) => value.To(defaultValue);

    /// <summary>
    /// Converts a string to a long integer.
    /// </summary>
    /// <param name="value">The string to convert.</param>
    /// <param name="defaultValue">The value returned when the string is null, empty or invalid.</param>
    /// <returns>The long integer, or <paramref name="defaultValue"/>.</returns>
    public static long ToLong(this string? value, long defaultValue = 0) => value.To(defaultValue);

    /// <summary>
    /// Converts a string to a short integer.
    /// </summary>
    /// <param name="value">The string to convert.</param>
    /// <param name="defaultValue">The value returned when the string is null, empty or invalid.</param>
    /// <returns>The short integer, or <paramref name="defaultValue"/>.</returns>
    public static short ToShort(this string? value, short defaultValue = 0) => value.To(defaultValue);

    /// <summary>
    /// Converts a string to a decimal.
    /// </summary>
    /// <remarks>
    /// Without a provider the conversion is flexible: a comma is the decimal separator when present ("1.234,56"),
    /// otherwise a dot is ("1234.56"). With a provider the string is parsed strictly with that culture.
    /// </remarks>
    /// <param name="value">The string to convert.</param>
    /// <param name="defaultValue">The value returned when the string is null, empty or invalid.</param>
    /// <param name="provider">The format provider for strict parsing, or null for the flexible rule.</param>
    /// <returns>The decimal, or <paramref name="defaultValue"/>.</returns>
    /// <example>
    /// <code>
    /// "1.234,56".ToDecimal();  // 1234.56
    /// "1234.56".ToDecimal();   // 1234.56
    /// "abc".ToDecimal(-1m);    // -1
    /// </code>
    /// </example>
    public static decimal ToDecimal(this string? value, decimal defaultValue = 0m, IFormatProvider? provider = null)
        => value.TryToDecimal(out decimal result, provider) ? result : defaultValue;

    /// <summary>
    /// Tries to convert a string to a decimal with the same rules as <see cref="ToDecimal"/>.
    /// </summary>
    /// <param name="value">The string to convert.</param>
    /// <param name="result">The decimal when the method returns true; otherwise zero.</param>
    /// <param name="provider">The format provider for strict parsing, or null for the flexible rule.</param>
    /// <returns>True if the string was converted; otherwise false.</returns>
    public static bool TryToDecimal(this string? value, out decimal result, IFormatProvider? provider = null)
    {
        result = 0m;

        if (string.IsNullOrWhiteSpace(value))
            return false;

        string trimmed = value.Trim();

        if (provider is not null)
            return decimal.TryParse(trimmed, NumberStyles.Number, provider, out result);

        bool commaIsDecimal = trimmed.Contains(',');
        var format = new NumberFormatInfo
        {
            NumberDecimalSeparator = commaIsDecimal ? "," : ".",
            NumberGroupSeparator = commaIsDecimal ? "." : ",",
        };

        return decimal.TryParse(trimmed, NumberStyles.Number, format, out result);
    }

    /// <summary>
    /// Converts a string to a DateTime.
    /// </summary>
    /// <param name="value">The string to convert.</param>
    /// <param name="defaultValue">The value returned when the string is null, empty or invalid.</param>
    /// <param name="provider">The format provider. Defaults to <see cref="CultureInfo.InvariantCulture"/>.</param>
    /// <returns>The DateTime, or <paramref name="defaultValue"/>.</returns>
    /// <example>
    /// <code>
    /// "2026-03-07".ToDateTime();                                          // 7 March 2026
    /// "07.03.2026".ToDateTime(provider: CultureInfo.GetCultureInfo("tr-TR")); // 7 March 2026
    /// </code>
    /// </example>
    public static DateTime ToDateTime(this string? value, DateTime defaultValue = default, IFormatProvider? provider = null)
        => value.To(defaultValue, provider);

    /// <summary>
    /// Converts a string to a boolean. Accepts "true"/"false" in any case and an optional custom true text.
    /// </summary>
    /// <param name="value">The string to convert.</param>
    /// <param name="trueText">An extra text that means true, compared ignoring case, for example "yes" or "1".</param>
    /// <param name="defaultValue">The value returned when the string is null or empty.</param>
    /// <returns>The boolean value. Any other non-empty text returns false.</returns>
    /// <example>
    /// <code>
    /// "TRUE".ToBoolean();               // true
    /// "yes".ToBoolean(trueText: "yes"); // true
    /// "".ToBoolean(defaultValue: true); // true
    /// </code>
    /// </example>
    public static bool ToBoolean(this string? value, string? trueText = null, bool defaultValue = false)
    {
        if (string.IsNullOrWhiteSpace(value))
            return defaultValue;

        string trimmed = value.Trim();

        if (bool.TryParse(trimmed, out bool result))
            return result;

        return trueText is not null && trimmed.Equals(trueText, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Converts a string to an enum value by name (ignoring case) or by a defined numeric value.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <param name="value">The string to convert.</param>
    /// <param name="defaultValue">The value returned when the string is null, empty or not a defined value.</param>
    /// <returns>The enum value, or <paramref name="defaultValue"/>.</returns>
    /// <example>
    /// <code>
    /// "Friday".ToEnum&lt;DayOfWeek&gt;();   // DayOfWeek.Friday
    /// "5".ToEnum&lt;DayOfWeek&gt;();        // DayOfWeek.Friday
    /// "99".ToEnum(DayOfWeek.Monday);  // DayOfWeek.Monday
    /// </code>
    /// </example>
    public static T ToEnum<T>(this string? value, T defaultValue = default)
        where T : struct, Enum
        => value.TryToEnum(out T result) ? result : defaultValue;

    /// <summary>
    /// Tries to convert a string to an enum value by name (ignoring case) or by a defined numeric value.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <param name="value">The string to convert.</param>
    /// <param name="result">The enum value when the method returns true; otherwise the default value.</param>
    /// <returns>True if the string matches a defined value; otherwise false.</returns>
    public static bool TryToEnum<T>(this string? value, out T result)
        where T : struct, Enum
    {
        result = default;

        if (string.IsNullOrWhiteSpace(value))
            return false;

        if (!Enum.TryParse(value.Trim(), ignoreCase: true, out T parsed) || !Enum.IsDefined(parsed))
            return false;

        result = parsed;
        return true;
    }

    /// <summary>
    /// Decodes a base64 string to bytes.
    /// </summary>
    /// <param name="value">The base64 string.</param>
    /// <returns>The decoded bytes, or null if the string is null, empty or not valid base64.</returns>
    /// <example>
    /// <code>
    /// "SGVyb0tpdA==".FromBase64(); // bytes of "HeroKit"
    /// </code>
    /// </example>
    public static byte[]? FromBase64(this string? value)
    {
        if (string.IsNullOrEmpty(value))
            return null;

        byte[] buffer = new byte[value.Length * 3 / 4];
        return Convert.TryFromBase64String(value, buffer, out int written) ? buffer[..written] : null;
    }
}
