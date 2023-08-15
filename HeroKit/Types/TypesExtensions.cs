using System.Reflection;

namespace HeroKit.Types;

/// <summary>
/// Provides extension methods for Type values.
/// </summary>
public static class TypesExtensions
{
    /// <summary>
    /// Retrieves the default value for a given type.
    /// </summary>
    /// <param name="type">The type for which to retrieve the default value.</param>
    /// <returns>The default value of the specified type, or null if not applicable.</returns>
    public static object GetDefault(this Type type)
    {
        if (type is not { IsValueType: true } || type == typeof(void))
            return null;

        string message;
        if (type.ContainsGenericParameters)
        {
            message = $"{{ {MethodBase.GetCurrentMethod()} }}" +
                      "Error:"                                 +
                      "\n\n"                                   +
                      $"The supplied value type <{type}> contains generic parameters, so the default value cannot be retrieved";

            throw new ArgumentException(message);
        }

        if (type.IsPrimitive || !type.IsNotPublic)
        {
            try
            {
                return Activator.CreateInstance(type);
            }
            catch (Exception e)
            {
                message = $"{{ {MethodBase.GetCurrentMethod()} }} Error:"                                                                 +
                          "\n\n"                                                                                                          +
                          $"The Activator.CreateInstance method could not create a default instance of the supplied value type <{type}> " +
                          $"(Inner Exception message: \"{e.Message}\")";

                throw new ArgumentException(message, e);
            }
        }

        message = $"{{ {MethodBase.GetCurrentMethod()} }} " +
                  "Error:"                                  +
                  "\n\n"                                    +
                  $"The supplied value type <{type}> is not a publicly-visible type, so the default value cannot be retrieved";

        throw new ArgumentException(message);
    }

    /// <summary>
    /// Gets the default value for a specified type.
    /// </summary>
    /// <typeparam name="T">The type for which to retrieve the default value.</typeparam>
    /// <returns>The default value of the specified type.</returns>
    public static T GetDefault<T>()
    {
        Type type = typeof(T).GetUnderlyingType();

        return type.IsValueType ? (T)Activator.CreateInstance(type) : default(T);
    }

    /// <summary>
    /// Gets the concrete implementation type for a given interface or abstract type.
    /// </summary>
    /// <param name="type">The interface or abstract type to get the implementation for.</param>
    /// <returns>The concrete implementation type, or null if not found.</returns>
    public static Type GetRealType(this Type type)
    {
        return Assembly.GetAssembly(type)?
                       .GetExportedTypes()
                       .Where(type.IsAssignableFrom)
                       .FirstOrDefault(t => !t.IsAbstract &&
                                            !t.IsInterface);
    }

    /// <summary>
    /// Gets extension methods for the specified type from a given assembly.
    /// </summary>
    /// <param name="type">The type to get extension methods for.</param>
    /// <param name="extensionsAssembly">The assembly containing extension methods.</param>
    /// <param name="bindingFlags">Binding flags to control the method visibility.</param>
    /// <returns>An enumerable collection of extension methods for the specified type.</returns>
    public static IEnumerable<MethodInfo> GetExtensionMethods(this Type type, Assembly extensionsAssembly, BindingFlags bindingFlags = BindingFlags.Static |
                                                                                                                                       BindingFlags.Public)
    {
        IEnumerable<MethodInfo> methods = extensionsAssembly.GetTypes()
                                                            .Where(t => !t.IsGenericType && !t.IsNested)
                                                            .SelectMany(t => t.GetMethods(bindingFlags), (t, m) => new
                                                             {
                                                                 t,
                                                                 m,
                                                             })
                                                            .Where(t1 => t1.m.IsDefined(typeof(System.Runtime.CompilerServices.ExtensionAttribute), false))
                                                            .Where(t1 => t1.m.GetParameters()[0].ParameterType == type)
                                                            .Select(t1 => t1.m);

        return methods;
    }

    /// <summary>
    /// Gets a specific extension method for the specified type from a given assembly.
    /// </summary>
    /// <param name="type">The type to get the extension method for.</param>
    /// <param name="extensionsAssembly">The assembly containing extension methods.</param>
    /// <param name="name">The name of the extension method.</param>
    /// <returns>The MethodInfo of the specified extension method, or null if not found.</returns>
    public static MethodInfo GetExtensionMethod(this Type type, Assembly extensionsAssembly, string name)
    {
        return type.GetExtensionMethods(extensionsAssembly).FirstOrDefault(m => m.Name == name);
    }

    /// <summary>
    /// Gets a specific extension method for the specified type from a collection of assemblies.
    /// </summary>
    /// <param name="type">The type to get the extension method for.</param>
    /// <param name="assemblies">The collection of assemblies containing extension methods.</param>
    /// <param name="name">The name of the extension method.</param>
    /// <returns>The MethodInfo of the specified extension method, or null if not found.</returns>
    public static MethodInfo GetExtensionMethod(this Type type, IEnumerable<Assembly> assemblies, string name)
    {
        return assemblies.Select(assembly => type.GetExtensionMethods(assembly)
                                                 .FirstOrDefault(m => m.Name == name))
                         .FirstOrDefault(method => method != null);
    }

    /// <summary>
    /// Gets a specific extension method with the given name and parameter types for the specified type from an assembly.
    /// </summary>
    /// <param name="type">The type to get the extension method for.</param>
    /// <param name="extensionsAssembly">The assembly containing extension methods.</param>
    /// <param name="name">The name of the extension method.</param>
    /// <param name="types">An array of parameter types for the extension method.</param>
    /// <returns>The MethodInfo of the specified extension method, or null if not found.</returns>
    public static MethodInfo GetExtensionMethod(this Type type, Assembly extensionsAssembly, string name, Type[] types)
    {
        List<MethodInfo> methods = (type.GetExtensionMethods(extensionsAssembly)
                                        .Where(m => m.Name                   == name &&
                                                    m.GetParameters().Length == types.Length + 1)).ToList();

        if (!methods.Any())
            return default;

        if (methods.Count == 1)
            return methods.FirstOrDefault();

        foreach (MethodInfo methodInfo in methods)
        {
            ParameterInfo[] parameters = methodInfo.GetParameters();

            bool found = true;
            for (byte b = 0; b < types.Length; b++)
            {
                found = parameters[b].GetType() == types[b];
            }

            if (found)
                return methodInfo;
        }

        return default;
    }
}