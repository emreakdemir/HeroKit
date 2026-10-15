using HeroKit.Enums;
using HeroKit.Tests.Strings;

namespace HeroKit.Tests.Enums;

public class EnumExtensionsTests
{
    [Fact]
    public void IsDefined_ShouldDetectUndefinedValues()
    {
        Assert.True(TestValues.SecondValue.IsDefined());
        Assert.False(((TestValues)99).IsDefined());
    }

    [Fact]
    public void ToByte_ShouldReturnUnderlyingValue()
    {
        Assert.Equal((byte)3, TestValues.ThirdValue.ToByte());
    }

    [Fact]
    public void ToUnderlying_ShouldReturnNumericValue()
    {
        Assert.Equal(2, TestValues.SecondValue.ToUnderlying<int>());
        Assert.Equal(5L, DayOfWeek.Friday.ToUnderlying<long>());
    }

    [Fact]
    public void ToList_ShouldReturnAllValues()
    {
        List<TestValues> result = EnumExtensions.ToList<TestValues>();

        Assert.Equal([TestValues.FirstValue, TestValues.SecondValue, TestValues.ThirdValue], result);
    }

}
