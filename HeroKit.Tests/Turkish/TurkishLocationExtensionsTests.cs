using HeroKit.Turkish;

namespace HeroKit.Tests.Turkish;

public class TurkishLocationExtensionsTests
{
    [Fact]
    public void Provinces_HasEightyOneEntriesWithSequentialCodes()
    {
        Assert.Equal(81, TurkishProvinces.All.Count);
        for (int i = 0; i < 81; i++)
            Assert.Equal(i + 1, TurkishProvinces.All[i].Code);
    }

    [Fact]
    public void Provinces_RoundTripCodeNameCode()
    {
        foreach (Province province in TurkishProvinces.All)
        {
            string? name = province.Code.ToProvinceName();
            Assert.Equal(province.Name, name);
            Assert.Equal(province.Code, name.ToProvinceCode());
        }
    }

    [Theory]
    [InlineData("34", "İstanbul")]
    [InlineData("06", "Ankara")]
    [InlineData("6", "Ankara")]
    [InlineData(" 81 ", "Düzce")]
    [InlineData("0", null)]
    [InlineData("00", null)]
    [InlineData("82", null)]
    [InlineData("abc", null)]
    [InlineData("-1", null)]
    [InlineData("", null)]
    [InlineData(null, null)]
    public void ToProvinceName_String(string? code, string? expected) =>
        Assert.Equal(expected, code.ToProvinceName());

    [Theory]
    [InlineData(0, null)]
    [InlineData(82, null)]
    [InlineData(-5, null)]
    [InlineData(1, "Adana")]
    public void ToProvinceName_Int(int code, string? expected) =>
        Assert.Equal(expected, code.ToProvinceName());

    [Theory]
    [InlineData("istanbul", 34)]
    [InlineData("İSTANBUL", 34)]
    [InlineData("Istanbul", 34)]
    [InlineData(" İstanbul ", 34)]
    [InlineData("sanliurfa", 63)]
    [InlineData("Şanlıurfa", 63)]
    [InlineData("ŞANLIURFA", 63)]
    [InlineData("igdir", 76)]
    [InlineData("IĞDIR", 76)]
    [InlineData("Iğdır", 76)]
    [InlineData("canakkale", 17)]
    [InlineData("Atlantis", null)]
    [InlineData("", null)]
    [InlineData("  ", null)]
    [InlineData(null, null)]
    public void ToProvinceCode_IgnoresCaseAndDiacritics(string? name, int? expected) =>
        Assert.Equal(expected, name.ToProvinceCode());

    [Theory]
    [InlineData("34ABC123", "34 ABC 123")]
    [InlineData("06 a 1234", "06 A 1234")]
    [InlineData("35-AB-123", "35 AB 123")]
    [InlineData("34 AB 1234", "34 AB 1234")]
    [InlineData("34 ABC 12", "34 ABC 12")]
    [InlineData("34 abc 123", "34 ABC 123")]
    [InlineData("81 ZY 9999", "81 ZY 9999")]
    public void FormatPlateTr_Valid(string value, string expected)
    {
        Assert.Equal(expected, value.FormatPlateTr());
        Assert.True(value.IsValidPlateTr());
    }

    [Theory]
    [InlineData("00 A 1234")]
    [InlineData("82 A 1234")]
    [InlineData("34 Q 1234")]
    [InlineData("34 W 1234")]
    [InlineData("34 X 1234")]
    [InlineData("34 Ç 1234")]
    [InlineData("34 A 123")]
    [InlineData("34 A 12345")]
    [InlineData("34 ABCD 12")]
    [InlineData("34 AB 012")]
    [InlineData("34 AB 12")]
    [InlineData("34 ABC 1")]
    [InlineData("34 ABC 1234")]
    [InlineData("34 AB")]
    [InlineData("34 123")]
    [InlineData("34 AB 12A")]
    [InlineData("34 AB 1.23")]
    [InlineData("ABC 123")]
    [InlineData("34 AB 123 456 789 012")]
    [InlineData("")]
    [InlineData(null)]
    public void Plate_Invalid(string? value)
    {
        Assert.False(value.IsValidPlateTr());
        Assert.Null(value.FormatPlateTr());
    }

    [Theory]
    [InlineData("34710", true)]
    [InlineData("06100", true)]
    [InlineData("81000", true)]
    [InlineData("82000", false)]
    [InlineData("00100", false)]
    [InlineData("3471", false)]
    [InlineData("347100", false)]
    [InlineData("3471a", false)]
    [InlineData("34 71", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void IsValidPostalCodeTr(string? value, bool expected) =>
        Assert.Equal(expected, value.IsValidPostalCodeTr());
}
