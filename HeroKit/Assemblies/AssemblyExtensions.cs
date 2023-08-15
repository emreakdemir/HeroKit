using System.Reflection;
using HeroKit.Arrays;

namespace HeroKit.Assemblies;

/// <summary>
/// Provides extension methods for Assembly values.
/// </summary>
public static class AssemblyExtensions
{
    /// <summary>
    /// Gets the specified attribute from the calling assembly.
    /// </summary>
    /// <typeparam name="T">The type of attribute to retrieve.</typeparam>
    /// <param name="callingAssembly">The calling assembly.</param>
    /// <returns>The attribute of type T if found, or null if not found.</returns>
    public static T GetAttribute<T>(this Assembly callingAssembly)
            where T : Attribute
    {
        T result = null;

        Attribute[] configAttributes = Attribute.GetCustomAttributes(callingAssembly, typeof(T), false);

        if (!configAttributes.IsNullOrEmpty())
            result = (T)configAttributes[0];

        return result;
    }
}