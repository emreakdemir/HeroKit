# Form validation

DataAnnotations attributes for the HeroKit validators. They work with ASP.NET Core model validation, Blazor's `DataAnnotationsValidator`, minimal API filters and `Validator.TryValidateObject`.

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

| Attribute | Checks |
|---|---|
| `[Tckn]` | Turkish identity number checksum |
| `[Vkn]` | Turkish tax number checksum |
| `[Iban]` | ISO 13616 mod-97; national length where a country has IBAN rules (TR: 26) |
| `[CardNumber]` | Length and Luhn |
| `[GibInvoiceNumber]` | e-Fatura / e-Arşiv invoice number in the GİB format, for example `ABC2026000000001` |
| `[Ettn]` | ETTN: a non-empty UUID in the 8-4-4-4-12 format |
| `[PhoneNumber("TR")]` | Phone number of a country; `MobileOnly = true` for mobiles |
| `[VehiclePlate("TR")]` | Vehicle plate of a country |
| `[PostalCode("TR")]` | Postal code of a country (TR: 5 digits, valid province prefix) |
| `[NationalId("TR")]` | National identity number of a country (TR: TCKN) |
| `[TaxId("TR")]` | Tax identifier of a country (TR: VKN or TCKN) |

The country attributes take an ISO 3166-1 alpha-2 code. The country is looked up in `Countries` when a value is validated, so a country you add with `Countries.Register` works too. See [Countries and languages](countries-and-languages.md). A country without the needed rules throws `NotSupportedException`.

## FluentValidation

The separate `HeroKit.FluentValidation` package adds the same checks as FluentValidation rules. The core package stays dependency-free.

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

Rules: `Tckn`, `Vkn`, `Iban`, `CardNumber`, `GibInvoiceNumber`, `Ettn`, and the country rules `PhoneNumber(country, mobileOnly)`, `VehiclePlate(country)`, `PostalCode(country)`, `NationalId(country)`, `TaxId(country)`. Messages use `{PropertyName}`; replace them with `WithMessage`.

## Notes

Null and empty values are valid, so optional fields stay optional. Add `[Required]` to require a value. Default messages are English and include the field name; set `ErrorMessage` or resource properties to localize them.
