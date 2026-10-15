using HeroKit.Decimals;

namespace HeroKit.Tests.Decimals;

public class DecimalPriceExtensionsTests
{
    [Theory]
    [InlineData("2.675", "0.01", MidpointRounding.AwayFromZero, "2.68")]
    [InlineData("2.675", "0.01", MidpointRounding.ToEven, "2.68")]
    [InlineData("2.665", "0.01", MidpointRounding.ToEven, "2.66")]
    [InlineData("2.665", "0.01", MidpointRounding.AwayFromZero, "2.67")]
    [InlineData("12.37", "0.05", MidpointRounding.AwayFromZero, "12.35")]
    [InlineData("12.38", "0.05", MidpointRounding.AwayFromZero, "12.40")]
    [InlineData("12.325", "0.05", MidpointRounding.AwayFromZero, "12.35")]
    [InlineData("1.24", "0.5", MidpointRounding.AwayFromZero, "1.0")]
    [InlineData("1.25", "0.5", MidpointRounding.AwayFromZero, "1.5")]
    [InlineData("1.75", "0.5", MidpointRounding.ToEven, "2.0")]
    [InlineData("12", "5", MidpointRounding.AwayFromZero, "10")]
    [InlineData("12.5", "5", MidpointRounding.AwayFromZero, "15")]
    [InlineData("12.5", "5", MidpointRounding.ToEven, "10")]
    [InlineData("-12.5", "5", MidpointRounding.AwayFromZero, "-15")]
    public void RoundToStep_ShouldRoundToNearestMultiple(string value, string step, MidpointRounding mode, string expected)
    {
        Assert.Equal(decimal.Parse(expected, System.Globalization.CultureInfo.InvariantCulture),
            decimal.Parse(value, System.Globalization.CultureInfo.InvariantCulture).RoundToStep(decimal.Parse(step, System.Globalization.CultureInfo.InvariantCulture), mode));
    }

    [Fact]
    public void RoundToStep_ShouldDefaultToAwayFromZero()
    {
        Assert.Equal(2.68m, 2.675m.RoundToStep(0.01m));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-0.05)]
    public void RoundToStep_NonPositiveStep_ShouldThrow(double step)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => 1m.RoundToStep((decimal)step));
    }

    [Theory]
    [InlineData("12.30", "0.99", "12.99")]
    [InlineData("12.99", "0.99", "12.99")]
    [InlineData("13.00", "0.99", "13.99")]
    [InlineData("0", "0.99", "0.99")]
    [InlineData("12.40", "0.50", "12.50")]
    [InlineData("12.60", "0.50", "13.50")]
    [InlineData("12.00", "0", "12")]
    [InlineData("12.01", "0", "13")]
    public void ToPsychologicalPrice_ShouldApplyEnding(string value, string ending, string expected)
    {
        var ci = System.Globalization.CultureInfo.InvariantCulture;
        Assert.Equal(decimal.Parse(expected, ci), decimal.Parse(value, ci).ToPsychologicalPrice(decimal.Parse(ending, ci)));
    }

    [Fact]
    public void ToPsychologicalPrice_ShouldDefaultToNinetyNine()
    {
        Assert.Equal(12.99m, 12.30m.ToPsychologicalPrice());
    }

    [Fact]
    public void ToPsychologicalPrice_InvalidArguments_ShouldThrow()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => (-0.01m).ToPsychologicalPrice());
        Assert.Throws<ArgumentOutOfRangeException>(() => 1m.ToPsychologicalPrice(-0.01m));
        Assert.Throws<ArgumentOutOfRangeException>(() => 1m.ToPsychologicalPrice(1m));
    }

    [Fact]
    public void AddPercentAndSubtractPercent_ShouldNotRound()
    {
        Assert.Equal(220m, 200m.AddPercent(10m));
        Assert.Equal(150m, 200m.SubtractPercent(25m));
        Assert.Equal(10.0333m, 10m.AddPercent(0.333m));
        Assert.Equal(100m, 100m.AddPercent(0m));
    }

    [Theory]
    [InlineData(1, 100, 101)]
    [InlineData(10, 100, 110)]
    [InlineData(20, 100, 120)]
    public void Vat_ShouldUseTurkishRates(decimal rate, decimal net, decimal gross)
    {
        Assert.Equal(gross, net.IncludeVat(rate));
        Assert.Equal(net, gross.ExcludeVat(rate));
        Assert.Equal(gross - net, gross.VatAmountFromGross(rate));
    }

    [Fact]
    public void Vat_ShouldRoundOnlyWhenDecimalsGiven()
    {
        Assert.True(100m.ExcludeVat(20m) > 83.3333m);
        Assert.Equal(83.33m, 100m.ExcludeVat(20m, 2));
        Assert.Equal(16.67m, 100m.VatAmountFromGross(20m, 2));
        Assert.Equal(12.35m, 10.285m.IncludeVat(20m, 2));
        Assert.Equal(0m, 0m.IncludeVat(20m));
    }

    [Fact]
    public void Vat_NegativeRate_ShouldThrow()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => 1m.IncludeVat(-1m));
        Assert.Throws<ArgumentOutOfRangeException>(() => 1m.ExcludeVat(-1m));
        Assert.Throws<ArgumentOutOfRangeException>(() => 1m.VatAmountFromGross(-1m));
    }
}
