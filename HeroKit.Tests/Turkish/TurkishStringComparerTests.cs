using System.Globalization;
using HeroKit.Turkish;

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
        Array.Sort(sorted, TurkishStringComparer.Instance);

        Assert.Equal(["Cem", "Çağla", "Gül", "Ğ", "Ilgın", "İpek", "Oya", "Ömer", "Şule", "Uğur", "Ümit", "Zeynep"], sorted);
    }

    [Fact]
    public void Sort_ShouldMatchIcuTurkishCollationForTurkishWords()
    {
        // ICU's tr-TR collation is the reference; our comparer must agree without depending on it.
        string[] expected = [.. Words];
        Array.Sort(expected, StringComparer.Create(CultureInfo.GetCultureInfo("tr-TR"), ignoreCase: false));

        string[] actual = [.. Words];
        Array.Sort(actual, TurkishStringComparer.Instance);

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
        Assert.Equal(expectedSign, Math.Sign(TurkishStringComparer.Instance.Compare(x, y)));
    }

    [Theory]
    [InlineData("IŞIK", "ışık")]
    [InlineData("İSTANBUL", "istanbul")]
    [InlineData("ÇİÇEK", "çiçek")]
    public void IgnoreCase_ShouldUseTurkishCasing(string x, string y)
    {
        Assert.True(TurkishStringComparer.IgnoreCase.Equals(x, y));
        Assert.Equal(TurkishStringComparer.IgnoreCase.GetHashCode(x), TurkishStringComparer.IgnoreCase.GetHashCode(y));
        Assert.False(TurkishStringComparer.Instance.Equals(x, y));
        Assert.False(TurkishStringComparer.IgnoreCase.Equals("ISIK", "isik"));
    }

    [Fact]
    public void IgnoreCase_ShouldWorkAsDictionaryComparer()
    {
        var cities = new Dictionary<string, int>(TurkishStringComparer.IgnoreCase) { ["İzmir"] = 35 };

        Assert.True(cities.ContainsKey("izmir"));
        Assert.False(cities.ContainsKey("IZMIR")); // I is ı, not i
    }

    private sealed record Customer(string Name, string City);

    [Fact]
    public void LinqHelpers_ShouldSortByKey()
    {
        Customer[] customers = [new("Şule", "İzmir"), new("Cem", "Ankara"), new("Çağla", "Ankara"), new("Cem", "Adana")];

        Assert.Equal(["Cem/Adana", "Cem/Ankara", "Çağla/Ankara", "Şule/İzmir"],
            customers.OrderByTurkish(c => c.Name).ThenByTurkish(c => c.City).Select(c => $"{c.Name}/{c.City}"));
        Assert.Equal(["Şule", "Çağla", "Cem", "Cem"], customers.OrderByTurkishDescending(c => c.Name).Select(c => c.Name));
        Assert.Equal(["cam", "çay", "su", "şeker"], new[] { "şeker", "su", "çay", "cam" }.OrderByTurkish());
    }
}
