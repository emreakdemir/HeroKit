using HeroKit.Globalization;
using HeroKit.Globalization.Turkey;

namespace HeroKit.Tests.Turkish;

public class TurkishTextExtensionsTests
{
    [Theory]
    [InlineData("istanbul ılıca", "İstanbul Ilıca")]
    [InlineData("IŞIK ÇİÇEK", "Işık Çiçek")]
    [InlineData("İZMİR", "İzmir")]
    [InlineData("ırmak", "Irmak")]
    [InlineData("istanbul'da", "İstanbul'da")]
    [InlineData("ali-veli", "Ali-Veli")]
    [InlineData("  çift   boşluk ", "  Çift   Boşluk ")]
    [InlineData("ğüşöç", "Ğüşöç")]
    [InlineData("a", "A")]
    [InlineData("", "")]
    [InlineData(null, "")]
    public void ToTitleCaseTr_ShouldFollowTurkishCasing(string? value, string expected)
    {
        Assert.Equal(expected, value.ToTitleCase(Languages.Turkish));
    }

    [Theory]
    [InlineData("IŞIK", "ışık", true)]
    [InlineData("İSTANBUL", "istanbul", true)]
    [InlineData("ISTANBUL", "istanbul", false)]
    [InlineData("ISIK", "isik", false)]
    [InlineData("Çiçek", "ÇİÇEK", true)]
    [InlineData("abc", "abd", false)]
    [InlineData(null, null, true)]
    [InlineData(null, "", false)]
    public void EqualsTr_ShouldIgnoreCaseWithTurkishRules(string? value, string? other, bool expected)
    {
        Assert.Equal(expected, value.EqualsIgnoreCase(other, Languages.Turkish));
    }
}
