using HeroKit.Objects;

namespace HeroKit.Tests.Objects;

public class ObjectControlExtensionsTests
{
    [Fact]
    public void IsNull_ShouldDetectNull()
    {
        object? nullObject = null;

        Assert.True(nullObject.IsNull());
        Assert.True(new object().IsNotNull());
    }

    [Theory]
    [InlineData(5, true)]
    [InlineData(5L, true)]
    [InlineData(5.5, true)]
    [InlineData("42", true)]
    [InlineData("abc", false)]
    [InlineData(null, false)]
    [InlineData('c', false)]
    public void IsNumeric_ShouldDetectNumericValues(object? value, bool expected)
    {
        Assert.Equal(expected, value.IsNumeric());
    }
}
