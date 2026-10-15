# Form doğrulama

HeroKit doğrulayıcıları için DataAnnotations öznitelikleri. ASP.NET Core model doğrulaması, Blazor'un `DataAnnotationsValidator`'ı, minimal API filtreleri ve `Validator.TryValidateObject` ile çalışırlar.

```csharp
using System.ComponentModel.DataAnnotations;
using HeroKit.Validation;

public sealed class Customer
{
    [Required, Tckn]
    public string? IdentityNumber { get; set; }

    [TaxIdTr]
    public string? TaxId { get; set; }

    [Iban]
    public string? Iban { get; set; }

    [TurkishPhone(MobileOnly = true, ErrorMessage = "Cep telefonu geçersiz.")]
    public string? Mobile { get; set; }

    [PostalCodeTr]
    public string? PostalCode { get; set; }

    [PlateTr]
    public string? Plate { get; set; }

    [CardNumber]
    public string? Card { get; set; }
}
```

| Öznitelik | Kontrol eder |
|---|---|
| `[Tckn]` | Türkiye kimlik numarası sağlama toplamı |
| `[Vkn]` | Türkiye vergi numarası sağlama toplamı |
| `[TaxIdTr]` | VKN veya TCKN |
| `[Iban]` | ISO 13616 mod-97; TR için uzunluk |
| `[TurkishPhone]` | Türkiye telefon numarası; cep telefonları için `MobileOnly = true` |
| `[PostalCodeTr]` | 5 rakam, geçerli il öneki |
| `[PlateTr]` | Sivil plaka biçimi |
| `[CardNumber]` | Uzunluk ve Luhn |

## FluentValidation

Ayrı `HeroKit.FluentValidation` paketi aynı kontrolleri FluentValidation kuralları olarak ekler. Çekirdek paket bağımlılıksız kalır.

```bash
dotnet add package HeroKit.FluentValidation
```

```csharp
using FluentValidation;
using HeroKit.FluentValidation;

public sealed class CustomerValidator : AbstractValidator<Customer>
{
    public CustomerValidator()
    {
        RuleFor(x => x.IdentityNumber).NotEmpty().Tckn();
        RuleFor(x => x.Iban).Iban();
        RuleFor(x => x.Mobile).TurkishPhone(mobileOnly: true);
        RuleFor(x => x.Plate).PlateTr();
    }
}
```

Kurallar: `Tckn`, `Vkn`, `TaxIdTr`, `Iban`, `TurkishPhone`, `PostalCodeTr`, `PlateTr`, `CardNumber`. Mesajlar `{PropertyName}` kullanır; değiştirmek için `WithMessage` kullanın.

## Notlar

Null ve boş değerler geçerlidir, bu yüzden isteğe bağlı alanlar isteğe bağlı kalır. Değer zorunlu olsun istiyorsanız `[Required]` ekleyin. Varsayılan mesajlar İngilizcedir ve alan adını içerir; yerelleştirmek için `ErrorMessage` ya da kaynak (resource) özelliklerini ayarlayın.
