using System.ComponentModel;
using System.Globalization;
using HeroKit.Objects;
using HeroKit.Types;

namespace HeroKit.Strings;

/// <summary>
/// Provides extension methods for string value checking and manipulation.
/// </summary>
public static class StringConversionExtensions
{
    /// <summary>
    /// Converts a string to a decimal number using the specified number format.
    /// </summary>
    /// <param name="this">The string to convert to decimal.</param>
    /// <param name="numberDecimalSeperator">The decimal separator character.</param>
    /// <param name="autoReplaceDotToSeperator">Whether to automatically replace dot with the specified separator.</param>
    /// <returns>The decimal representation of the string.</returns>
    public static decimal ToDecimal(this string @this, string numberDecimalSeperator = ",", bool autoReplaceDotToSeperator = true)
    {
        var numberFormat = new NumberFormatInfo
        {
            NumberDecimalSeparator = numberDecimalSeperator,
        };
        if (autoReplaceDotToSeperator)
            @this = @this.Replace(".", numberDecimalSeperator);

        return Convert.ToDecimal(@this, numberFormat);
    }

    /// <summary>
    /// Converts a string to an enum value of the specified type.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <param name="value">The string to convert to enum.</param>
    /// <param name="defaultValue">The default value if conversion fails.</param>
    /// <returns>The enum value converted from the string.</returns>
    public static T ToEnum<T>(this string value, T defaultValue = default)
    {
        try
        {
            if (string.IsNullOrEmpty(value))
                return defaultValue;

            if (!value.IsNumeric())
                return defaultValue;

            if (!int.TryParse(value, out int numeric))
                return defaultValue;

            Type type = typeof(T).GetUnderlyingType();

            if (Enum.IsDefined(type, value))
                return (T)Enum.Parse(type, value, true);

            if (Enum.IsDefined(type, numeric))
                return (T)Enum.ToObject(type, numeric);
        }
        catch
        {
            // ignored
        }

        return defaultValue;
    }

    /// <summary>
    /// Converts a base64 encoded string to a byte array.
    /// </summary>
    /// <param name="text">The base64 encoded string to convert.</param>
    /// <returns>The byte array representation of the base64 encoded string.</returns>
    public static byte[] ToByteArray(this string text) => Convert.FromBase64String(text);

    /// <summary>
    /// Converts a string to a nullable value of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of the value to convert to.</typeparam>
    /// <param name="source">The string to convert.</param>
    /// <returns>
    /// A tuple containing the conversion status and the nullable result value.
    /// The status indicates whether the conversion was successful or not.
    /// The result value is the converted nullable value.
    /// </returns>
    public static (bool status, T? result) ToNullable<T>(this string source)
            where T : struct
    {
        var result = new T?();
        try
        {
            if (source.IsNotNullOrEmpty() && source.Trim().IsNotNullOrEmpty())
            {
                TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
                result = (T)converter.ConvertFromInvariantString(source)!;
            }
        }
        catch
        {
            return (false, result);
        }

        return (true, result);
    }

    /// <summary>
    /// Converts the specified string value to a boolean value based on the specified true condition and default value.
    /// </summary>
    /// <param name="value">The string value to be converted.</param>
    /// <param name="trueCondition">The condition that represents a true value. Default value is "true".</param>
    /// <param name="defaultvalue">The default value to be returned if the conversion fails. Default value is false.</param>
    /// <returns>A boolean value representing the conversion result.</returns>
    /// <exception cref="ArgumentException">Thrown when the specified value cannot be converted to a boolean value.</exception>
    public static bool ToBoolean(this string value, string trueCondition = "true", bool defaultvalue = false)
    {
        try
        {
            if (bool.TryParse(value, out bool result))
                return result;

            if (trueCondition.IsNotNullOrEmpty())
                return value.Equals(trueCondition, StringComparison.InvariantCultureIgnoreCase);
        }
        catch
        {
            throw new ArgumentException("Invalid boolean value");
        }

        return defaultvalue;
    }

    /// <summary>
    /// Converts the specified string representation of a number to its equivalent short integer. </summary> <param name="value">A string containing a number to convert.</param> <returns>The short integer equivalent to the value of the specified string.</returns>
    /// /
    public static short ToShort(this string value)
    {
        short result = 0;

        if (value.IsNotNullOrEmpty())
            _ = short.TryParse(value, out result);

        return result;
    }

    /// <summary>
    /// Converts a string to an integer.
    /// </summary>
    /// <param name="value">The string value to be converted.</param>
    /// <returns>The integer representation of the string. Returns 0 if the conversion fails.</returns>
    public static int ToInt(this string value)
    {
        int result = 0;

        if (value.IsNotNullOrEmpty())
            _ = int.TryParse(value, out result);

        return result;
    }

    /// <summary>
    /// Converts the specified string representation of a number to its equivalent long integer value.
    /// </summary>
    /// <param name="value">A string to convert.</param>
    /// <returns>
    /// A long integer that represents the converted numeric value, or zero if the conversion failed.
    /// </returns>
    public static long ToLong(this string value)
    {
        long result = 0;

        if (value.IsNotNullOrEmpty())
            _ = long.TryParse(value, out result);

        return result;
    }

    /// <summary>
    /// Converts a string to a DateTime value.
    /// </summary>
    /// <param name="value">The string value to convert.</param>
    /// <param name="cultureInfo">Optional. The culture-specific information used to interpret the string.</param>
    /// <param name="defaultValue">Optional. The default value to return if the conversion fails.</param>
    /// <returns>The DateTime value converted from the string, or the default value if the conversion fails.</returns>
    public static DateTime ToDateTime(this string value, CultureInfo cultureInfo = null, DateTime defaultValue = default)
    {
        if (value.IsNullOrEmpty())
            return defaultValue;

        cultureInfo ??= CultureInfo.DefaultThreadCurrentCulture;

        if (!DateTime.TryParse(value, cultureInfo, out DateTime datetimeValue))
            return defaultValue;

        return datetimeValue;
    }
}