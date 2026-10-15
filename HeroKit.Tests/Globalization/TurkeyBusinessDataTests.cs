using System.ComponentModel.DataAnnotations;
using FluentValidation;
using HeroKit.Banking;
using HeroKit.Barcodes;
using HeroKit.FluentValidation;
using HeroKit.Globalization;
using HeroKit.Globalization.Turkey;
using HeroKit.Strings;
using HeroKit.Validation;

namespace HeroKit.Tests.Globalization;

public class TurkeyBusinessDataTests
{
    [Theory]
    [InlineData("TR78 0006 2000 0000 0006 2950 05", "Türkiye Garanti Bankası A.Ş.")]
    [InlineData("TR850001000000000012345678", "T.C. Ziraat Bankası A.Ş.")]
    [InlineData("tr25 0009 9012 3456 7890 1234 56", "ING Bank A.Ş.")]
    [InlineData("TR330006100519786457841326", null)]
    [InlineData("TR780006200000000006295006", null)]
    [InlineData("DE89370400440532013000", null)]
    [InlineData(null, null)]
    public void GetBank_ShouldResolveTheBankCodeOfTurkishIbans(string? iban, string? expected)
    {
        Assert.Equal(expected, iban.GetBank()?.Name);
    }

    [Fact]
    public void Banks_ShouldListTurkishBanksWithFiveDigitCodes()
    {
        IReadOnlyList<Bank> banks = Countries.Turkey.Banks!.All;

        Assert.Equal(30, banks.Count);
        Assert.All(banks, b => Assert.Matches("^[0-9]{5}$", b.Code));
        Assert.Equal(banks.Count, banks.Select(b => b.Code).Distinct().Count());
        Assert.Null(Countries.UnitedStates.Banks);
    }

    [Theory]
    [InlineData("10000000146", 2, 2, "10*******46")]
    [InlineData("TR33 0006 1005 1978 6457 8413 26", 4, 4, "TR33 **** **** **** **** **13 26")]
    [InlineData("0532 123 45 67", 4, 2, "0532 *** ** 67")]
    [InlineData("1234", 2, 2, "****")]
    [InlineData("", 2, 2, "")]
    public void MaskMiddle_ShouldKeepBothEndsAndHideShortValues(string value, int start, int end, string expected)
    {
        Assert.Equal(expected, value.MaskMiddle(start, end));
    }

    [Fact]
    public void MaskMiddle_ShouldHandleNullAndRejectNegativeCounts()
    {
        Assert.Null(((string?)null).MaskMiddle(1, 1));
        Assert.Throws<ArgumentOutOfRangeException>(() => "abc".MaskMiddle(-1, 1));
        Assert.Throws<ArgumentOutOfRangeException>(() => "abc".MaskMiddle(1, -1));
        Assert.Equal("1#####6", "1234566".MaskMiddle(1, 1, '#'));
    }

    [Theory]
    [InlineData("emre.akdemir@example.com", "e***********@example.com")]
    [InlineData("a@b.com", "*@b.com")]
    [InlineData("noatsign", "********")]
    [InlineData("@x.com", "******")]
    [InlineData("user@", "*****")]
    [InlineData(null, null)]
    public void MaskEmail_ShouldKeepTheFirstCharacterAndTheDomain(string? value, string? expected)
    {
        Assert.Equal(expected, value.MaskEmail());
    }

    [Theory]
    [InlineData("Ayşe Nur Yılmaz", "A*** N** Y*****")]
    [InlineData("  Emre  ", "  E***  ")]
    [InlineData(null, null)]
    public void MaskName_ShouldKeepTheFirstLetterOfEachWord(string? value, string? expected)
    {
        Assert.Equal(expected, value.MaskName());
    }

