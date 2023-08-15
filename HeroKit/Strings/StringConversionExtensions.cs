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

}