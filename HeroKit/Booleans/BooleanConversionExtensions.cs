namespace HeroKit.Booleans;

/// <summary>
/// Provides extension methods for bool value manipulation.
/// </summary>
public static class BooleanConversionExtensions
{
    /// <summary>
    /// Converts a boolean value to a "Yes" or "No" string representation.
    /// </summary>
    /// <param name="value">The boolean value to convert.</param>
    /// <returns>"Yes" if the value is true, otherwise "No".</returns>
    public static string ToYesNo(this bool value) => value.ToString("Yes", "No");

    /// <summary>
    /// Converts a nullable boolean value to a "Yes" or "No" string representation.
    /// </summary>
    /// <param name="value">The nullable boolean value to convert.</param>
    /// <returns>"Yes" if the value is not null and true, otherwise "No".</returns>
    public static string ToYesNo(this bool? value) => value.ToString("Yes", "No", "No");

    /// <summary>
    /// Converts a boolean value to an integer representation (1 for true, 0 for false).
    /// </summary>
    /// <param name="value">The boolean value to convert.</param>
    /// <returns>1 if the value is true, 0 if the value is false.</returns>
    public static int ToInt(this bool value) => value ? 1 : 0;

    /// <summary>
    /// Converts a boolean value to given string equivalents.
    /// </summary>
    /// <param name="value">The boolean value to convert.</param>
    /// <param name="trueText">String for true value.</param>
    /// <param name="falseText">String for false value.</param>
    /// <returns>If the value is true returns "trueText", otherwise "falseText".</returns>
    public static string ToString(this bool value, string trueText, string falseText) => value ? trueText : falseText;

    /// <summary>
    /// Converts a boolean value to given string equivalents.
    /// </summary>
    /// <param name="value">The nullable boolean value to convert.</param>
    /// <param name="trueText">String for true value.</param>
    /// <param name="falseText">String for false value.</param>
    /// <param name="nullText">String for null value.</param>
    /// <returns>If the value is null returns "nullText", when is true returns "trueText", otherwise "falseText".</returns>
    public static string ToString(this bool? value, string trueText, string falseText, string nullText = "-") => !value.HasValue ? nullText : value.Value.ToString(trueText, falseText);
}