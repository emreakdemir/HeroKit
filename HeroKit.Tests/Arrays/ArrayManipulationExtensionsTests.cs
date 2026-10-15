namespace HeroKit.Tests.Arrays;

using HeroKit.Arrays;

public class ArrayManipulationExtensionsTests
{
    [Fact]
    public void CopyFromShouldReplaceArrayItems()
    {
        // Arrange
        int[] array = { 0, 0, 0, 0, 0 };
        int[] items = { 1, 2, 3, 4, 5 };

        // Act
        int copied = array.CopyFrom(items);

        // Assert
        Assert.Equal(items, array);
        Assert.Equal(5, copied);
    }

    [Fact]
    public void CopyFromWithMoreItemsThanArraySizeShouldNotThrowException()
    {
        // Arrange
        int[] array = { 0, 0, 0, 0, 0 };
        int[] items = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        // Act
        int copied = array.CopyFrom(items);

        // Assert
        Assert.Equal(items.Take(array.Length), array);
    }

    [Fact]
    public void CopyFromWithLessItemsThanArraySizeShouldOnlyReplaceSomeItems()
    {
        // Arrange
        int[] array = { 0, 0, 0, 0, 0 };
        int[] items = { 1, 2, 3 };

        // Act
        int copied = array.CopyFrom(items);

        // Assert
        Assert.Equal(new[] { 1, 2, 3, 0, 0 }, array);
        Assert.Equal(3, copied);
    }
}