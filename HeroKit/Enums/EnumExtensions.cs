namespace HeroKit.Enums;

/// <summary>
/// Provides extension methods for working with enums.
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Checks if the specified value is defined in the given enumeration type.
    /// </summary>
    /// <typeparam name="T">The enum type to be checked.</typeparam>
    /// <param name="value">The value to be checked.</param>
    /// <returns>True if the value is defined in the enum type; otherwise, false.</returns>
    public static bool IsDefined<T>(this T value) where T : struct, IConvertible => Enum.IsDefined(typeof(T), value);

    /// <summary>
    /// Converts an enum value to its equivalent byte representation.
    /// </summary>
    /// <param name="value">The enum value to be converted.</param>
    /// <returns>The byte representation of the enum value.</returns>
    /// <remarks>
    /// This method is an extension method that can be called on any enum value.
    /// </remarks>
    public static byte ToByte(this Enum value) => Convert.ToByte(value);

    /// <summary>
    /// Gets the string representation of the specified enum value.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <param name="value">The enum value.</param>
    /// <returns>The string representation of the enum value.</returns>
    public static string GetValue<T>(this Enum value) => Convert.ChangeType(value, typeof(T)).ToString();

    /// <summary>
    /// Returns a list of all values defined in the given enumeration type.
    /// </summary>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <returns>A list of all values defined in the enum type.</returns>
    public static List<TEnum> ToList<TEnum>()
    {
        var type = typeof(TEnum);

        if (type.BaseType == typeof(Enum))
            throw new ArgumentException("T must be type of System.Enum");

        var values = Enum.GetValues(type);

        return values.Length switch
        {
            > 0 => values.Cast<TEnum>().ToList(),
            _ => []
        };
    }
}