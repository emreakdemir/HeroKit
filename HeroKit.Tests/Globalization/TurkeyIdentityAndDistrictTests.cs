using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using FluentValidation;
using HeroKit.FluentValidation;
using HeroKit.Globalization;
using HeroKit.Globalization.Turkey;
using HeroKit.Validation;

namespace HeroKit.Tests.Globalization;

public class TurkeyIdentityAndDistrictTests
{
    [Fact]
    public void Districts_ShouldListAllTurkishDistricts()
    {
        IReadOnlyList<District> all = Countries.Turkey.Districts;

        Assert.Equal(973, all.Count);
        Assert.Equal(973, all.Select(d => d.Code).Distinct().Count());
        Assert.All(all, d => Assert.NotNull(Countries.Turkey.FindRegion(d.RegionCode)));
        Assert.Equal(81, all.Select(d => d.RegionCode).Distinct().Count());
        Assert.Empty(Countries.UnitedStates.Districts);
    }

    [Theory]
    [InlineData(34, 39)]
    [InlineData(6, 25)]
    [InlineData(35, 30)]
    [InlineData(74, 4)]
    [InlineData(99, 0)]
    public void GetDistricts_ShouldReturnTheDistrictsOfAProvince(int province, int count)
    {
        Assert.Equal(count, Countries.Turkey.GetDistricts(province).Count);
    }

    [Fact]
    public void GetDistricts_ShouldAcceptProvinceNames()
    {
        IReadOnlyList<District> istanbul = Countries.Turkey.GetDistricts("istanbul");

        Assert.Equal(39, istanbul.Count);
        Assert.Equal("Adalar", istanbul[0].Name);
        Assert.Equal(istanbul.Select(d => d.Name).Order(Languages.Turkish.TextRules!.Comparer), istanbul.Select(d => d.Name));
        Assert.Equal(25, Countries.Turkey.GetDistricts("06").Count);
        Assert.Empty(Countries.Turkey.GetDistricts("Atlantis"));
        Assert.Empty(Countries.Turkey.GetDistricts(null));
    }

    [Theory]
    [InlineData("istanbul", "kadikoy", "Kadıköy", 1421)]
    [InlineData("34", "KADIKÖY", "Kadıköy", 1421)]
    [InlineData("ankara", "Çankaya", "Çankaya", 1231)]
    [InlineData("Denizli", "merkezefendi", "Merkezefendi", 2079)]
    [InlineData("adana", "aladag", "Aladağ", 1757)]
    public void FindDistrict_ShouldIgnoreCaseAndDiacritics(string province, string district, string name, int code)
    {
        District? found = Countries.Turkey.FindDistrict(province, district);

        Assert.Equal(name, found?.Name);
        Assert.Equal(code, found?.Code);
        Assert.Equal(found, Countries.Turkey.FindDistrict(code));
    }

    [Theory]
    [InlineData("ankara", "kadikoy")]
    [InlineData("atlantis", "kadikoy")]
    [InlineData("istanbul", "")]
    [InlineData("istanbul", null)]
    [InlineData(null, "kadikoy")]
    public void FindDistrict_ShouldReturnNullForUnknownPairs(string? province, string? district)
    {
        Assert.Null(Countries.Turkey.FindDistrict(province, district));
        Assert.Null(Countries.Turkey.FindDistrict(1));
    }

