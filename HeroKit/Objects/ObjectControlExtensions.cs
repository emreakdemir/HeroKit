namespace HeroKit.Objects;

/// <summary>
/// Provides extension methods for Object values.
/// </summary>
public static class ObjectControlExtensions
{
    /// <summary>
    /// Checks if an object is null.
    /// </summary>
    /// <param name="object">The object to check.</param>
    /// <returns>True if the object is null, false otherwise.</returns>
    public static bool IsNull(this object @object) => @object == null;

    /// <summary>
    /// Checks if an object is not null.
    /// </summary>
    /// <param name="object">The object to check.</param>
    /// <returns>True if the object is not null, false otherwise.</returns>
    public static bool IsNotNull(this object @object) => !@object.IsNull();

    /// <summary>
    /// Checks if an object is numeric.
    /// </summary>
    /// <param name="value">The object to check.</param>
    /// <returns>True if the object is numeric, false otherwise.</returns>
    public static bool IsNumeric(this object value)
    {
        return value switch
        {
            null                                      => false,
            int or long or float or double or decimal => true,
            string strValue => int.TryParse(strValue, out _)    ||
                               long.TryParse(strValue, out _)   ||
                               float.TryParse(strValue, out _)  ||
                               double.TryParse(strValue, out _) ||
                               decimal.TryParse(strValue, out _),
            _ => false,
        };
    }
}