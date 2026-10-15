using System.ComponentModel.DataAnnotations;
using HeroKit.Banking;
using HeroKit.Globalization;
using HeroKit.Strings;
using HeroKit.Validation;

namespace HeroKit.Tests.Globalization;

public class CountryModelTests
{
    // A minimal custom country: only phone rules, registered under a private-use code.
    private sealed class DigitsOnlyPhoneRules : IPhoneNumberRules
    {
        public bool IsValid(string? value) => value is { Length: 6 } && value.All(char.IsAsciiDigit);
        public bool IsMobile(string? value) => IsValid(value) && value![0] == '9';
        public string? ToE164(string? value) => IsValid(value) ? "+999" + value : null;
        public string? Format(string? value) => IsValid(value) ? $"{value![..3]} {value[3..]}" : null;
    }

    private static readonly Country Testland = new("XT", "Testland", Languages.Turkish) { PhoneNumbers = new DigitsOnlyPhoneRules() };

    private sealed class Contact
    {
        [PhoneNumber("XT")]
        public string? Phone { get; set; }
    }

    [Fact]
    public void CustomCountry_ShouldWorkWithoutChangingTheLibrary()
    {
        Countries.Register(Testland);

        Assert.Same(Testland, Countries.Get("xt"));
        Assert.True("912345".IsValidPhone(Testland));
        Assert.True("912345".IsValidMobilePhone(Testland));
        Assert.Equal("+999912345", "912345".ToE164(Testland));
        Assert.Equal("912 345", "912345".FormatPhone(Testland));

        var contact = new Contact { Phone = "12" };
        var results = new List<ValidationResult>();
        Assert.False(Validator.TryValidateObject(contact, new ValidationContext(contact), results, validateAllProperties: true));
        Assert.Single(results);
    }

    [Fact]
    public void MissingRules_ShouldThrowNotSupported()
    {
        var bare = new Country("XB", "Bareland", Languages.Turkish);

        NotSupportedException error = Assert.Throws<NotSupportedException>(() => "34 ABC 123".IsValidVehiclePlate(bare));
        Assert.Contains("Bareland", error.Message);
        Assert.Throws<NotSupportedException>(() => new DateOnly(2026, 1, 2).IsBusinessDay(bare));
        Assert.Throws<NotSupportedException>(() => "x".IsValidPostalCode(bare));
        Assert.Throws<NotSupportedException>(() => 5L.ToWords(new Language("xx", "None")));
    }

    [Fact]
    public void Registry_ShouldResolveTurkeyAndRejectUnknownCodes()
    {
        Assert.Same(Countries.Turkey, Countries.Get("TR"));
        Assert.Contains(Countries.Turkey, Countries.All);
        Assert.False(Countries.TryGet("ZZ", out _));
        Assert.Throws<KeyNotFoundException>(() => Countries.Get("ZZ"));
    }

    [Theory]
    [InlineData("34", 34, "İstanbul")]
    [InlineData("06", 6, "Ankara")]
    [InlineData("istanbul", 34, "İstanbul")]
    [InlineData("ISTANBUL", 34, "İstanbul")]
    [InlineData("sanliurfa", 63, "Şanlıurfa")]
    [InlineData(" Iğdır ", 76, "Iğdır")]
    public void FindRegion_ShouldMatchCodesAndNames(string query, int code, string name)
    {
        Assert.Equal(new Region(code, name), Countries.Turkey.FindRegion(query));
    }

    [Theory]
    [InlineData("")]
    [InlineData("82")]
    [InlineData("Atlantis")]
    [InlineData(null)]
    public void FindRegion_ShouldReturnNullForUnknownValues(string? query)
    {
        Assert.Null(Countries.Turkey.FindRegion(query));
    }

    [Fact]
    public void Iban_ShouldApplyRegisteredCountryRules()
    {
        Assert.True("TR330006100519786457841326".IsValidIban());
        Assert.False("TR3300061005197864578413".IsValidIban()); // TR length comes from Countries.Turkey
        Assert.True("DE89370400440532013000".IsValidIban());     // no German rules registered: checksum only
    }

    [Theory]
    [InlineData("1234.5", CurrencyStyle.Symbol, "₺1.234,50")]
    [InlineData("1234.5", CurrencyStyle.Abbreviation, "1.234,50 TL")]
    [InlineData("1234.5", CurrencyStyle.IsoCode, "1.234,50 TRY")]
    [InlineData("-0.005", CurrencyStyle.Symbol, "-₺0,01")]
    public void ToCurrencyString_ShouldUseCurrencyNotation(string amount, CurrencyStyle style, string expected)
    {
        decimal value = decimal.Parse(amount, System.Globalization.CultureInfo.InvariantCulture);

        Assert.Equal(expected, value.ToCurrencyString(Currencies.TurkishLira, style));
    }

    [Theory]
    [InlineData("istanbul", "İSTANBUL", "istanbul")]
    [InlineData("IŞIK", "IŞIK", "ışık")]
    public void UpperAndLower_ShouldUseLanguageRules(string value, string upper, string lower)
    {
        Assert.Equal(upper, value.ToUpper(Languages.Turkish));
        Assert.Equal(lower, value.ToLower(Languages.Turkish));
    }

    [Theory]
    [InlineData("Çağdaş Şehir İstanbul", "Cagdas Sehir Istanbul")]
    [InlineData("Ilıca", "Ilica")]
    [InlineData("Łódź Kraków", "Lodz Krakow")]
    [InlineData("Señor Müller", "Senor Muller")]
    [InlineData("こんにちは", "こんにちは")]
    [InlineData(null, "")]
    public void RemoveDiacritics_ShouldUseTheFixedTable(string? value, string expected)
    {
        Assert.Equal(expected, value.RemoveDiacritics());
    }
}
