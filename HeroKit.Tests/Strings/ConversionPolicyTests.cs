using System.Globalization;
using HeroKit.Objects;
using HeroKit.Properties;

namespace HeroKit.Tests.Strings;

// Lenient ToX returns a default value; TryToX reports success. Neither throws for bad input.
public class ConversionPolicyTests
{
    private static readonly CultureInfo Turkish = CultureInfo.GetCultureInfo("tr-TR");

    [Fact]
    public void To_ShouldParseAnyParsableType()
    {
        Assert.Equal(42, "42".To<int>());
        Assert.Equal(new DateOnly(2026, 3, 7), "2026-03-07".To<DateOnly>());
        Assert.Equal(Guid.Empty, "00000000-0000-0000-0000-000000000000".To<Guid>());
        Assert.Equal(1.5m, "1,5".To<decimal>(provider: Turkish));
        Assert.Equal(42, " 42 ".To<int>());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("abc")]
    [InlineData("99999999999")]
    public void To_ShouldReturnDefaultForBadInput(string? value)
    {
        Assert.Equal(-1, value.To(defaultValue: -1));
        Assert.False(value.TryTo(out int _));
        Assert.Null(value.ToNullable<int>());
        Assert.Equal(-1, value.ToInt(-1));
    }

    [Fact]
    public void TryTo_ShouldReportSuccess()
    {
        Assert.True("42".TryTo(out int number));
        Assert.Equal(42, number);
        Assert.Equal(42, "42".ToNullable<int>());
    }

    [Theory]
    [InlineData("1.234,56", 1234.56)]
    [InlineData("1234.56", 1234.56)]
    [InlineData("1,5", 1.5)]
    public void ToDecimal_WithoutProvider_ShouldUseFlexibleRule(string value, double expected)
    {
        Assert.Equal((decimal)expected, value.ToDecimal());
        Assert.True(value.TryToDecimal(out decimal result));
        Assert.Equal((decimal)expected, result);
    }

    [Fact]
    public void ToDecimal_WithProvider_ShouldParseStrictly()
    {
        Assert.Equal(1234.56m, "1.234,56".ToDecimal(provider: Turkish));
        Assert.Equal(1234.56m, "1,234.56".ToDecimal(provider: CultureInfo.InvariantCulture));
        Assert.Equal(-1m, "abc".ToDecimal(-1m));
        Assert.False("abc".TryToDecimal(out _));
    }

    [Fact]
    public void ToEnum_ShouldUseNamesAndDefinedValues()
    {
        Assert.Equal(DayOfWeek.Friday, "friday".ToEnum<DayOfWeek>());
        Assert.Equal(DayOfWeek.Friday, "5".ToEnum<DayOfWeek>());
        Assert.Equal(DayOfWeek.Monday, "99".ToEnum(DayOfWeek.Monday));
        Assert.True("Sunday".TryToEnum(out DayOfWeek day));
        Assert.Equal(DayOfWeek.Sunday, day);
        Assert.False("Someday".TryToEnum(out DayOfWeek _));
    }

    [Fact]
    public void ToBoolean_ShouldAcceptCustomTrueText()
    {
        Assert.True("yes".ToBoolean(trueText: "YES"));
        Assert.False("no".ToBoolean(trueText: "yes"));
        Assert.False("yes".ToBoolean());
    }

    [Fact]
    public void FromBase64_ShouldReturnNullForInvalidInput()
    {
        Assert.Equal("HeroKit"u8.ToArray(), "SGVyb0tpdA==".FromBase64());
        Assert.Null("not base64!".FromBase64());
        Assert.Null(((string?)null).FromBase64());
    }

    [Fact]
    public void ConvertTo_ShouldBeLenientAndTryConvertToShouldReport()
    {
        object number = "42";
        object bad = "abc";

        Assert.Equal(42, number.ConvertTo<int>());
        Assert.Equal(-1, bad.ConvertTo(-1));
        Assert.False(bad.TryConvertTo(out int _));
        Assert.True(((object)"2").TryConvertTo(out DayOfWeek day));
        Assert.Equal(DayOfWeek.Tuesday, day);
        Assert.True(((object)new byte[16]).TryConvertTo(out Guid guid));
        Assert.Equal(Guid.Empty, guid);
        Assert.False(DBNull.Value.TryConvertTo(out int _));
    }

    private sealed class Sample
    {
        public int Number { get; set; } = 7;
        public int? OptionalNumber { get; set; } = 7;
        public string? Text { get; set; } = "x";
    }

    [Fact]
    public void TrySetConvertedValue_ShouldSetNullOnlyWhereAllowed()
    {
        var sample = new Sample();

        Assert.False(typeof(Sample).GetProperty(nameof(Sample.Number))!.TrySetConvertedValue(sample, null));
        Assert.True(typeof(Sample).GetProperty(nameof(Sample.OptionalNumber))!.TrySetConvertedValue(sample, null));
        Assert.True(typeof(Sample).GetProperty(nameof(Sample.Text))!.TrySetConvertedValue(sample, null));

        Assert.Equal(7, sample.Number);
        Assert.Null(sample.OptionalNumber);
        Assert.Null(sample.Text);
    }
}
