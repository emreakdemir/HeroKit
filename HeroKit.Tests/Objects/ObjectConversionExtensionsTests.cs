using HeroKit.Objects;
using HeroKit.Tests.Strings;

namespace HeroKit.Tests.Objects;

public class ObjectConversionExtensionsTests
{
    [Fact]
    public void ConvertTo_Enum_ShouldReturnDefaultValueWhenConversionFails()
    {
        object source = "missing";

        Assert.Equal(TestValues.ThirdValue, source.ConvertTo(TestValues.ThirdValue));
    }

    [Fact]
    public void ConvertTo_Enum_ShouldConvertByName()
    {
        object source = "SecondValue";

        Assert.Equal(TestValues.SecondValue, source.ConvertTo<TestValues>());
    }
}
