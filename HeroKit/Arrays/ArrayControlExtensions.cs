namespace HeroKit.Arrays;

/// <summary>
/// Provides extension methods for Array value checking
/// </summary>
public static class ArrayControlExtensions
{
    /// <summary>
    /// Checks if an array is null or empty.
    /// </summary>
    /// <typeparam name="T">The type of items in the array.</typeparam>
    /// <param name="array">The array to check.</param>
    /// <returns>True if the array is null or empty, false otherwise.</returns>
    public static bool IsNullOrEmpty<T>(this T[] array) => array is not { Length: > 0 };

    /// <summary>
    /// Checks if an array is not null and not empty.
    /// </summary>
    /// <typeparam name="T">The type of items in the array.</typeparam>
    /// <param name="array">The array to check.</param>
    /// <returns>True if the array is not null and not empty, false otherwise.</returns>
    public static bool IsNotNullOrEmpty<T>(this T[] array) => !array.IsNullOrEmpty();
}