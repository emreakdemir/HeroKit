namespace HeroKit.Tests.Strings;

public class StringControlExtensionsTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void IsNullOrEmpty_ShouldReturnTrue_ForNullOrEmptyStrings(string value)
    {
        // Arrange

        // Act
        bool result = value.IsNullOrEmpty();

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData("Hello")]
    [InlineData("   Hello")]
    [InlineData("Hello   ")]
    [InlineData("   Hello   ")]
    public void IsNullOrEmpty_ShouldReturnFalse_ForNonNullOrEmptyStrings(string value)
    {
        // Arrange

        // Act
        bool result = value.IsNullOrEmpty();

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void IsNotNullOrEmpty_ShouldReturnFalse_ForNullOrEmptyStrings(string value)
    {
        // Arrange

        // Act
        bool result = value.IsNotNullOrEmpty();

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData("Hello")]
    [InlineData("   Hello")]
    [InlineData("Hello   ")]
    [InlineData("   Hello   ")]
    public void IsNotNullOrEmpty_ShouldReturnTrue_ForNonNullOrEmptyStrings(string value)
    {
        // Arrange

        // Act
        bool result = value.IsNotNullOrEmpty();

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData(null, ' ', true)]
    [InlineData("", ' ', true)]
    [InlineData("  ", ' ', false)]
    [InlineData("Hello World", ' ', false)]
    [InlineData("SomeValue", ' ', false)]
    public void IsNullOrEmpty_ShouldCheckIfStringIsNullOrEmptyOrSplitValue(string input, char splitValue, bool expectedResult)
    {
        // Arrange

        // Act
        bool result = input.IsNullOrEmpty(splitValue);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(null, ' ', false)]
    [InlineData("", ' ', false)]
    [InlineData("  ", ' ', true)]
    [InlineData("Hello World", ' ', true)]
    [InlineData("SomeValue", ' ', true)]
    public void IsNotNullOrEmpty_ShouldCheckIfStringIsNotNullOrEmptyOrSplitValue(string input, char splitValue, bool expectedResult)
    {
        // Arrange

        // Act
        bool result = input.IsNotNullOrEmpty(splitValue);

        // Assert
        Assert.Equal(expectedResult, result);
    }
}