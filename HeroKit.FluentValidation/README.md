# HeroKit.FluentValidation

FluentValidation rules for [HeroKit](https://www.nuget.org/packages/HeroKit): Turkish identity and tax numbers, IBAN, Turkish phone numbers, postal codes, vehicle plates and card numbers.

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
        RuleFor(x => x.TaxId).TaxIdTr();
        RuleFor(x => x.Iban).Iban();
        RuleFor(x => x.Mobile).TurkishPhone(mobileOnly: true);
        RuleFor(x => x.PostalCode).PostalCodeTr();
        RuleFor(x => x.Plate).PlateTr();
        RuleFor(x => x.Card).CardNumber();
    }
}
```

Null and empty values pass; add `NotEmpty()` to require a value. Messages use `{PropertyName}` and can be replaced with `WithMessage`.

Documentation: https://herokit.emreakdemir.dev/articles/validation.html
