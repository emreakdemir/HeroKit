namespace HeroKit.Tests.Types;

public class TypeControlExtensionsTests
{
    [Theory]
    [InlineData(typeof(int?))]
    [InlineData(typeof(double?))]
    public void IsNullable_ShouldReturnTrue_ForNullableTypes(Type type)
    {
        // Arrange

        // Act
        bool result = type.IsNullable();

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData(typeof(int))]
    [InlineData(typeof(double))]
    [InlineData(typeof(object))]
    [InlineData(typeof(List<>))]
    public void IsNullable_ShouldReturnFalse_ForNonNullableTypes(Type type)
    {
        // Arrange

        // Act
        bool result = type.IsNullable();

        // Assert
        Assert.False(result);
    }
}