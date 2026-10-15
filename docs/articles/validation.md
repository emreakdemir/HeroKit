# Form validation

DataAnnotations attributes for the HeroKit validators. They work with ASP.NET Core model validation, Blazor's `DataAnnotationsValidator`, minimal API filters and `Validator.TryValidateObject`.

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

| Attribute | Checks |
|---|---|
| `[Tckn]` | Turkish identity number checksum |
| `[Vkn]` | Turkish tax number checksum |
| `[TaxIdTr]` | VKN or TCKN |
| `[Iban]` | ISO 13616 mod-97; length for TR |
| `[TurkishPhone]` | Turkish phone number; `MobileOnly = true` for mobiles |
| `[PostalCodeTr]` | 5 digits, valid province prefix |
| `[PlateTr]` | Civilian plate format |
| `[CardNumber]` | Length and Luhn |

## FluentValidation

The separate `HeroKit.FluentValidation` package adds the same checks as FluentValidation rules. The core package stays dependency-free.

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

Rules: `Tckn`, `Vkn`, `TaxIdTr`, `Iban`, `TurkishPhone`, `PostalCodeTr`, `PlateTr`, `CardNumber`. Messages use `{PropertyName}`; replace them with `WithMessage`.

## Notes

Null and empty values are valid, so optional fields stay optional. Add `[Required]` to require a value. Default messages are English and include the field name; set `ErrorMessage` or resource properties to localize them.
