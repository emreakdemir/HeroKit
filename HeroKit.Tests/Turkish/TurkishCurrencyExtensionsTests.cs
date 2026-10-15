using HeroKit.Turkish;

namespace HeroKit.Tests.Turkish;

public class TurkishCurrencyExtensionsTests
{
    [Theory]
    [InlineData("1234.5", "₺1.234,50", "1.234,50 TL")]
    [InlineData("0", "₺0,00", "0,00 TL")]
    [InlineData("1234567.891", "₺1.234.567,89", "1.234.567,89 TL")]
    [InlineData("0.005", "₺0,01", "0,01 TL")]
    [InlineData("-12.3", "-₺12,30", "-12,30 TL")]
    [InlineData("-0.004", "₺0,00", "0,00 TL")]
    public void ToTurkishLira_ShouldFormatWithTurkishSeparators(string amount, string symbol, string code)
    {
        decimal value = decimal.Parse(amount, System.Globalization.CultureInfo.InvariantCulture);

        Assert.Equal(symbol, value.ToTurkishLira());
        Assert.Equal(code, value.ToTurkishLira(TurkishLiraFormat.Code));
    }
}
