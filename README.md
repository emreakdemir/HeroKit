# HeroKit

[![NuGet](https://img.shields.io/nuget/v/HeroKit.svg)](https://www.nuget.org/packages/HeroKit)
[![NuGet downloads](https://img.shields.io/nuget/dt/HeroKit.svg)](https://www.nuget.org/packages/HeroKit)
[![Build & Test](https://github.com/emreakdemir/HeroKit/actions/workflows/build-tests.yml/badge.svg)](https://github.com/emreakdemir/HeroKit/actions/workflows/build-tests.yml)
[![Coverage](https://raw.githubusercontent.com/emreakdemir/HeroKit/badges/coverage.svg)](https://github.com/emreakdemir/HeroKit/actions/workflows/build-tests.yml)
[![License: MIT](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

Türkçe: [README.TR.md](README.TR.md) · **Documentation: [herokit.emreakdemir.dev](https://herokit.emreakdemir.dev)**

Everyday extension methods for .NET, with first-class support for Turkish data: TCKN, VKN, IBAN, phone numbers, plates, provinces and amounts in words.

```bash
dotnet add package HeroKit
```

Targets .NET 8 and .NET 10. No dependencies. Nullable-annotated and Native AOT ready.

## Turkish toolkit

```csharp
using HeroKit.Banking;
using HeroKit.Turkish;

"10000000146".IsValidTckn();                  // true  (T.C. Kimlik No)
"4840847211".IsValidVkn();                    // true  (Vergi Kimlik No)
"TR330006100519786457841326".FormatIban();   // "TR33 0006 1005 1978 6457 8413 26"
"0532 123 45 67".ToE164Tr();                  // "+905321234567"
"34abc123".FormatPlateTr();                   // "34 ABC 123"
"34".ToProvinceName();                        // "İstanbul"
"34710".IsValidPostalCodeTr();                // true
123.50m.ToTurkishCurrencyWords();             // "yüz yirmi üç Türk lirası elli kuruş"
1234.5m.ToTurkishLira();                      // "₺1.234,50"
"istanbul ılıca".ToTitleCaseTr();             // "İstanbul Ilıca"
```

## Form validation

```csharp
using HeroKit.Validation;

public sealed class Customer
{
    [Required, Tckn] public string? IdentityNumber { get; set; }
    [Iban] public string? Iban { get; set; }
    [TurkishPhone(MobileOnly = true)] public string? Mobile { get; set; }
    [PlateTr] public string? Plate { get; set; }
}
```

Also: `[Vkn]`, `[TaxIdTr]`, `[PostalCodeTr]`, `[CardNumber]`. They work with ASP.NET Core, Blazor and `Validator`.

Using FluentValidation? Add [HeroKit.FluentValidation](https://www.nuget.org/packages/HeroKit.FluentValidation): `RuleFor(x => x.IdentityNumber).Tckn();`

## Conversions

Every conversion has a lenient form that returns a default value and a `Try` form. Neither throws for bad input. Parsing is culture-invariant unless you pass a provider.

```csharp
using HeroKit.Strings;

"42".ToInt();                       // 42
"abc".ToInt(-1);                    // -1
"2026-03-07".To<DateOnly>();        // any IParsable<T>
"42".TryTo(out int number);         // true
"1.234,56".ToDecimal();             // 1234.56
"Friday".ToEnum<DayOfWeek>();       // DayOfWeek.Friday
```

## More

| Area | Examples |
|---|---|
| Strings | `ToSlug`, `Truncate`, `Mask`, `Left`/`Right`, `RemoveWhitespace`, `ToTitleCase(culture)` |
| Collections | `IsNullOrEmpty`, `WhereIf` and `Page` (also on `IQueryable`), `ToReadOnlyList` |
| Dates | `StartOfWeek`, `EndOfMonth`, `IsWeekend`, `IsBetween`, `Age` (DateTime and DateOnly) |
| Prices | `RoundToStep`, `ToPsychologicalPrice`, `IncludeVat`, `ExcludeVat` |
| Cards | `IsValidCardNumber` (Luhn), `GetCardBrand` (incl. Troy) |
| Enums | `GetDescription`, `ToEnum`, `ToUnderlying<T>` |

The [guides and API reference](https://herokit.emreakdemir.dev) cover every method. Upgrading from 1.x? See the [migration guide](https://herokit.emreakdemir.dev/articles/migrating-to-2.html).

## Questions and contributions

- Questions and ideas: [GitHub Discussions](https://github.com/emreakdemir/HeroKit/discussions)
- Bugs and feature requests: [Issues](https://github.com/emreakdemir/HeroKit/issues)
- How to contribute: [CONTRIBUTING.md](CONTRIBUTING.md)

## License

MIT
