# Getting started

## Install

```bash
dotnet add package HeroKit
```

HeroKit targets .NET 8 and .NET 10 and has no dependencies.

## Use

Every helper is an extension method. Import the namespace of the area you need:

| Namespace | What it covers |
|---|---|
| `HeroKit.Strings` | Checks, conversions, slugs, casing, truncating and masking |
| `HeroKit.Globalization` | `Country`, `Language` and `Currency` with `Countries.Turkey`, `Languages.Turkish`, `Currencies.TurkishLira`: phone numbers, plates, postal codes, regions, national and tax ids, holidays and business days, currency formatting, amounts in words, casing and sorting |
| `HeroKit.Globalization.Turkey` | Türkiye-only checks: TCKN, VKN, e-Fatura invoice numbers, ETTN |
| `HeroKit.Banking` | IBAN and card number validation |
| `HeroKit.Validation` | Form validation attributes: `[Tckn]`, `[Iban]`, `[PhoneNumber("TR")]`, ... |
| `HeroKit.Enumerables` | Null-safe checks, conditional filters, paging |
| `HeroKit.DateTimes` | Formatting and date ranges |
| `HeroKit.Decimals` | Price rounding and VAT |
| `HeroKit.Objects`, `HeroKit.Types`, `HeroKit.Enums`, ... | Conversions and reflection helpers |

```csharp
using HeroKit.Strings;

"HeroKit extensions".ToSlug();     // "herokit-extensions"
"42".ToInt();                      // 42
"HeroKit extensions".Truncate(10); // "HeroKit e…"
```

The country and language helpers take a `Country` or `Language` argument, so a new country needs no new methods. See [Countries and languages](countries-and-languages.md).

## Conventions

- **Bad input does not throw.** Checks return `false`; conversions have a lenient `ToX` (default value) and a checked `TryToX` form; string helpers return `null` or an empty string as documented.
- **Parsing is culture-invariant.** Pass a `CultureInfo` when a culture matters, for example `"07.03.2026".ToDateTime(provider: CultureInfo.GetCultureInfo("tr-TR"))`.
- **Deprecated APIs** are marked `[Obsolete]` with the replacement in the message and are removed only in the next major version. Upgrading from 1.x or 2.x? See [Upgrading to 3.0](upgrading.md).
