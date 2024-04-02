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
    public static bool CanAssignValue(this PropertyInfo property, object value)
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
    /// Sets the value of a PropertyInfo on an instance of a class.
    /// </summary>
    /// <typeparam name="T">The type of the instance.</typeparam>
    /// <param name="property">The PropertyInfo to set.</param>
    /// <param name="instance">The instance of the class.</param>
    /// <param name="value">The value to be set.</param>
    public static void SetValue<T>(this PropertyInfo property, T instance, object value)
    {
        var constructedMethod = typeof(ObjectConversionExtensions).GetMethod("TryCast")!
            .MakeGenericMethod(property.PropertyType);

        object valueSet = null;
        var parameters = new[] { value, null };

        if (Convert.ToBoolean(constructedMethod.Invoke(null, parameters)))
            valueSet = parameters[1];

        if (!property.CanAssignValue(valueSet))
            return;
        property.SetValue(instance, valueSet, null);
    }
}