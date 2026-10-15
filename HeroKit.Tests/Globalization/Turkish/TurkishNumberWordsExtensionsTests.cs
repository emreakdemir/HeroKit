using HeroKit.Globalization;
using HeroKit.Globalization.Turkey;

namespace HeroKit.Tests.Turkish;

public class TurkishNumberWordsExtensionsTests
{
    [Theory]
    [InlineData(0, "sıfır")]
    [InlineData(1, "bir")]
    [InlineData(10, "on")]
    [InlineData(11, "on bir")]
    [InlineData(40, "kırk")]
    [InlineData(99, "doksan dokuz")]
    [InlineData(100, "yüz")]
    [InlineData(101, "yüz bir")]
    [InlineData(123, "yüz yirmi üç")]
    [InlineData(200, "iki yüz")]
    [InlineData(999, "dokuz yüz doksan dokuz")]
    [InlineData(1000, "bin")]
    [InlineData(1001, "bin bir")]
    [InlineData(1100, "bin yüz")]
    [InlineData(2000, "iki bin")]
    [InlineData(1234, "bin iki yüz otuz dört")]
    [InlineData(11000, "on bir bin")]
    [InlineData(21000, "yirmi bir bin")]
    [InlineData(100000, "yüz bin")]
    [InlineData(101000, "yüz bir bin")]
    [InlineData(1000000, "bir milyon")]
    [InlineData(1001000, "bir milyon bin")]
    [InlineData(1000001, "bir milyon bir")]
    [InlineData(-5, "eksi beş")]
    [InlineData(int.MaxValue, "iki milyar yüz kırk yedi milyon dört yüz seksen üç bin altı yüz kırk yedi")]
    public void ToTurkishWords_ShouldWriteIntegers(int value, string expected)
    {
        Assert.Equal(expected, value.ToWords(Languages.Turkish));
    }

    [Theory]
    [InlineData(1_000_000_000L, "bir milyar")]
    [InlineData(1_000_000_000_000L, "bir trilyon")]
    [InlineData(long.MaxValue, "dokuz kentilyon iki yüz yirmi üç katrilyon üç yüz yetmiş iki trilyon otuz altı milyar sekiz yüz elli dört milyon yedi yüz yetmiş beş bin sekiz yüz yedi")]
    [InlineData(long.MinValue, "eksi dokuz kentilyon iki yüz yirmi üç katrilyon üç yüz yetmiş iki trilyon otuz altı milyar sekiz yüz elli dört milyon yedi yüz yetmiş beş bin sekiz yüz sekiz")]
    public void ToTurkishWords_ShouldWriteLargeNumbers(long value, string expected)
    {
        Assert.Equal(expected, value.ToWords(Languages.Turkish));
    }

    [Theory]
    [InlineData("123.50", "yüz yirmi üç Türk lirası elli kuruş")]
    [InlineData("1000", "bin Türk lirası")]
    [InlineData("0.75", "yetmiş beş kuruş")]
    [InlineData("0", "sıfır Türk lirası")]
    [InlineData("0.001", "sıfır Türk lirası")]
    [InlineData("1.005", "bir Türk lirası bir kuruş")]
    [InlineData("1.999", "iki Türk lirası")]
    [InlineData("-12.30", "eksi on iki Türk lirası otuz kuruş")]
    [InlineData("1250000.05", "bir milyon iki yüz elli bin Türk lirası beş kuruş")]
    public void ToTurkishCurrencyWords_ShouldWriteLiraAndKurus(string amount, string expected)
    {
        Assert.Equal(expected, decimal.Parse(amount, System.Globalization.CultureInfo.InvariantCulture).ToCurrencyWords(Currencies.TurkishLira));
    }

    [Fact]
    public void ToTurkishCurrencyWords_ShouldUseGivenUnitNames()
    {
        var euroInTurkish = new Currency("EUR", "€", Languages.Turkish) { UnitName = "Euro", SubunitName = "sent" };

        Assert.Equal("on Euro yirmi sent", 10.20m.ToCurrencyWords(euroInTurkish));
    }
}
