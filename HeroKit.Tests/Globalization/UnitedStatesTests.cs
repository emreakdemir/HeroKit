using System.ComponentModel.DataAnnotations;
using HeroKit.Globalization;
using HeroKit.Validation;

namespace HeroKit.Tests.Globalization;

public class UnitedStatesTests
{
    private static readonly Country Us = Countries.UnitedStates;

    [Theory]
    [InlineData("(212) 555-0123", true)]
    [InlineData("+1 212 555 0123", true)]
    [InlineData("1-212-555-0123", true)]
    [InlineData("001 212 555 0123", true)]
    [InlineData("212.555.0123", true)]
    [InlineData("112 555 0123", false)]
    [InlineData("211 555 0123", false)]
    [InlineData("212 155 0123", false)]
    [InlineData("+44 20 7946 0958", false)]
    [InlineData("212555012", false)]
    [InlineData("2125550123a", false)]
    [InlineData(null, false)]
    public void IsValidPhone_ShouldFollowTheNorthAmericanNumberingPlan(string? value, bool expected)
    {
        Assert.Equal(expected, value.IsValidPhone(Us));
        Assert.Equal(expected, value.IsValidMobilePhone(Us));
    }

    [Fact]
    public void Phone_ShouldFormatAndConvertToE164()
    {
        Assert.Equal("(212) 555-0123", "+1 212 555 0123".FormatPhone(Us));
        Assert.Equal("+12125550123", "(212) 555-0123".ToE164(Us));
        Assert.Null("211 555 0123".ToE164(Us));
    }

    [Theory]
    [InlineData("10001", true)]
    [InlineData("10001-1234", true)]
    [InlineData("00000", false)]
    [InlineData("1000", false)]
    [InlineData("10001-12", false)]
    [InlineData("10001 1234", false)]
    [InlineData("1000a", false)]
    [InlineData(null, false)]
    public void IsValidPostalCode_ShouldAcceptZipAndZipPlus4(string? value, bool expected)
    {
        Assert.Equal(expected, value.IsValidPostalCode(Us));
    }

    [Theory]
    [InlineData("123-45-6789", true)]
    [InlineData("123456789", true)]
    [InlineData("078-05-1120", true)]
    [InlineData("000-12-3456", false)]
    [InlineData("666-12-3456", false)]
    [InlineData("912-70-1234", false)]
    [InlineData("123-00-6789", false)]
    [InlineData("123-45-0000", false)]
    [InlineData("12-3456789", false)]
    [InlineData("123-456-789", false)]
    [InlineData(null, false)]
    public void IsValidNationalId_ShouldValidateSsn(string? value, bool expected)
    {
        Assert.Equal(expected, value.IsValidNationalId(Us));
    }

    [Theory]
    [InlineData("123-45-6789", true)]
    [InlineData("912-70-1234", true)]
    [InlineData("912-40-1234", false)]
    [InlineData("12-3456789", true)]
    [InlineData("07-1234567", false)]
    [InlineData("1-23456789", false)]
    [InlineData(null, false)]
    public void IsValidTaxId_ShouldAcceptSsnItinOrEin(string? value, bool expected)
    {
        Assert.Equal(expected, value.IsValidTaxId(Us));
    }

    [Fact]
    public void VehiclePlates_ShouldNotBeSupported()
    {
        Assert.Null(Us.VehiclePlates);
        Assert.Throws<NotSupportedException>(() => "ABC 1234".IsValidVehiclePlate(Us));
    }

    [Fact]
    public void Holidays_ShouldFollowFederalRulesWithObservedDays()
    {
        DateOnly[] dates = [.. Us.Holidays!.GetHolidays(2026).Select(h => h.Date)];

        DateOnly[] expected =
        [
            new(2026, 1, 1), new(2026, 1, 19), new(2026, 2, 16), new(2026, 5, 25), new(2026, 6, 19), new(2026, 7, 3),
            new(2026, 7, 4), new(2026, 9, 7), new(2026, 10, 12), new(2026, 11, 11), new(2026, 11, 26), new(2026, 12, 25),
        ];
        Assert.Equal(expected, dates);
        Assert.Equal("Independence Day (observed)", new DateOnly(2026, 7, 3).GetPublicHoliday(Us)?.Name);
    }

    [Fact]
    public void Holidays_ShouldMoveNewYearToTheYearBeforeWhenItFallsOnSaturday()
    {
        IReadOnlyList<PublicHoliday> holidays2021 = Us.Holidays!.GetHolidays(2021);
        IReadOnlyList<PublicHoliday> holidays2022 = Us.Holidays.GetHolidays(2022);

        Assert.Contains(holidays2021, h => h.Date == new DateOnly(2021, 12, 31));
        Assert.Contains(holidays2021, h => h.Date == new DateOnly(2021, 6, 18));
        Assert.DoesNotContain(holidays2022, h => h.Date.Year != 2022);
        Assert.DoesNotContain(Us.Holidays.GetHolidays(2020), h => h.Name.StartsWith("Juneteenth", StringComparison.Ordinal));
        Assert.Throws<ArgumentOutOfRangeException>(() => Us.Holidays.GetHolidays(1985));
    }

