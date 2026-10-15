using System.Globalization;

namespace HeroKit.Tests.Strings;

public class CultureAwareCasingTests
{
    private static readonly CultureInfo Turkish = CultureInfo.GetCultureInfo("tr-TR");
    private static readonly CultureInfo English = CultureInfo.GetCultureInfo("en-US");

    [Theory]
    [InlineData("istanbul ılıca", "tr-TR", "İstanbul Ilıca")]
    [InlineData("istanbul ılıca", "en-US", "Istanbul Ilıca")]
    [InlineData("HELLO WORLD", "", "Hello World")]
    [InlineData("jean-luc picard", "fr-FR", "Jean-Luc Picard")]
    [InlineData(null, "", "")]
    public void ToTitleCase_ShouldUseCultureRules(string? value, string culture, string expected)
    {
        Assert.Equal(expected, value.ToTitleCase(CultureInfo.GetCultureInfo(culture)));
    }

    [Theory]
    [InlineData("IŞIK", "ışık", true)]
    [InlineData("ISIK", "isik", false)]
    public void EqualsIgnoreCase_WithTurkishCulture_ShouldMatchDotlessI(string value, string other, bool expected)
    {
        Assert.Equal(expected, value.EqualsIgnoreCase(other, Turkish));
    }

    [Theory]
    [InlineData("ISIK", "isik", true)]
    [InlineData("FILE", "file", true)]
    [InlineData(null, null, true)]
    [InlineData("a", null, false)]
    public void EqualsIgnoreCase_WithEnglishCulture_ShouldMatchDottedI(string? value, string? other, bool expected)
    {
        Assert.Equal(expected, value.EqualsIgnoreCase(other, English));
    }

    [Fact]
    public void Methods_ShouldRejectNullCulture()
    {
        Assert.Throws<ArgumentNullException>(() => "a".ToTitleCase(null!));
        Assert.Throws<ArgumentNullException>(() => "a".EqualsIgnoreCase("a", null!));
    }
}
