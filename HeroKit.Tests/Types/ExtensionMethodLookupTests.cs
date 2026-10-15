using System.Reflection;

namespace HeroKit.Tests.Types;

public static class OverloadedSampleExtensions
{
    public static string Pad(this string value, int count) => value + new string(' ', count);

    public static string Pad(this string value, char character) => value + character;
}

public class ExtensionMethodLookupTests
{
    private static readonly Assembly TestAssembly = typeof(OverloadedSampleExtensions).Assembly;

    [Theory]
    [InlineData(typeof(int))]
    [InlineData(typeof(char))]
    public void GetExtensionMethod_ShouldMatchOverloadByParameterTypes(Type parameterType)
    {
        MethodInfo? method = typeof(string).GetExtensionMethod(TestAssembly, nameof(OverloadedSampleExtensions.Pad), [parameterType]);

        Assert.NotNull(method);
        Assert.Equal(parameterType, method.GetParameters()[1].ParameterType);
    }

    [Fact]
    public void GetExtensionMethod_ShouldReturnNullWhenNoOverloadMatches()
    {
        Assert.Null(typeof(string).GetExtensionMethod(TestAssembly, nameof(OverloadedSampleExtensions.Pad), [typeof(long)]));
    }
}
