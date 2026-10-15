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
    [InlineData("SecondValue", TestValues.SecondValue)]
    [InlineData("thirdvalue", TestValues.ThirdValue)]
    [InlineData("99", TestValues.FirstValue)]
    [InlineData("2", TestValues.SecondValue)]
    [InlineData("3", TestValues.ThirdValue)]
    [InlineData("NonExistentValue", TestValues.FirstValue)]
    [InlineData(null, TestValues.FirstValue)]
    public void ToEnum_ShouldConvertStringToEnumValue(string? input, TestValues expectedResult)
    {
        // Arrange

        // Act
        TestValues result = input.ToEnum(TestValues.FirstValue);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData("1.234,56", 1234.56)]
    [InlineData("1234,56", 1234.56)]
    [InlineData("1234.56", 1234.56)]
    public void ToDecimal_ShouldHandleGroupSeparators(string input, double expected)
    {
        Assert.Equal((decimal)expected, input.ToDecimal());
    }

    [Theory]
    [InlineData(null, false, false)]
    [InlineData(null, true, true)]
    [InlineData("", true, true)]
    [InlineData("True", false, true)]
    [InlineData("TRUE", false, true)]
    [InlineData("false", true, false)]
    public void ToBoolean_ShouldReturnDefaultForMissingInput(string? input, bool defaultValue, bool expected)
    {
        Assert.Equal(expected, input.ToBoolean(defaultvalue: defaultValue));
    }
}