    [Theory]
    [InlineData("₺1.234,56", "1234.56")]
    [InlineData("1.234,56 TL", "1234.56")]
    [InlineData("1.234.567,89 TRY", "1234567.89")]
    [InlineData("-1.234,5", "-1234.5")]
    [InlineData("₺-10", "-10")]
    [InlineData("- ₺10", "-10")]
    [InlineData("1234,5", "1234.5")]
    [InlineData("1.234", "1234")]
    [InlineData(" 0,05 tl ", "0.05")]
    public void TryToAmount_ShouldReadTurkishAmounts(string value, string expected)
    {
        Assert.True(value.TryToAmount(Currencies.TurkishLira, out decimal amount));
        Assert.Equal(decimal.Parse(expected, System.Globalization.CultureInfo.InvariantCulture), amount);
    }

    [Theory]
    [InlineData("1.5")]
    [InlineData("12.34,5")]
    [InlineData("1,2,3")]
    [InlineData("1.234,")]
    [InlineData("1,234.56")]
    [InlineData(".234")]
    [InlineData("abc")]
    [InlineData("₺")]
    [InlineData("")]
    [InlineData(null)]
    public void TryToAmount_ShouldRejectAmbiguousOrInvalidText(string? value)
    {
        Assert.False(value.TryToAmount(Currencies.TurkishLira, out decimal amount));
        Assert.Equal(0m, amount);
    }

    [Fact]
    public void ToAmount_ShouldUseTheCurrencySeparators()
    {
        Assert.Equal(1234.56m, "$1,234.56".ToAmount(Currencies.USDollar));
        Assert.Equal(1234.56m, "1,234.56 USD".ToAmount(Currencies.USDollar));
        Assert.Equal(5m, "x".ToAmount(Currencies.TurkishLira, 5m));
    }

    [Theory]
    [InlineData("0879001756600379", true)]
    [InlineData("0015001526400497", true)]
    [InlineData("0879001756700379", false)]
    [InlineData("1879001756600379", false)]
    [InlineData("087900175660037", false)]
    [InlineData("08790017566003a9", false)]
    [InlineData(null, false)]
    public void IsValidMersis_ShouldCheckTheEmbeddedVkn(string? value, bool expected)
    {
        Assert.Equal(expected, value.IsValidMersis());
    }

    [Theory]
    [InlineData("8691234567890", true)]
    [InlineData("8691234567891", false)]
    [InlineData("4006381333931", true)]
    [InlineData("96385074", true)]
    [InlineData("036000291452", true)]
    [InlineData("10614141000415", true)]
    [InlineData("12345", false)]
    [InlineData("869123456789a", false)]
    [InlineData(null, false)]
    public void IsValidGtin_ShouldCheckTheGs1CheckDigit(string? value, bool expected)
    {
        Assert.Equal(expected, value.IsValidGtin());
    }

    private const string QrJson = """
        {"vkntckn":"4840847211","avkntckn":"11111111111","senaryo":"EARSIVFATURA","tip":"SATIS","tarih":"2026-10-15",
         "no":"ABC2026000000001","ettn":"51d3bb18-9650-4c8e-825d-5a7f845be630","parabirimi":"TRY","malhizmettoplam":"100.00",
         "kdvmatrah(20)":"100.00","hesaplanankdv(20)":"20.00","kdvmatrah(10)":"50,00","hesaplanankdv(10)":5,"vergidahil":"175.00","odenecek":"175.00"}
        """;

    [Fact]
    public void QrCode_ShouldReadTheGibFields()
    {
        Assert.True(GibInvoiceQrCode.TryParse(QrJson, out GibInvoiceQrCode? qr));

        Assert.Equal("4840847211", qr.SellerTaxId);
        Assert.Equal("11111111111", qr.BuyerTaxId);
        Assert.Equal("ABC2026000000001", qr.Number.ToString());
        Assert.Equal(Guid.Parse("51d3bb18-9650-4c8e-825d-5a7f845be630"), qr.Ettn);
        Assert.Equal(new DateOnly(2026, 10, 15), qr.Date);
        Assert.Equal("EARSIVFATURA", qr.Scenario);
        Assert.Equal("SATIS", qr.InvoiceType);
        Assert.Equal("TRY", qr.CurrencyCode);
        Assert.Equal(100m, qr.LineTotal);
        Assert.Equal(175m, qr.TaxInclusiveTotal);
        Assert.Equal(175m, qr.PayableAmount);
        Assert.Equal(100m, qr.VatBases[20m]);
        Assert.Equal(50m, qr.VatBases[10m]);
        Assert.Equal(20m, qr.VatAmounts[20m]);
        Assert.Equal(5m, qr.VatAmounts[10m]);
        Assert.Equal("100.00", qr.Fields["malhizmettoplam"]);
    }

