using System.Globalization;

namespace HeroKit.Integers;

/// <summary>
/// Provides extension methods for integers.
/// </summary>
public static class IntegerExtensions
{
    /// <summary>
    /// Returns half of the given integer, rounded toward zero.
    /// </summary>
    /// <param name="value">The integer to halve.</param>
    /// <returns><paramref name="value"/> divided by 2.</returns>
    public static int Half(this int value) => value / 2;

    /// <summary>
    /// Returns the cube of the given integer.
    /// </summary>
    /// <param name="value">The integer to cube.</param>
    /// <returns><paramref name="value"/> to the power of 3.</returns>
    public static int Cube(this int value) => value * value * value;

    /// <summary>
    /// Returns the square of the given integer.
    /// </summary>
    /// <param name="value">The integer to square.</param>
    /// <returns><paramref name="value"/> multiplied by itself.</returns>
    public static int Square(this int value) => value * value;

    /// <summary>
    /// Determines whether the given number is even.
    /// </summary>
    /// <param name="value">The number to check.</param>
    /// <returns>True if the number is even; otherwise false.</returns>
    public static bool IsEven(this int value) => value % 2 == 0;

    /// <summary>
    /// Determines whether the given number is odd. Works for negative numbers.
    /// </summary>
    /// <param name="value">The number to check.</param>
    /// <returns>True if the number is odd; otherwise false.</returns>
    public static bool IsOdd(this int value) => value % 2 != 0;

    /// <summary>
    /// Returns the name of the month with the given number.
    /// </summary>
    /// <param name="value">The month number (1-12).</param>
    /// <param name="culture">The culture for the month name. Defaults to <see cref="CultureInfo.CurrentCulture"/>, because the result is meant for display.</param>
    /// <returns>The month name, or an empty string if the value is out of range.</returns>
    /// <example>
    /// <code>
    /// 3.ToMonthName(CultureInfo.GetCultureInfo("tr-TR")); // "Mart"
    /// </code>
    /// </example>
    public static string ToMonthName(this int value, CultureInfo? culture = null)
    {
        culture ??= CultureInfo.CurrentCulture;

        return value is >= 1 and <= 12 ? culture.DateTimeFormat.GetMonthName(value) : string.Empty;
    }
}
