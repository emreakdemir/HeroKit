# HeroKit

[![NuGet](https://img.shields.io/nuget/v/HeroKit.svg)](https://www.nuget.org/packages/HeroKit)
[![NuGet downloads](https://img.shields.io/nuget/dt/HeroKit.svg)](https://www.nuget.org/packages/HeroKit)
[![Build & Test](https://github.com/emreakdemir/HeroKit/actions/workflows/build-tests.yml/badge.svg)](https://github.com/emreakdemir/HeroKit/actions/workflows/build-tests.yml)
[![Coverage](https://raw.githubusercontent.com/emreakdemir/HeroKit/badges/coverage.svg)](https://github.com/emreakdemir/HeroKit/actions/workflows/build-tests.yml)
[![License: MIT](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

English: [README.md](README.md) · **Dokümantasyon: [herokit.emreakdemir.dev](https://herokit.emreakdemir.dev)**

.NET için günlük işlerde kullanılan uzantı metotları; Türkçe veriler için birinci sınıf destekle: TCKN, VKN, IBAN, telefon, plaka, il, resmî tatiller ve yazıyla tutar.

```bash
dotnet add package HeroKit
```

.NET 8 ve .NET 10'u hedefler. Bağımlılığı yoktur. Null işaretleri tamdır ve Native AOT ile uyumludur.

## Türkçe veri araçları

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
new DateOnly(2026, 3, 19).AddBusinessDaysTr(1); // 2026-03-23 (bayram ve hafta sonu atlanır)
```

## Form doğrulama

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

Ayrıca: `[Vkn]`, `[TaxIdTr]`, `[PostalCodeTr]`, `[CardNumber]`. ASP.NET Core, Blazor ve `Validator` ile çalışır.

FluentValidation mı kullanıyorsunuz? [HeroKit.FluentValidation](https://www.nuget.org/packages/HeroKit.FluentValidation) paketini ekleyin: `RuleFor(x => x.IdentityNumber).Tckn();`

## Dönüşümler

Her dönüşümün varsayılan değer döndüren esnek bir hâli ve bir `Try` hâli vardır. İkisi de hatalı girdide hata fırlatmaz. Bir kültür verilmezse çözümleme kültürden bağımsızdır.

```csharp
using HeroKit.Strings;

"42".ToInt();                       // 42
"abc".ToInt(-1);                    // -1
"2026-03-07".To<DateOnly>();        // her IParsable<T>
"42".TryTo(out int number);         // true
"1.234,56".ToDecimal();             // 1234.56
"Friday".ToEnum<DayOfWeek>();       // DayOfWeek.Friday
```

## Diğerleri

| Alan | Örnekler |
|---|---|
| Metin | `ToSlug`, `Truncate`, `Mask`, `Left`/`Right`, `RemoveWhitespace`, `ToTitleCase(culture)` |
| Koleksiyon | `IsNullOrEmpty`, `WhereIf` ve `Page` (`IQueryable` dahil), `ToReadOnlyList` |
| Tarih | `StartOfWeek`, `EndOfMonth`, `IsWeekend`, `IsBetween`, `Age` (DateTime ve DateOnly) |
| Fiyat | `RoundToStep`, `ToPsychologicalPrice`, `IncludeVat`, `ExcludeVat` |
| Kart | `IsValidCardNumber` (Luhn), `GetCardBrand` (Troy dahil) |
| Enum | `GetDescription`, `ToEnum`, `ToUnderlying<T>` |

Bütün metotlar [rehberlerde ve API referansında](https://herokit.emreakdemir.dev). 1.x'ten mi geçiyorsunuz? [Geçiş rehberine](https://herokit.emreakdemir.dev/articles/migrating-to-2.html) bakın.

## Sorular ve katkı

- Sorular ve fikirler: [GitHub Discussions](https://github.com/emreakdemir/HeroKit/discussions)
- Hata ve özellik talepleri: [Issues](https://github.com/emreakdemir/HeroKit/issues)
- Katkı rehberi: [CONTRIBUTING.md](CONTRIBUTING.md)

## Lisans

MIT