    [Fact]
    public void QrCode_ShouldAcceptUpperCaseKeysAndOtherDateFormats()
    {
        string json = """{"VKNTCKN":"4840847211","AVKNTCKN":"10000000146","NO":"ABC2026000000002","ETTN":"51d3bb18-9650-4c8e-825d-5a7f845be630","TARIH":"15.10.2026","extra":{"a":[1,2]}}""";

        Assert.True(GibInvoiceQrCode.TryParse(json, out GibInvoiceQrCode? qr));
        Assert.Equal(new DateOnly(2026, 10, 15), qr.Date);
        Assert.Null(qr.Scenario);
        Assert.Null(qr.PayableAmount);
        Assert.Empty(qr.VatAmounts);
    }

    [Theory]
    [InlineData("""{"vkntckn":"4840847212","avkntckn":"11111111111","no":"ABC2026000000001","ettn":"51d3bb18-9650-4c8e-825d-5a7f845be630","tarih":"2026-10-15"}""")]
    [InlineData("""{"vkntckn":"4840847211","avkntckn":"123","no":"ABC2026000000001","ettn":"51d3bb18-9650-4c8e-825d-5a7f845be630","tarih":"2026-10-15"}""")]
    [InlineData("""{"vkntckn":"4840847211","avkntckn":"11111111111","no":"ABC2026000000000","ettn":"51d3bb18-9650-4c8e-825d-5a7f845be630","tarih":"2026-10-15"}""")]
    [InlineData("""{"vkntckn":"4840847211","avkntckn":"11111111111","no":"ABC2026000000001","ettn":"00000000-0000-0000-0000-000000000000","tarih":"2026-10-15"}""")]
    [InlineData("""{"vkntckn":"4840847211","avkntckn":"11111111111","no":"ABC2026000000001","ettn":"51d3bb18-9650-4c8e-825d-5a7f845be630","tarih":"2026-13-15"}""")]
    [InlineData("""{"vkntckn":"4840847211","avkntckn":"11111111111","no":"ABC2026000000001","ettn":"51d3bb18-9650-4c8e-825d-5a7f845be630"}""")]
    [InlineData("""[1,2,3]""")]
    [InlineData("""{"vkntckn":""")]
    [InlineData("not json")]
    [InlineData("")]
    [InlineData(null)]
    public void QrCode_ShouldRejectInvalidContent(string? json)
    {
        Assert.False(GibInvoiceQrCode.TryParse(json, out GibInvoiceQrCode? qr));
        Assert.Null(qr);
    }

    private sealed class Company
    {
        [Mersis]
        public string? Mersis { get; set; }

        [Gtin]
        public string? Barcode { get; set; }
    }

    private sealed class CompanyValidator : AbstractValidator<Company>
    {
        public CompanyValidator()
        {
            RuleFor(x => x.Mersis).Mersis();
            RuleFor(x => x.Barcode).Gtin();
        }
    }

    [Theory]
    [InlineData("0879001756600379", "8691234567890", 0)]
    [InlineData(null, "", 0)]
    [InlineData("0879001756700379", "8691234567891", 2)]
    public void AttributesAndRules_ShouldValidateMersisAndGtin(string? mersis, string? barcode, int errors)
    {
        var company = new Company { Mersis = mersis, Barcode = barcode };

        var results = new List<ValidationResult>();
        Validator.TryValidateObject(company, new ValidationContext(company), results, validateAllProperties: true);
        Assert.Equal(errors, results.Count);
        Assert.Equal(errors, new CompanyValidator().Validate(company).Errors.Count);
    }

    [Fact]
    public void Attributes_ShouldRejectNonStringValues()
    {
        Assert.False(new MersisAttribute().IsValid(879001756600379L));
        Assert.False(new GtinAttribute().IsValid(8691234567890L));
    }
}