    [Theory]
    [InlineData("firma@hs01.kep.tr", true)]
    [InlineData("ad.soyad@hs03.kep.tr", true)]
    [InlineData("FIRMA@HS01.KEP.TR", true)]
    [InlineData("firma-ltd_2026@hs01.kep.tr", true)]
    [InlineData("firma@kep.tr", false)]
    [InlineData("firma@hs01.kep.tr.com", false)]
    [InlineData("firma@example.com", false)]
    [InlineData("@hs01.kep.tr", false)]
    [InlineData("a@b@hs01.kep.tr", false)]
    [InlineData("ad soyad@hs01.kep.tr", false)]
    [InlineData(".firma@hs01.kep.tr", false)]
    [InlineData("fir..ma@hs01.kep.tr", false)]
    [InlineData("firma@-hs01.kep.tr", false)]
    [InlineData("firma@hs01..kep.tr", false)]
    [InlineData("firma@hs_01.kep.tr", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void IsValidKepAddress_ShouldRequireAKepDomain(string? value, bool expected)
    {
        Assert.Equal(expected, value.IsValidKepAddress());
    }

    [Theory]
    [InlineData("A12B34567", true)]
    [InlineData("Z99Z00000", true)]
    [InlineData("a12b34567", false)]
    [InlineData("A12B3456", false)]
    [InlineData("A12B345678", false)]
    [InlineData("112B34567", false)]
    [InlineData("A1BB34567", false)]
    [InlineData("A12B3456X", false)]
    [InlineData("A12 B34567", false)]
    [InlineData(null, false)]
    public void IsValidIdCardSerial_ShouldCheckTheChipCardFormat(string? value, bool expected)
    {
        Assert.Equal(expected, value.IsValidIdCardSerial());
    }

    [Fact]
    public void Tckn_ShouldParseOnlyValidNumbers()
    {
        Tckn id = Tckn.Parse("10000000146");

        Assert.Equal("10000000146", id.Value);
        Assert.Equal("10000000146", id.ToString());
        Assert.Equal("10*******46", id.ToMaskedString());
        Assert.False(id.IsEmpty);
        Assert.True(Tckn.TryParse("10000000146", out Tckn same));
        Assert.Equal(id, same);
        Assert.True(id == same);
        Assert.False(id != same);
        Assert.Equal(id.GetHashCode(), same.GetHashCode());
        Assert.True(id.Equals((object)same));
        Assert.False(id.Equals("10000000146"));
        string text = id;
        Assert.Equal("10000000146", text);
        Assert.Equal(id, (Tckn)"10000000146");

        Assert.False(Tckn.TryParse("10000000147", out Tckn invalid));
        Assert.True(invalid.IsEmpty);
        Assert.False(Tckn.TryParse(null, out _));
        Assert.Throws<FormatException>(() => Tckn.Parse("123"));
        Assert.Throws<ArgumentNullException>(() => Tckn.Parse(null!));
        Assert.Throws<FormatException>(() => (Tckn)"abc");
    }

    [Fact]
    public void Tckn_DefaultShouldBeEmpty()
    {
        Tckn empty = default;

        Assert.True(empty.IsEmpty);
        Assert.Equal(string.Empty, empty.Value);
        Assert.Equal(string.Empty, empty.ToMaskedString());
        Assert.Equal(0, empty.GetHashCode());
        Assert.NotEqual(empty, Tckn.Parse("10000000146"));
        Assert.True(empty.CompareTo(Tckn.Parse("10000000146")) < 0);
    }

    private static T ParseVia<T>(string value) where T : IParsable<T> => T.Parse(value, null);

    private static bool TryParseVia<T>(string? value, out T? result) where T : IParsable<T> => T.TryParse(value, null, out result);

    [Fact]
    public void Tckn_ShouldWorkAsIParsable()
    {
        Assert.Equal("10000000146", ParseVia<Tckn>("10000000146").Value);
        Assert.False(TryParseVia<Tckn>("1", out _));
    }

    private sealed record Person(Tckn Id, string Name);

    [Fact]
    public void Tckn_ShouldRoundTripThroughJson()
    {
        string json = JsonSerializer.Serialize(new Person(Tckn.Parse("10000000146"), "Ayşe"));

        Assert.Contains("\"Id\":\"10000000146\"", json);
        Assert.Equal(Tckn.Parse("10000000146"), JsonSerializer.Deserialize<Person>(json)!.Id);
        Assert.Equal("{\"Id\":null,\"Name\":\"x\"}", JsonSerializer.Serialize(new Person(default, "x")));
        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<Person>("{\"Id\":\"10000000147\",\"Name\":\"x\"}"));
        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<Person>("{\"Id\":10000000146,\"Name\":\"x\"}"));
    }

    [Fact]
    public void Tckn_ShouldHaveATypeConverter()
    {
        TypeConverter converter = TypeDescriptor.GetConverter(typeof(Tckn));

        Assert.True(converter.CanConvertFrom(typeof(string)));
        Assert.True(converter.CanConvertTo(typeof(string)));
        Assert.Equal(Tckn.Parse("10000000146"), converter.ConvertFromInvariantString(" 10000000146 "));
        Assert.Equal("10000000146", converter.ConvertToInvariantString(Tckn.Parse("10000000146")));
        Assert.Throws<FormatException>(() => converter.ConvertFromInvariantString("123"));
    }

    // [Tckn] must still resolve to the attribute when the Tckn type is in scope too.
    private sealed class Applicant
    {
        [Tckn]
        public string? Id { get; set; }

        [KepAddress]
        public string? Kep { get; set; }

        [IdCardSerial]
        public string? CardSerial { get; set; }
    }

    private sealed class ApplicantValidator : AbstractValidator<Applicant>
    {
        public ApplicantValidator()
        {
            RuleFor(x => x.Kep).KepAddress();
            RuleFor(x => x.CardSerial).IdCardSerial();
        }
    }

    [Theory]
    [InlineData("10000000146", "firma@hs01.kep.tr", "A12B34567", 0, 0)]
    [InlineData(null, "", null, 0, 0)]
    [InlineData("10000000147", "firma@example.com", "a12b34567", 3, 2)]
    public void AttributesAndRules_ShouldValidateKepAndCardSerial(string? id, string? kep, string? serial, int attributeErrors, int ruleErrors)
    {
        var applicant = new Applicant { Id = id, Kep = kep, CardSerial = serial };

        var results = new List<ValidationResult>();
        Validator.TryValidateObject(applicant, new ValidationContext(applicant), results, validateAllProperties: true);
        Assert.Equal(attributeErrors, results.Count);
        Assert.Equal(ruleErrors, new ApplicantValidator().Validate(applicant).Errors.Count);
        Assert.False(new KepAddressAttribute().IsValid(42));
        Assert.False(new IdCardSerialAttribute().IsValid(42));
    }
}
