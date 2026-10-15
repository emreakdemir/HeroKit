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
    public void GetValue_ShouldConvertToRequestedType()
    {
        Assert.Equal("2", TestValues.SecondValue.GetValue<int>());
    }

    [Fact]
    public void ToList_ShouldReturnAllValues()
    {
        List<TestValues> result = EnumExtensions.ToList<TestValues>();

        Assert.Equal([TestValues.FirstValue, TestValues.SecondValue, TestValues.ThirdValue], result);
    }

    [Fact]
    public void ToList_ShouldThrowForNonEnumType()
    {
        Assert.Throws<ArgumentException>(() => EnumExtensions.ToList<int>());
    }
}
