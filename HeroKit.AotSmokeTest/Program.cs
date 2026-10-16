using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HeroKit.Banking;
using HeroKit.Enums;
using HeroKit.FileTypes;
using HeroKit.Objects;
using HeroKit.Properties;
using HeroKit.Strings;
using HeroKit.Globalization;
using HeroKit.Globalization.Turkey;
using HeroKit.Types;
using HeroKit.Validation;

// Exercises the public API under Native AOT and exits with a non-zero code on any unexpected result.
var checks = new (string Name, bool Passed)[]
{
    ("Enum ToList", string.Join(",", EnumExtensions.ToList<DayOfWeek>()) == "Sunday,Monday,Tuesday,Wednesday,Thursday,Friday,Saturday"),
    ("ToEnum", "Friday".ToEnum<DayOfWeek>() == DayOfWeek.Friday),
    ("GetDescription", Status.Shipped.GetDescription() == "Kargoya verildi"),
    ("To<T>", "2026-03-07".To<DateOnly>() == new DateOnly(2026, 3, 7)),
    ("ConvertTo", ((object)"42").ConvertTo<int>() == 42),
    ("TrySetConvertedValue", SetQuantity("42") == 42),
    ("ToSlug", "Çağdaş Şehir İstanbul".ToSlug() == "cagdas-sehir-istanbul"),
    ("GetDefault", (int)typeof(int).GetDefault()! == 0),
    ("ToDecimal", "1.234,56".ToDecimal() == 1234.56m),
    ("TCKN", "10000000146".IsValidTckn()),
    ("IBAN", "TR330006100519786457841326".FormatIban() == "TR33 0006 1005 1978 6457 8413 26"),
    ("Card brand", "4111111111111111".GetCardBrand() == CardBrand.Visa),
    ("Plate", "34abc123".FormatVehiclePlate(Countries.Turkey) == "34 ABC 123"),
    ("Lira", 1234.5m.ToCurrencyString(Currencies.TurkishLira) == "₺1.234,50"),
    ("Words", 123.50m.ToCurrencyWords(Currencies.TurkishLira) == "yüz yirmi üç Türk lirası elli kuruş"),
    ("Holidays", new DateOnly(2026, 3, 19).AddBusinessDays(1, Countries.Turkey) == new DateOnly(2026, 3, 23)),
    ("Turkish business data", "TR78 0006 2000 0000 0006 2950 05".GetBank()?.Code == "00062"
        && "₺1.234,56".ToAmount(Currencies.TurkishLira) == 1234.56m
        && "0879001756600379".IsValidMersis()
        && GibInvoiceQrCode.TryParse("""{"vkntckn":"4840847211","avkntckn":"11111111111","no":"ABC2026000000001","ettn":"51d3bb18-9650-4c8e-825d-5a7f845be630","tarih":"2026-10-15","odenecek":"120.00"}""", out GibInvoiceQrCode? qr)
        && qr.PayableAmount == 120m),
    ("File types", FileTypes.Detect([0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A]) == FileTypes.Png
        && FileTypes.GetMimeType("report.docx") == FileTypes.Docx.MimeType
        && FileTypes.CheckUpload("MZ\u0090\u0000"u8, "photo.png").Rejection == UploadRejection.DangerousContent
        && !FileTypes.CheckUpload("<svg onload=\"x\"/>"u8, "icon.svg").IsSafe),
    ("United States", "(212) 555-0123".ToE164(Countries.UnitedStates) == "+12125550123"
        && 1.01m.ToCurrencyWords(Currencies.USDollar) == "one dollar and one cent"
        && new DateOnly(2026, 7, 2).AddBusinessDays(1, Countries.UnitedStates) == new DateOnly(2026, 7, 6)),
    ("Title case", "istanbul ılıca".ToTitleCase(Languages.Turkish) == "İstanbul Ilıca"),
    ("Country attribute", new PhoneNumberAttribute("TR").IsValid("0532 123 45 67") && !new PhoneNumberAttribute("TR") { MobileOnly = true }.IsValid("0212 555 12 34")),
    ("Attribute", new TcknAttribute().IsValid("10000000146") && !new IbanAttribute().IsValid("TR00")),
};

foreach (var (name, passed) in checks)
    Console.WriteLine($"{(passed ? "PASS" : "FAIL")} {name}");

return checks.All(c => c.Passed) ? 0 : 1;

static int SetQuantity(string value)
{
    var order = new Order();
    typeof(Order).GetProperty(nameof(Order.Quantity))!.TrySetConvertedValue(order, value);
    return order.Quantity;
}

enum Status
{
    [Description("Kargoya verildi")]
    Shipped,
}

sealed class Order
{
    public int Quantity { get; set; }
}
