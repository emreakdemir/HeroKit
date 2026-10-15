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
    public void IsOdd_ShouldHandleNegativeNumbers(int input, bool expected)
    {
        Assert.Equal(expected, input.IsOdd());
        Assert.Equal(!expected, input.IsEven());
    }

}
