using System.Globalization;
using HeroKit.Strings;
using HeroKit.Types;

namespace HeroKit.Objects;

/// <summary>
/// Provides extension methods for Object values.
/// </summary>
public static class ObjectConversionExtensions
{
    /// <summary>
    /// Converts an object to the specified type, with an optional default value.
    /// </summary>
    /// <typeparam name="T">The target type for conversion.</typeparam>
    /// <param name="source">The object to be converted.</param>
    /// <param name="defaultValue">The default value to return if conversion fails.</param>
    /// <returns>The converted value of type T or the default value if conversion fails.</returns>
    public static T ConvertTo<T>(this object source, T defaultValue = default)
    {
        if (source.IsNull() || source == DBNull.Value || (source is string s && s.IsNullOrEmpty()))
            return defaultValue;

        Type type = typeof(T).GetUnderlyingType();

        if (type.IsEnum)
            return source.ToString().ToEnum<T>();

        if (type == typeof(Guid))
        {
            if (source is string @string)
                source = new Guid(@string);
            if (source is byte[] bytes)
                source = new Guid(bytes);
        }

        return (T)Convert.ChangeType(source, type, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Tries to cast an object to the specified type.
    /// </summary>
    /// <typeparam name="T">The target type to cast to.</typeparam>
    /// <param name="source">The object to be casted.</param>
    /// <param name="result">The result of the cast operation if successful, or the default value of type T if unsuccessful.</param>
    /// <returns>True if the cast operation is successful, false otherwise.</returns>
    public static bool TryCast<T>(this object source, out T result)
    {
        try
        {
            result = source.ConvertTo<T>();
            return true;
        }
        catch
        {
            result = default(T);
            return false;
        }
    }
}