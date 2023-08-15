namespace HeroKit.Tests.Strings;

public class StringConversionExtensionsTests
{
    [Fact]
    public void ToDecimal_ShouldConvertStringToDecimal()
    {
        // Arrange
        const string input = "123.45";

        // Act
        decimal result = input.ToDecimal();

        // Assert
        Assert.Equal(123.45M, result);
    }

    [Theory]
    [InlineData("", TestValues.FirstValue)]
    [InlineData("SecondValue", TestValues.FirstValue)]
    [InlineData("2", TestValues.SecondValue)]
    [InlineData("3", TestValues.ThirdValue)]
    [InlineData("NonExistentValue", TestValues.FirstValue)]
    [InlineData(null, TestValues.FirstValue)]
    public void ToEnum_ShouldConvertStringToEnumValue(string input, TestValues expectedResult)
    {
        // Arrange

        // Act
        TestValues result = input.ToEnum(TestValues.FirstValue);

        // Assert
        Assert.Equal(expectedResult, result);
    }
}