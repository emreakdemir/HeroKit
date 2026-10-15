using System.Reflection;
using HeroKit.Objects;
using HeroKit.Types;

namespace HeroKit.Properties;

/// <summary>
/// Provides extension methods for PropertyInfo values.
/// </summary>
public static class PropertyInfoExtensions
{
    /// <summary>
    /// Determines if a value can be assigned to a PropertyInfo.
    /// </summary>
    /// <param name="property">The PropertyInfo to check.</param>
    /// <param name="value">The value to be assigned.</param>
    /// <returns>True if the value can be assigned, false otherwise.</returns>
    public static bool CanAssignValue(this PropertyInfo property, object? value)
    {
        return value.IsNull() ? property.IsNullable() : property.PropertyType.IsInstanceOfType(value);
    }

    /// <summary>
    /// Determines if the PropertyInfo is nullable.
    /// </summary>
    /// <param name="property">The PropertyInfo to check.</param>
    /// <returns>True if the PropertyInfo is nullable, false otherwise.</returns>
    public static bool IsNullable(this PropertyInfo property) => property.PropertyType.IsNullable();

    /// <summary>
    /// Converts a value to the property type and sets it on an instance.
    /// </summary>
    /// <typeparam name="T">The type of the instance.</typeparam>
    /// <param name="property">The property to set.</param>
    /// <param name="instance">The instance that owns the property.</param>
    /// <param name="value">The value to convert and set.</param>
    /// <returns>True if the value was converted and set; false if it could not be assigned.</returns>
    public static bool TrySetConvertedValue<T>(this PropertyInfo property, T instance, object? value)
    {
        var constructedMethod = typeof(ObjectConversionExtensions).GetMethod(nameof(ObjectConversionExtensions.TryCast))!
            .MakeGenericMethod(property.PropertyType);

        object? valueSet = null;
        var parameters = new object?[] { value, null };

        if (Convert.ToBoolean(constructedMethod.Invoke(null, parameters)))
            valueSet = parameters[1];

        if (!property.CanAssignValue(valueSet))
            return false;

        property.SetValue(instance, valueSet, null);
        return true;
    }

    /// <summary>
    /// Sets the value of a PropertyInfo on an instance of a class.
    /// </summary>
    /// <remarks>
    /// In instance syntax the built-in <see cref="PropertyInfo.SetValue(object, object)"/> hides this method.
    /// Use <see cref="TrySetConvertedValue{T}"/> instead.
    /// </remarks>
    /// <typeparam name="T">The type of the instance.</typeparam>
    /// <param name="property">The PropertyInfo to set.</param>
    /// <param name="instance">The instance of the class.</param>
    /// <param name="value">The value to be set.</param>
    [Obsolete("PropertyInfo.SetValue(object, object) hides this method in instance syntax. Use TrySetConvertedValue instead. This method will be removed in 2.0.0.")]
    public static void SetValue<T>(this PropertyInfo property, T instance, object? value) => property.TrySetConvertedValue(instance, value);
}