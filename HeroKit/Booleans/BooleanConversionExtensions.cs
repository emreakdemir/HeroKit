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
    public static string ToYesNo(this bool value) => value ? "Yes" : "No";

    /// <summary>
    /// Converts a nullable boolean value to a "Yes" or "No" string representation.
    /// </summary>
    /// <param name="value">The nullable boolean value to convert.</param>
    /// <returns>"Yes" if the value is not null and true, otherwise "No".</returns>
    public static string ToYesNo(this bool? value) => value.IsTrue() ? "Yes" : "No";
    
    /// <summary>
    /// Converts a boolean value to an integer representation (1 for true, 0 for false).
    /// </summary>
    /// <param name="value">The boolean value to convert.</param>
    /// <returns>1 if the value is true, 0 if the value is false.</returns>
    public static int ToInt(this bool value) => value ? 1 : 0;
}