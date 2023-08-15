namespace HeroKit.Arrays;

/// <summary>
/// Provides extension methods for Array value manipulation.
/// </summary>
public static class ArrayManipulationExtensions
{
    /// <summary>
    /// Inserts items from an enumerable into an array.
    /// </summary>
    /// <typeparam name="T">The type of items in the array.</typeparam>
    /// <param name="array">The array to insert items into.</param>
    /// <param name="items">The enumerable containing items to insert.</param>
    public static void InsertItems<T>(this T[] array, IEnumerable<T> items)
    {
        int index = 0;
        foreach (T item in items)
        {
            if (index >= array.Length)
                break;

            array[index] = item;
            index++;
        }
    }
}