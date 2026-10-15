using HeroKit.Integers;

namespace HeroKit.Tests.Integers;

public class IntegerExtensionsTests
{
    [Theory]
    [InlineData(3, true)]
    [InlineData(-3, true)]
    [InlineData(4, false)]
    [InlineData(-4, false)]
    [InlineData(0, false)]
    public void IsOddNumber_ShouldHandleNegativeNumbers(int input, bool expected)
    {
        Assert.Equal(expected, input.IsOddNumber());
        Assert.Equal(!expected, input.IsEvenNumber());
    }

    [Theory]
    [InlineData(12, 16)]
    [InlineData(9, 12)]
    [InlineData(0, 0)]
    public void ToPixel_ShouldConvertPointsAt96Dpi(int point, int expected)
    {
        Assert.Equal(expected, point.ToPixel());
    }
}
