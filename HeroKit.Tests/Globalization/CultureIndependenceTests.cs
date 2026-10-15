using System.Globalization;
using HeroKit.Enums;
using HeroKit.Objects;
using HeroKit.Tests.Strings;

namespace HeroKit.Tests.Globalization;

public class CultureIndependenceTests
{
    public static TheoryData<string> Cultures => ["tr-TR", "en-US", "de-DE", "fr-FR"];

    [Theory]
    [MemberData(nameof(Cultures))]
    public void Conversions_ShouldNotDependOnCurrentCulture(string cultureName)
    {
        CultureInfo original = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo(cultureName);

            Assert.Equal(new DateTime(2026, 3, 7), "2026-03-07".ToDateTime());
            Assert.Equal(new DateTime(2026, 3, 7), "03/07/2026".ToDateTime());
            Assert.Equal(-42, "-42".ToInt());
            Assert.Equal(-42L, "-42".ToLong());
            Assert.Equal((short)-42, "-42".ToShort());
            Assert.True("1.5".IsNumeric());
            Assert.Equal(1.5, "1.5".ConvertTo<double>());
            Assert.Equal(2, TestValues.SecondValue.ToUnderlying<int>());
            Assert.Equal("istanbul", "İSTANBUL".ToLower(CultureInfo.GetCultureInfo("tr-TR")).RemoveDiacritics());
        }
        finally
        {
            CultureInfo.CurrentCulture = original;
        }
    }

    [Fact]
    public void ToDateTime_ShouldUseGivenCulture()
    {
        Assert.Equal(new DateTime(2026, 3, 7), "07.03.2026".ToDateTime(provider: CultureInfo.GetCultureInfo("tr-TR")));
    }
}
