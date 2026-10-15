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
| `HeroKit.Turkish` | TCKN/VKN, phone numbers, amounts in words, Turkish casing |
| `HeroKit.Banking` | IBAN validation and formatting |
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

## Conventions

- **Null input does not throw.** Checks return `false`, conversions return the default value, string helpers return `null` or an empty string as documented.
- **Parsing is culture-invariant.** Pass a `CultureInfo` when a culture matters, for example `"07.03.2026".ToDateTime(CultureInfo.GetCultureInfo("tr-TR"))`.
- **Deprecated APIs** are marked `[Obsolete]` with the replacement in the message and are removed only in the next major version.
