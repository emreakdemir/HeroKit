# Form doğrulama

HeroKit doğrulayıcıları için DataAnnotations öznitelikleri. ASP.NET Core model doğrulaması, Blazor'un `DataAnnotationsValidator`'ı, minimal API filtreleri ve `Validator.TryValidateObject` ile çalışırlar.

```csharp
using System.ComponentModel.DataAnnotations;
using HeroKit.Validation;

public sealed class Customer
{
    [Required, Tckn]
    public string? IdentityNumber { get; set; }

    [TaxId("TR")]
    public string? TaxId { get; set; }

    [Iban]
    public string? Iban { get; set; }

    [PhoneNumber("TR", MobileOnly = true, ErrorMessage = "Cep telefonu geçersiz.")]
    public string? Mobile { get; set; }

    [PostalCode("TR")]
    public string? PostalCode { get; set; }

    [VehiclePlate("TR")]
    public string? Plate { get; set; }

    [CardNumber]
    public string? Card { get; set; }

    [GibInvoiceNumber]
    public string? InvoiceNumber { get; set; }

    [Ettn]
    public string? Ettn { get; set; }
}
```

| Öznitelik | Kontrol eder |
|---|---|
| `[Tckn]` | Türkiye kimlik numarası sağlama toplamı |
| `[Vkn]` | Türkiye vergi numarası sağlama toplamı |
| `[Iban]` | ISO 13616 mod-97; ülkenin IBAN kuralı varsa ulusal uzunluk (TR: 26) |
| `[CardNumber]` | Uzunluk ve Luhn |
| `[GibInvoiceNumber]` | GİB biçiminde e-fatura / e-arşiv fatura numarası, örneğin `ABC2026000000001` |
| `[Ettn]` | ETTN: 8-4-4-4-12 biçiminde boş olmayan bir UUID |
| `[PhoneNumber("TR")]` | Bir ülkenin telefon numarası; cep telefonları için `MobileOnly = true` |
| `[VehiclePlate("TR")]` | Bir ülkenin araç plakası |
| `[PostalCode("TR")]` | Bir ülkenin posta kodu (TR: 5 rakam, geçerli il öneki) |
| `[NationalId("TR")]` | Bir ülkenin kimlik numarası (TR: TCKN) |
| `[TaxId("TR")]` | Bir ülkenin vergi numarası (TR: VKN veya TCKN) |

Ülke öznitelikleri ISO 3166-1 alpha-2 kodu alır. Ülke, değer doğrulanırken `Countries` içinde aranır; bu yüzden `Countries.Register` ile eklediğiniz ülke de çalışır. Bkz. [Ülkeler ve diller](countries-and-languages.md). Gereken kurala sahip olmayan bir ülke `NotSupportedException` fırlatır.

## FluentValidation

Ayrı `HeroKit.FluentValidation` paketi aynı kontrolleri FluentValidation kuralları olarak ekler. Çekirdek paket bağımlılıksız kalır.

```bash
dotnet add package HeroKit.FluentValidation
```

```csharp
using FluentValidation;
using HeroKit.FluentValidation;
using HeroKit.Globalization;

public sealed class CustomerValidator : AbstractValidator<Customer>
{
    public CustomerValidator()
    {
        RuleFor(x => x.IdentityNumber).NotEmpty().Tckn();
        RuleFor(x => x.Iban).Iban();
        RuleFor(x => x.Mobile).PhoneNumber(Countries.Turkey, mobileOnly: true);
        RuleFor(x => x.Plate).VehiclePlate(Countries.Turkey);
        RuleFor(x => x.InvoiceNumber).GibInvoiceNumber();
    }
}
```

Kurallar: `Tckn`, `Vkn`, `Iban`, `CardNumber`, `GibInvoiceNumber`, `Ettn` ve ülke kuralları `PhoneNumber(country, mobileOnly)`, `VehiclePlate(country)`, `PostalCode(country)`, `NationalId(country)`, `TaxId(country)`. Mesajlar `{PropertyName}` kullanır; değiştirmek için `WithMessage` kullanın.

## Notlar

Null ve boş değerler geçerlidir, bu yüzden isteğe bağlı alanlar isteğe bağlı kalır. Değer zorunlu olsun istiyorsanız `[Required]` ekleyin. Varsayılan mesajlar İngilizcedir ve alan adını içerir; yerelleştirmek için `ErrorMessage` ya da kaynak (resource) özelliklerini ayarlayın.
