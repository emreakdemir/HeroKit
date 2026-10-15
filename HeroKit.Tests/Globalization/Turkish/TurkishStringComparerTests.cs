using System.Globalization;
using HeroKit.Globalization;
using HeroKit.Globalization.Turkey;

namespace HeroKit.Tests.Turkish;

public class TurkishStringComparerTests
{
    private static readonly string[] Words =
    [
        "Zeynep", "çiçek", "Cem", "Ömer", "ılık", "İlker", "Irmak", "şeker", "Selim", "Gül", "Ali", "Ünal", "Umut", "Oğuz",
        "ağaç", "Ağaç", "Ahmet", "ağır", "ışık", "iğne", "Çağla", "çay", "cam", "su", "Su", "kuş", "Kuş", "öğle", "üzüm",
    ];

    [Fact]
    public void Sort_ShouldFollowTheTurkishAlphabet()
    {
        string[] sorted = ["Zeynep", "Çağla", "Ilgın", "İpek", "Cem", "Şule", "Oya", "Ömer", "Uğur", "Ümit", "Gül", "Ğ"];
        Array.Sort(sorted, Languages.Turkish.TextRules!.Comparer);

        Assert.Equal(["Cem", "Çağla", "Gül", "Ğ", "Ilgın", "İpek", "Oya", "Ömer", "Şule", "Uğur", "Ümit", "Zeynep"], sorted);
    }

    [Fact]
    public void Sort_ShouldMatchIcuTurkishCollationForTurkishWords()
    {
        // ICU's tr-TR collation is the reference; our comparer must agree without depending on it.
        string[] expected = [.. Words];
        Array.Sort(expected, StringComparer.Create(CultureInfo.GetCultureInfo("tr-TR"), ignoreCase: false));

        string[] actual = [.. Words];
        Array.Sort(actual, Languages.Turkish.TextRules!.Comparer);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("ılık", "ilk", -1)]   // ı before i
    [InlineData("Irmak", "İnci", -1)]  // I (ı) before İ (i)
    [InlineData("ağaç", "Ağaç", -1)]   // lower case first
    [InlineData("ab", "abc", -1)]
    [InlineData("a b", "ab", -1)]      // space before letters
    [InlineData("a1", "aa", -1)]       // digits before letters
    [InlineData("café", "cafe", 1)]    // accent only differs at the code-point tie-break
    [InlineData(null, "a", -1)]
    [InlineData("a", null, 1)]
    [InlineData(null, null, 0)]
    public void Compare_ShouldOrderEdgeCases(string? x, string? y, int expectedSign)
    {
        Assert.Equal(expectedSign, Math.Sign(Languages.Turkish.TextRules!.Comparer.Compare(x, y)));
    }

    [Theory]
    [InlineData("IŞIK", "ışık")]
    [InlineData("İSTANBUL", "istanbul")]
    [InlineData("ÇİÇEK", "çiçek")]
    public void IgnoreCase_ShouldUseTurkishCasing(string x, string y)
    {
        Assert.True(Languages.Turkish.TextRules!.IgnoreCaseComparer.Equals(x, y));
        Assert.Equal(Languages.Turkish.TextRules!.IgnoreCaseComparer.GetHashCode(x), Languages.Turkish.TextRules!.IgnoreCaseComparer.GetHashCode(y));
        Assert.False(Languages.Turkish.TextRules!.Comparer.Equals(x, y));
        Assert.False(Languages.Turkish.TextRules!.IgnoreCaseComparer.Equals("ISIK", "isik"));
    }

    [Fact]
    public void IgnoreCase_ShouldWorkAsDictionaryComparer()
    {
        var cities = new Dictionary<string, int>(Languages.Turkish.TextRules!.IgnoreCaseComparer) { ["İzmir"] = 35 };

        Assert.True(cities.ContainsKey("izmir"));
        Assert.False(cities.ContainsKey("IZMIR")); // I is ı, not i
    }

    private sealed record Customer(string Name, string City);

    [Fact]
    public void LinqHelpers_ShouldSortByKey()
    {
        Customer[] customers = [new("Şule", "İzmir"), new("Cem", "Ankara"), new("Çağla", "Ankara"), new("Cem", "Adana")];

        Assert.Equal(["Cem/Adana", "Cem/Ankara", "Çağla/Ankara", "Şule/İzmir"],
            customers.OrderBy(c => c.Name, Languages.Turkish.TextRules!.Comparer).ThenBy(c => c.City, Languages.Turkish.TextRules!.Comparer).Select(c => $"{c.Name}/{c.City}"));
        Assert.Equal(["Şule", "Çağla", "Cem", "Cem"], customers.OrderByDescending(c => c.Name, Languages.Turkish.TextRules!.Comparer).Select(c => c.Name));
        Assert.Equal(["cam", "çay", "su", "şeker"], new[] { "şeker", "su", "çay", "cam" }.Order(Languages.Turkish.TextRules!.Comparer));
    }
}
