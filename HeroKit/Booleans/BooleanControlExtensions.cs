namespace HeroKit.Booleans;

/// <summary>
/// Provides extension methods for `bool` value checking and manipulation.
/// </summary>
public static class BooleanControlExtensions
{
    /// <summary>
    /// Checks whether a nullable boolean value is true.
    /// </summary>
    /// <param name="value">The nullable boolean value.</param>
    /// <returns>True if the value is not null and true, otherwise false.</returns>
    public static bool IsTrue(this bool? value) => value ?? false;

    /// <summary>
    /// Checks whether a nullable boolean value is false.
    /// </summary>
    /// <param name="value">The nullable boolean value.</param>
    /// <returns>True if the value is not null and false, otherwise false.</returns>
    public static bool IsFalse(this bool? value) => !value.IsTrue();

    /// <summary>
    /// Returns true if all boolean values in the collection are true.
    /// </summary>
    /// <param name="values">The collection of boolean values.</param>
    /// <returns>True if all values are true, otherwise false.</returns>
    public static bool AllTrue(this IEnumerable<bool> values) => values.All(value => value);

    /// <summary>
    /// Returns true if any boolean value in the collection is true.
    /// </summary>
    /// <param name="values">The collection of boolean values.</param>
    /// <returns>True if at least one value is true, otherwise false.</returns>
    public static bool AnyTrue(this IEnumerable<bool> values) => values.Any(value => value);

    /// <summary>
    /// Returns true if all boolean values in the collection are false.
    /// </summary>
    /// <param name="values">The collection of boolean values.</param>
    /// <returns>True if all values are false, otherwise false.</returns>
    public static bool AllFalse(this IEnumerable<bool> values) => values.All(value => !value);

    /// <summary>
    /// Returns true if any boolean value in the collection is false.
    /// </summary>
    /// <param name="values">The collection of boolean values.</param>
    /// <returns>True if at least one value is false, otherwise true.</returns>
    public static bool AnyFalse(this IEnumerable<bool> values) => values.Any(value => !value);
}