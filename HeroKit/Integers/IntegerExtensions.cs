using System.Globalization;
using HeroKit.Objects;

namespace HeroKit.Integers;

/// <summary>
/// Provides extension methods for integers.
/// </summary>
public static class IntegerExtensions
{
    /// <summary>
    /// Returns half of the given integer.
    /// </summary>
    /// <param name="source">The integer to halve.</param>
    /// <returns>The result of dividing the given integer by 2.</returns>
    public static int Half(this int source) => source / 2;

    /// <summary>
    /// Returns the cube of the given integer.
    /// </summary>
    /// <param name="source">The integer to cube.</param>
    /// <returns>The result of raising the given integer to the power of 3.</returns>
    public static int Cube(this int source) => (int)Math.Pow(source, 3);

    /// <summary>
    /// Returns the square of the given integer.
    /// </summary>
    /// <param name="source">The integer to square.</param>
    /// <returns>The result of multiplying the given integer by itself.</returns>
    public static int Square(this int source) => (int)Math.Pow(source, 2);

    /// <summary>
    /// Determines whether the given number is an even number.
    /// </summary>
    /// <param name="number">The number to check.</param>
    /// <returns>true if the number is even, false otherwise.</returns>
    public static bool IsEvenNumber(this int number) => ((number % 2) == 0);

    /// <summary>
    /// Determines whether the given number is an odd number.
    /// </summary>
    /// <param name="number">The number to check.</param>
    /// <returns>true if the number is odd, false otherwise.</returns>
    public static bool IsOddNumber(this int number) => ((number % 2) == 1);

    /// <summary>
    /// Returns the month name of the given integer value.
    /// </summary>
    /// <param name="value">The integer value representing a month (1-12).</param>
    /// <param name="cultureInfo">The culture information to use for month names (optional).</param>
    /// <returns>The month name corresponding to the given value, or an empty string if the value is out of range.</returns>
    public static string ToMonthString(this int value, CultureInfo cultureInfo = null)
    {
        if (cultureInfo.IsNull())
        {
            cultureInfo = CultureInfo.CurrentCulture;
        }
        
        return value switch
        {
            >= 1 and <= 12 => cultureInfo.DateTimeFormat.GetMonthName(value),
            _ => ""
        };
    }

    /// <summary>
    /// Converts the given integer point value to pixel value.
    /// </summary>
    /// <param name="point">The integer point value.</param>
    /// <returns>The pixel value after conversion.</returns>
    public static decimal ToPixel(this int point) => decimal.Truncate(point * 4 / 5) + 1;

    /// <summary>
    /// Returns the given number of seconds as milliseconds.
    /// </summary>
    /// <param name="source">The number of seconds.</param>
    /// <returns>The number of seconds converted to milliseconds.</returns>
    public static int Seconds(this int source) => source * 1000;
}