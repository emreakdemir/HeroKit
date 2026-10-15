namespace HeroKit.Booleans;

/// <summary>
/// Provides extension methods for converting boolean values.
/// </summary>
public static class BooleanConversionExtensions
{
    /// <summary>
    /// Converts a boolean to "Yes" or "No".
    /// </summary>
    /// <param name="value">The boolean value to convert.</param>
    /// <returns>"Yes" for true; "No" for false.</returns>
    public static string ToYesNo(this bool value) => value.ToText("Yes", "No");

    /// <summary>
    /// Converts a nullable boolean to "Yes" or "No". Null is "No".
    /// </summary>
    /// <param name="value">The nullable boolean value to convert.</param>
    /// <returns>"Yes" for true; "No" for false or null.</returns>
    public static string ToYesNo(this bool? value) => value.ToText("Yes", "No", "No");

    /// <summary>
    /// Converts a boolean to 1 or 0.
    /// </summary>
    /// <param name="value">The boolean value to convert.</param>
    /// <returns>1 for true; 0 for false.</returns>
    public static int ToInt(this bool value) => value ? 1 : 0;

    /// <summary>
    /// Converts a boolean to one of two texts.
    /// </summary>
    /// <param name="value">The boolean value to convert.</param>
    /// <param name="trueText">The text for true.</param>
    /// <param name="falseText">The text for false.</param>
    /// <returns><paramref name="trueText"/> or <paramref name="falseText"/>.</returns>
    /// <example>
    /// <code>
    /// isActive.ToText("Aktif", "Pasif");
    /// </code>
    /// </example>
    public static string ToText(this bool value, string trueText, string falseText) => value ? trueText : falseText;

    /// <summary>
    /// Converts a nullable boolean to one of three texts.
    /// </summary>
    /// <param name="value">The nullable boolean value to convert.</param>
    /// <param name="trueText">The text for true.</param>
    /// <param name="falseText">The text for false.</param>
    /// <param name="nullText">The text for null. Defaults to "-".</param>
    /// <returns><paramref name="trueText"/>, <paramref name="falseText"/> or <paramref name="nullText"/>.</returns>
    public static string ToText(this bool? value, string trueText, string falseText, string nullText = "-") =>
        value.HasValue ? value.Value.ToText(trueText, falseText) : nullText;
}
