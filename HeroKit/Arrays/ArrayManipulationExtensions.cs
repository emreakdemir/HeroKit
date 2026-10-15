namespace HeroKit.Arrays;

/// <summary>
/// Provides extension methods for changing arrays.
/// </summary>
public static class ArrayManipulationExtensions
{
    /// <summary>
    /// Copies items into the array starting at index 0. Copying stops when the array is full.
    /// </summary>
    /// <typeparam name="T">The element type.</typeparam>
    /// <param name="array">The array to write to.</param>
    /// <param name="items">The items to copy.</param>
    /// <returns>The number of items copied.</returns>
    public static int CopyFrom<T>(this T[] array, IEnumerable<T> items)
    {
        ArgumentNullException.ThrowIfNull(array);
        ArgumentNullException.ThrowIfNull(items);

        int index = 0;

        foreach (T item in items)
        {
            if (index >= array.Length)
                break;

            array[index++] = item;
        }

        return index;
    }
}
