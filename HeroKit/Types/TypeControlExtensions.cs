using HeroKit.Objects;

namespace HeroKit.Types;

/// <summary>
/// Provides extension methods for Type values.
/// </summary>
public static class TypeControlExtensions
{
    /// <summary>
    /// Checks if a type is a Nullable type (e.g., int?, double?).
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <returns>True if the type is nullable, false otherwise.</returns>
    public static bool IsNullable(this Type type)
    {
        if (type.IsNull())
            throw new ArgumentNullException(nameof(type));

        return type.IsGenericType                                    &&
               type.GetGenericTypeDefinition() == typeof(Nullable<>) &&
               Nullable.GetUnderlyingType(type).IsNotNull();
    }

    /// <summary>
    /// Checks if a generic type is assignable from another type and provides generic arguments if applicable.
    /// </summary>
    /// <param name="toType">The target generic type.</param>
    /// <param name="fromType">The source type.</param>
    /// <param name="genericArguments">The generic arguments if assignment is possible, otherwise null.</param>
    /// <returns>True if assignment is possible, false otherwise.</returns>
    public static bool IsGenericAssignableFrom(this Type toType, Type fromType, out Type[] genericArguments)
    {
        if (toType.IsNull())
            throw new ArgumentNullException(nameof(toType), "is null");

        if (fromType.IsNull())
            throw new ArgumentNullException(nameof(fromType), "is null");

        genericArguments = null;

        if (!toType.IsGenericTypeDefinition || fromType.IsGenericTypeDefinition)
        {
            return false;
        }

        if (toType.IsInterface)
        {
            return IsGenericAssignableFromControlForInterface(toType, fromType, ref genericArguments);
        }

        while (fromType != null)
        {
            if (fromType.IsGenericType && fromType.GetGenericTypeDefinition() == toType)
            {
                genericArguments = fromType.GetGenericArguments();

                return true;
            }
            fromType = fromType.BaseType;
        }

        return false;
    }

    private static bool IsGenericAssignableFromControlForInterface(Type toType, Type fromType, ref Type[] genericArguments)
    {
        foreach (Type interfaceCandidate in fromType.GetInterfaces())
        {
            if (interfaceCandidate.IsGenericType && interfaceCandidate.GetGenericTypeDefinition() == toType)
            {
                genericArguments = interfaceCandidate.GetGenericArguments();

                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Gets the underlying non-nullable type of a nullable type (if applicable).
    /// If the input type is not nullable, the same type is returned.
    /// </summary>
    /// <param name="type">The type to examine.</param>
    /// <returns>The underlying non-nullable type or the same type if not nullable.</returns>
    public static Type GetUnderlyingType(this Type type)
    {
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            return Nullable.GetUnderlyingType(type) ?? type;

        return type;
    }

    /// <summary>
    /// Checks if the provided value is the default value for its type.
    /// </summary>
    /// <typeparam name="T">The type of the value to check.</typeparam>
    /// <param name="value">The value to check for default value.</param>
    /// <returns>True if the value is the default value, false otherwise.</returns>
    public static bool IsDefaultValue<T>(this T value) => EqualityComparer<T>.Default.Equals(value, default);
}