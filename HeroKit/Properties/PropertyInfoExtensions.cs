using System.Reflection;
using HeroKit.Objects;
using HeroKit.Types;

namespace HeroKit.Properties;

/// <summary>
/// Provides helpers for reading and setting properties through reflection.
/// </summary>
public static class PropertyInfoExtensions
{
    /// <summary>
    /// Checks whether a value can be assigned to the property without conversion.
    /// </summary>
    /// <param name="property">The property.</param>
    /// <param name="value">The value to check. Null is assignable to reference types and nullable value types.</param>
    /// <returns>True if the value can be assigned; otherwise false.</returns>
    public static bool CanAssignValue(this PropertyInfo property, object? value)
    {
        ArgumentNullException.ThrowIfNull(property);

        return value is null
            ? !property.PropertyType.IsValueType || property.PropertyType.IsNullable()
            : property.PropertyType.IsInstanceOfType(value);
    }

    /// <summary>
    /// Checks whether the property type is a nullable value type, such as <c>int?</c>.
    /// </summary>
    /// <param name="property">The property.</param>
    /// <returns>True if the property type is <see cref="Nullable{T}"/>; otherwise false.</returns>
    public static bool IsNullable(this PropertyInfo property) => property.PropertyType.IsNullable();

    /// <summary>
    /// Converts a value to the property type and sets it on an instance.
    /// </summary>
    /// <remarks>
    /// Conversion follows <see cref="ObjectConversionExtensions.TryConvertTo{T}"/>. Null is set only on properties that accept null.
    /// </remarks>
    /// <param name="property">The property to set.</param>
    /// <param name="instance">The instance that owns the property, or null for a static property.</param>
    /// <param name="value">The value to convert and set.</param>
    /// <returns>True if the value was converted and set; otherwise false.</returns>
    /// <example>
    /// <code>
    /// typeof(Order).GetProperty("Quantity")!.TrySetConvertedValue(order, "42"); // order.Quantity == 42
    /// </code>
    /// </example>
    public static bool TrySetConvertedValue(this PropertyInfo property, object? instance, object? value)
    {
        ArgumentNullException.ThrowIfNull(property);

        if (value is null || value == DBNull.Value)
        {
            if (!property.CanAssignValue(null))
                return false;

            property.SetValue(instance, null);
            return true;
        }

        if (!ObjectConversionExtensions.TryConvert(value, property.PropertyType, out object? converted))
            return false;

        property.SetValue(instance, converted);
        return true;
    }
}
