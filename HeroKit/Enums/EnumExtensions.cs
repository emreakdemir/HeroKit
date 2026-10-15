using System.Globalization;

namespace HeroKit.Enums;

/// <summary>
/// Provides extension methods for enums.
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Checks whether a value is defined in its enum type.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <param name="value">The value to check.</param>
    /// <returns>True if the value is defined; otherwise false.</returns>
    public static bool IsDefined<T>(this T value) where T : struct, Enum => Enum.IsDefined(value);

    /// <summary>
    /// Converts an enum value to a byte.
    /// </summary>
    /// <param name="value">The enum value to convert.</param>
    /// <returns>The underlying value as a byte.</returns>
    /// <exception cref="OverflowException">The underlying value does not fit in a byte.</exception>
    public static byte ToByte(this Enum value) => Convert.ToByte(value, CultureInfo.InvariantCulture);

    /// <summary>
    /// Converts an enum value to its underlying numeric value in the requested type.
    /// </summary>
    /// <typeparam name="T">The numeric type, for example int or long.</typeparam>
    /// <param name="value">The enum value.</param>
    /// <returns>The underlying value.</returns>
    /// <example>
    /// <code>
    /// DayOfWeek.Friday.ToUnderlying&lt;int&gt;(); // 5
    /// </code>
    /// </example>
    public static T ToUnderlying<T>(this Enum value) where T : struct, IConvertible =>
        (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);

    /// <summary>
    /// Returns all values defined in an enum type.
    /// </summary>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <returns>A list of all defined values in declaration order.</returns>
    /// <example>
    /// <code>
    /// EnumExtensions.ToList&lt;DayOfWeek&gt;(); // Sunday … Saturday
    /// </code>
    /// </example>
    public static List<TEnum> ToList<TEnum>() where TEnum : struct, Enum => [.. Enum.GetValues<TEnum>()];
}
