using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using HeroKit.Types;

namespace HeroKit.Objects;

/// <summary>
/// Converts objects to other types.
/// </summary>
/// <remarks>
/// <see cref="ConvertTo{T}"/> is lenient and returns a default value; <see cref="TryConvertTo{T}"/> reports success. Neither throws for invalid input.
/// </remarks>
public static class ObjectConversionExtensions
{
    /// <summary>
    /// Converts an object to the specified type, or returns a default value when it cannot.
    /// </summary>
    /// <typeparam name="T">The target type. Nullable value types and enums are supported.</typeparam>
    /// <param name="value">The object to convert.</param>
    /// <param name="defaultValue">The value returned when <paramref name="value"/> is null, <see cref="DBNull"/>, empty or not convertible.</param>
    /// <returns>The converted value, or <paramref name="defaultValue"/>.</returns>
    /// <example>
    /// <code>
    /// ((object)"42").ConvertTo&lt;int&gt;();      // 42
    /// ((object)"abc").ConvertTo(-1);         // -1
    /// </code>
    /// </example>
    public static T? ConvertTo<T>(this object? value, T? defaultValue = default)
        => value.TryConvertTo(out T? result) ? result : defaultValue;

    /// <summary>
    /// Tries to convert an object to the specified type.
    /// </summary>
    /// <typeparam name="T">The target type. Nullable value types and enums are supported.</typeparam>
    /// <param name="value">The object to convert.</param>
    /// <param name="result">The converted value when the method returns true; otherwise the default value.</param>
    /// <returns>True if the value was converted; false if it is null, <see cref="DBNull"/>, empty or not convertible.</returns>
    /// <example>
    /// <code>
    /// if (row["Count"].TryConvertTo(out int count)) { /* ... */ }
    /// </code>
    /// </example>
    public static bool TryConvertTo<T>(this object? value, [NotNullWhen(true)] out T? result)
    {
        if (value is T same)
        {
            result = same;
            return true;
        }

        if (TryConvert(value, typeof(T), out object? converted))
        {
            result = (T)converted;
            return true;
        }

        result = default;
        return false;
    }

    // Non-generic core, also used by PropertyInfoExtensions without reflection over generic methods.
    internal static bool TryConvert(object? value, Type targetType, [NotNullWhen(true)] out object? result)
    {
        result = null;

        if (value is null || value == DBNull.Value || (value is string text && string.IsNullOrWhiteSpace(text)))
            return false;

        Type type = targetType.GetUnderlyingType();

        if (type.IsInstanceOfType(value))
        {
            result = value;
            return true;
        }

        try
        {
            result = type switch
            {
                { IsEnum: true } => Enum.TryParse(type, Convert.ToString(value, CultureInfo.InvariantCulture), true, out object? parsed) && Enum.IsDefined(type, parsed) ? parsed : null,
                _ when type == typeof(Guid) => value switch
                {
                    string guidText when Guid.TryParse(guidText, out Guid guid) => guid,
                    byte[] { Length: 16 } bytes => new Guid(bytes),
                    _ => null,
                },
                _ => Convert.ChangeType(value, type, CultureInfo.InvariantCulture),
            };

            return result is not null;
        }
        catch (Exception exception) when (exception is FormatException or InvalidCastException or OverflowException or ArgumentException)
        {
            result = null;
            return false;
        }
    }
}
