using HeroKit.Arrays;
using HeroKit.Objects;

namespace HeroKit.Tests.Nullability;

// These tests compile only if the null-state attributes work:
// the test project treats nullable warnings as errors.
public class NullFlowAnalysisTests
{
    [Theory]
    [InlineData("hero")]
    public void IsNotNullOrEmpty_ShouldMarkStringAsNotNull(string? value)
    {
        if (value.IsNotNullOrEmpty())
            Assert.Equal(4, value.Length);
    }

    [Theory]
    [InlineData("hero")]
    public void IsNullOrEmpty_ShouldMarkStringAsNotNullWhenFalse(string? value)
    {
        if (!value.IsNullOrEmpty())
            Assert.Equal(4, value.Length);
    }

    [Fact]
    public void IsNotNull_ShouldMarkObjectAsNotNull()
    {
        object? value = "hero";

        if (value.IsNotNull())
            Assert.Equal("hero", value.ToString());
    }

    [Fact]
    public void IsNotNullOrEmpty_ShouldMarkArrayAsNotNull()
    {
        int[]? values = [1, 2];

        if (values.IsNotNullOrEmpty())
            Assert.Equal(2, values.Length);
    }

    [Fact]
    public void TryConvertTo_ShouldMarkResultAsNotNullWhenTrue()
    {
        object? value = "hero";

        if (value.TryConvertTo(out string? result))
            Assert.Equal(4, result.Length);

        object? missing = null;
        Assert.False(missing.TryConvertTo(out string? _));
    }
}
