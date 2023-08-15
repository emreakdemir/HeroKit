using HeroKit.Objects;

namespace HeroKit.Arrays;

/// <summary>
/// Provides extension methods for Array value conversions
/// </summary>
public static class ArrayConversionExtensions
{
    /// <summary>
    /// Converts an array of items to a list using a mapping function.
    /// </summary>
    /// <typeparam name="T">The type of items in the array and list.</typeparam>
    /// <param name="items">The array of items to convert.</param>
    /// <param name="mapFunction">The mapping function to apply on each item.</param>
    /// <returns>A list of converted items.</returns>
    public static List<T> ToList<T>(this T[] items, Func<object, T> mapFunction)
    {
        if (items.IsNullOrEmpty() || mapFunction.IsNull())
            return new List<T>();

        return items.Cast<object>().Select((_, i) => mapFunction(items.GetValue(i)))
                    .Where(val => val.IsNotNull())
                    .ToList();
    }

    /// <summary>
    /// Encodes a byte array into a URL-safe string.
    /// </summary>
    /// <param name="array">The byte array to encode.</param>
    /// <returns>The URL-safe encoded string.</returns>
    public static string UrlTokenEncode(this byte[] array)
    {
        return array.IsNullOrEmpty() ? string.Empty : System.Web.HttpUtility.UrlEncode(array);
    }
}