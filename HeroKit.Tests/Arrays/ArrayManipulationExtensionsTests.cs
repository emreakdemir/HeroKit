namespace HeroKit.Tests.Arrays;

using HeroKit.Arrays;

public class ArrayManipulationExtensionsTests
{
    [Fact]
    public void InsertItemsShouldReplaceArrayItems()
    {
        // Arrange
        int[] array = { 0, 0, 0, 0, 0 };
        int[] items = { 1, 2, 3, 4, 5 };

        // Act
        array.InsertItems(items);

        // Assert
        Assert.Equal(items, array);
    }

    [Fact]
    public void InsertItemsWithMoreItemsThanArraySizeShouldNotThrowException()
    {
        // Arrange
        int[] array = { 0, 0, 0, 0, 0 };
        int[] items = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        // Act
        array.InsertItems(items);

        // Assert
        Assert.Equal(items.Take(array.Length), array);
    }

    [Fact]
    public void InsertItemsWithLessItemsThanArraySizeShouldOnlyReplaceSomeItems()
    {
        // Arrange
        int[] array = { 0, 0, 0, 0, 0 };
        int[] items = { 1, 2, 3 };

        // Act
        array.InsertItems(items);

        // Assert
        Assert.Equal(new[] { 1, 2, 3, 0, 0 }, array);
    }
}