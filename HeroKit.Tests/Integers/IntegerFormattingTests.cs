using System.Globalization;
using HeroKit.Integers;

namespace HeroKit.Tests.Integers;

public class IntegerFormattingTests
{
    [Fact]
    public void Arithmetic_ShouldReturnExpectedValues()
    {
        Assert.Equal(5, 10.Half());
        Assert.Equal(27, 3.Cube());
        Assert.Equal(16, 4.Square());
        Assert.Equal(3000, 3.Seconds());
    }

    [Theory]
    [InlineData(1, "January")]
    [InlineData(12, "December")]
    [InlineData(0, "")]
    [InlineData(13, "")]
    public void ToMonthString_ShouldReturnMonthName(int month, string expected)
    {
        Assert.Equal(expected, month.ToMonthString(new CultureInfo("en-US")));
    }
}
