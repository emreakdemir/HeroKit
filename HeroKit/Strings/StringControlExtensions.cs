using HeroKit.Arrays;

namespace HeroKit.Strings;

/// <summary>
/// Provides extension methods for string value checking and manipulation.
/// </summary>
public static class StringControlExtensions
{
    /// <summary>
    /// Checks if the string is null or empty.
    /// </summary>
    /// <param name="value">The string to check.</param>
    /// <returns>True if the string is null or empty, false otherwise.</returns>
    public static bool IsNullOrEmpty(this string value) => string.IsNullOrEmpty(value);

    /// <summary>
    /// Checks if the string is null, empty, or consists only of the specified split value.
    /// </summary>
    /// <param name="source">The string to check.</param>
    /// <param name="splitValue">The character used for splitting.</param>
    /// <returns>True if the string is null, empty, or consists only of the split value, false otherwise.</returns>
    public static bool IsNullOrEmpty(this string source, char splitValue) => source.IsNullOrEmpty() || source.Split(splitValue).IsNullOrEmpty();

    /// <summary>
    /// Checks if the string is not null or empty.
    /// </summary>
    /// <param name="value">The string to check.</param>
    /// <returns>True if the string is not null or empty, false otherwise.</returns>
    public static bool IsNotNullOrEmpty(this string value) => !value.IsNullOrEmpty();

    /// <summary>
    /// Checks if the string is not null, empty, or consists only of the specified split value.
    /// </summary>
    /// <param name="source">The string to check.</param>
    /// <param name="splitValue">The character used for splitting.</param>
    /// <returns>True if the string is not null, empty, or consists only of the split value, false otherwise.</returns>
    public static bool IsNotNullOrEmpty(this string source, char splitValue) => !source.IsNullOrEmpty(splitValue);
}
