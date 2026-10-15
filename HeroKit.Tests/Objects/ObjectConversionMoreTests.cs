using HeroKit.Objects;

namespace HeroKit.Tests.Objects;

public class ObjectConversionMoreTests
{
    public enum Level
    {
        Low = 1,
        High = 2,
    }

    private static readonly Guid Sample = new("d3b07384-d9a0-4c9b-8f0e-1c2b3a4d5e6f");

    [Fact]
    public void TryConvertTo_ShouldParseGuid_FromString()
    {
        bool ok = Sample.ToString().TryConvertTo(out Guid result);

        Assert.True(ok);
        Assert.Equal(Sample, result);
    }

    [Fact]
    public void TryConvertTo_ShouldFail_ForInvalidGuidString()
    {
        Assert.False("not-a-guid".TryConvertTo(out Guid _));
    }

    [Fact]
    public void TryConvertTo_ShouldBuildGuid_FromSixteenBytes()
    {
        bool ok = Sample.ToByteArray().TryConvertTo(out Guid result);

        Assert.True(ok);
        Assert.Equal(Sample, result);
    }

    [Fact]
    public void TryConvertTo_ShouldFail_ForGuidFromWrongLengthBytes()
    {
        Assert.False(new byte[5].TryConvertTo(out Guid _));
    }

    [Fact]
    public void TryConvertTo_ShouldFail_ForGuidFromUnsupportedType()
    {
        Assert.False(42.TryConvertTo(out Guid _));
    }

    [Fact]
    public void TryConvertTo_ShouldConvertNullableGuid()
    {
        bool ok = Sample.ToString().TryConvertTo(out Guid? result);

        Assert.True(ok);
        Assert.Equal(Sample, result);
    }

    [Theory]
    [InlineData("High", Level.High)]
    [InlineData("high", Level.High)]
    [InlineData("2", Level.High)]
    [InlineData(1, Level.Low)]
    public void TryConvertTo_ShouldConvertEnum_ByNameOrNumber(object input, Level expected)
    {
        bool ok = input.TryConvertTo(out Level result);

        Assert.True(ok);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("Unknown")]
    [InlineData("99")]
    [InlineData(99)]
    public void TryConvertTo_ShouldFail_ForUndefinedEnumValue(object input)
    {
        Assert.False(input.TryConvertTo(out Level _));
    }

    [Fact]
    public void TryConvertTo_ShouldConvertNullableEnum()
    {
        bool ok = "Low".TryConvertTo(out Level? result);

        Assert.True(ok);
        Assert.Equal(Level.Low, result);
    }

    [Theory]
    [InlineData(300)]
    [InlineData(-1)]
    public void TryConvertTo_ShouldFail_OnOverflow(int input)
    {
        Assert.False(input.TryConvertTo(out byte _));
    }

    [Fact]
    public void TryConvertTo_ShouldFail_ForUnparsableNumber()
    {
        Assert.False("abc".TryConvertTo(out int _));
    }

    [Fact]
    public void TryConvertTo_ShouldFail_ForInvalidCast()
    {
        Assert.False(new object().TryConvertTo(out int _));
    }

    [Fact]
    public void TryConvertTo_ShouldConvertToNullableTarget()
    {
        bool ok = "42".TryConvertTo(out int? result);

        Assert.True(ok);
        Assert.Equal(42, result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void TryConvertTo_ShouldFail_ForNullOrBlank(object? input)
    {
        Assert.False(input.TryConvertTo(out int _));
    }

    [Fact]
    public void TryConvertTo_ShouldFail_ForDbNull()
    {
        Assert.False(DBNull.Value.TryConvertTo(out int _));
    }

    [Fact]
    public void ConvertTo_ShouldReturnDefault_WhenConversionFails()
    {
        Assert.Equal(7, "abc".ConvertTo(7));
    }

    [Fact]
    public void ConvertTo_ShouldReturnConvertedValue_WhenConversionSucceeds()
    {
        Assert.Equal(12, "12".ConvertTo(7));
    }
}