    [Fact]
    public void BusinessDays_ShouldSkipObservedHolidaysAndWeekends()
    {
        Assert.False(new DateTime(2026, 7, 3).IsBusinessDay(Us));
        Assert.Equal(new DateTime(2026, 7, 6), new DateTime(2026, 7, 2).AddBusinessDays(1, Us));
    }

    [Fact]
    public void Regions_ShouldListStatesWithFipsCodesAndAbbreviations()
    {
        Assert.Equal(51, Us.Regions.Count);
        Assert.Equal("California", Us.FindRegion("ca")?.Name);
        Assert.Equal("California", Us.FindRegion(6)?.Name);
        Assert.Equal("NY", Us.FindRegion("new york")?.Abbreviation);
        Assert.Null(Us.FindRegion("XX"));
        Assert.Null(Countries.Turkey.FindRegion("34")?.Abbreviation);
    }

    [Theory]
    [InlineData(0L, "zero")]
    [InlineData(15L, "fifteen")]
    [InlineData(21L, "twenty-one")]
    [InlineData(40L, "forty")]
    [InlineData(100L, "one hundred")]
    [InlineData(123L, "one hundred twenty-three")]
    [InlineData(1_000L, "one thousand")]
    [InlineData(1_000_001L, "one million one")]
    [InlineData(2_500_340L, "two million five hundred thousand three hundred forty")]
    [InlineData(-5L, "minus five")]
    [InlineData(long.MinValue, "minus nine quintillion two hundred twenty-three quadrillion three hundred seventy-two trillion thirty-six billion eight hundred fifty-four million seven hundred seventy-five thousand eight hundred eight")]
    public void ToWords_ShouldSpellAmericanEnglish(long value, string expected)
    {
        Assert.Equal(expected, value.ToWords(Languages.English));
    }

    [Theory]
    [InlineData("123.50", "one hundred twenty-three dollars and fifty cents")]
    [InlineData("1", "one dollar")]
    [InlineData("1.01", "one dollar and one cent")]
    [InlineData("0.5", "fifty cents")]
    [InlineData("0", "zero dollars")]
    [InlineData("-2.25", "minus two dollars and twenty-five cents")]
    public void ToCurrencyWords_ShouldUsePluralsAndConjunction(string amount, string expected)
    {
        Assert.Equal(expected, decimal.Parse(amount, System.Globalization.CultureInfo.InvariantCulture).ToCurrencyWords(Currencies.USDollar));
    }

    [Fact]
    public void ToCurrencyString_ShouldUseDollarFormat()
    {
        Assert.Equal("$1,234.50", 1234.5m.ToCurrencyString(Currencies.USDollar));
        Assert.Equal("1,234.50 USD", 1234.5m.ToCurrencyString(Currencies.USDollar, CurrencyStyle.IsoCode));
        Assert.Equal("yüz yirmi üç Türk lirası elli kuruş", 123.50m.ToCurrencyWords(Currencies.TurkishLira));
    }

    [Fact]
    public void EnglishText_ShouldCaseAndSortWithoutCulture()
    {
        Assert.Equal("Hello World", "hello WORLD".ToTitleCase(Languages.English));
        Assert.Equal("ISTANBUL", "istanbul".ToUpper(Languages.English));
        Assert.True("HELLO".EqualsIgnoreCase("hello", Languages.English));

        string[] words = ["banana", "Apple", "Cherry", "apple"];
        Array.Sort(words, Languages.English.TextRules!.Comparer);
        Assert.Equal(["apple", "Apple", "banana", "Cherry"], words);
    }

    private sealed class Customer
    {
        [PhoneNumber("US")]
        public string? Phone { get; set; }

        [PostalCode("US")]
        public string? Zip { get; set; }

        [NationalId("US")]
        public string? Ssn { get; set; }

        [TaxId("US")]
        public string? TaxId { get; set; }
    }

    [Fact]
    public void Attributes_ShouldResolveTheUnitedStatesByCode()
    {
        Assert.Same(Us, Countries.Get("us"));

        var valid = new Customer { Phone = "(212) 555-0123", Zip = "10001", Ssn = "123-45-6789", TaxId = "12-3456789" };
        Assert.True(Validator.TryValidateObject(valid, new ValidationContext(valid), null, validateAllProperties: true));

        var invalid = new Customer { Phone = "211 555 0123", Zip = "00000", Ssn = "666-12-3456", TaxId = "07-1234567" };
        var results = new List<ValidationResult>();
        Assert.False(Validator.TryValidateObject(invalid, new ValidationContext(invalid), results, validateAllProperties: true));
        Assert.Equal(4, results.Count);
    }
}
