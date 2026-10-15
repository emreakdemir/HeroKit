# Migrating to 3.0

HeroKit 3.0 replaces the Turkish-only methods (`...Tr`, `ToTurkish...`) with one set of methods that take a `Country`, `Language` or `Currency`. Türkiye is built in as `Countries.Turkey`, `Languages.Turkish` and `Currencies.TurkishLira`. Other countries use the same methods once you add them: see [Countries and languages](countries-and-languages.md).

Most changes are renames that the compiler points out. The behavior and the results are the same.

## Namespaces

| 2.x | 3.0 |
|---|---|
| `using HeroKit.Turkish;` | `using HeroKit.Globalization;` |
| `IsValidTckn`, `IsValidVkn` | `using HeroKit.Globalization.Turkey;` |

## Phone numbers

| 2.x | 3.0 |
|---|---|
| `value.IsValidPhoneTr()` | `value.IsValidPhone(Countries.Turkey)` |
| `value.IsValidMobileTr()` | `value.IsValidMobilePhone(Countries.Turkey)` |
| `value.ToE164Tr()` | `value.ToE164(Countries.Turkey)` |
| `value.FormatPhoneTr()` | `value.FormatPhone(Countries.Turkey)` |

## Identity, tax, plates, postal codes and regions

| 2.x | 3.0 |
|---|---|
| `value.IsValidTckn()`, `value.IsValidVkn()` | unchanged (namespace `HeroKit.Globalization.Turkey`) |
| — | `value.IsValidNationalId(Countries.Turkey)` (TCKN) |
| `value.IsValidTaxIdTr()` | `value.IsValidTaxId(Countries.Turkey)` |
| `value.IsValidPlateTr()` | `value.IsValidVehiclePlate(Countries.Turkey)` |
| `value.FormatPlateTr()` | `value.FormatVehiclePlate(Countries.Turkey)` |
| `value.IsValidPostalCodeTr()` | `value.IsValidPostalCode(Countries.Turkey)` |
| `"34".ToProvinceName()` | `Countries.Turkey.FindRegion("34")?.Name` |
| `6.ToProvinceName()` | `Countries.Turkey.FindRegion(6)?.Name` |
| `"sanliurfa".ToProvinceCode()` | `Countries.Turkey.FindRegion("sanliurfa")?.Code` |
| `TurkishProvinces.All` | `Countries.Turkey.Regions` (a list of `Region(Code, Name)`) |

`FindRegion` returns `null` for an unknown region.

## Amounts, currency and words

| 2.x | 3.0 |
|---|---|
| `123.ToTurkishWords()` | `123.ToWords(Languages.Turkish)` |
| `amount.ToTurkishCurrencyWords()` | `amount.ToCurrencyWords(Currencies.TurkishLira)` |
| `amount.ToTurkishCurrencyWords("Euro", "sent")` | `amount.ToCurrencyWords(new Currency("EUR", "€", Languages.Turkish) { UnitName = "Euro", SubunitName = "sent" })` |
| `amount.ToTurkishLira()` | `amount.ToCurrencyString(Currencies.TurkishLira)` |
| `amount.ToTurkishLira(TurkishLiraFormat.Code)` | `amount.ToCurrencyString(Currencies.TurkishLira, CurrencyStyle.Abbreviation)` (`"1.234,50 TL"`) |
| — | `CurrencyStyle.IsoCode` (`"1.234,50 TRY"`) |

## Public holidays and business days

| 2.x | 3.0 |
|---|---|
| `TurkishHolidays.GetHolidays(2026)` | `Countries.Turkey.Holidays!.GetHolidays(2026)` |
| `date.IsPublicHolidayTr()` | `date.IsPublicHoliday(Countries.Turkey)` |
| `date.IsHalfDayHolidayTr()` | `date.IsHalfDayHoliday(Countries.Turkey)` |
| `date.GetPublicHolidayTr()` | `date.GetPublicHoliday(Countries.Turkey)` |
| `date.IsBusinessDayTr()` | `date.IsBusinessDay(Countries.Turkey)` |
| `date.AddBusinessDaysTr(n)` | `date.AddBusinessDays(n, Countries.Turkey)` |
| `date.BusinessDaysUntilTr(end)` | `date.BusinessDaysUntil(end, Countries.Turkey)` |

## Casing and text

| 2.x | 3.0 |
|---|---|
| `value.ToTitleCaseTr()` | `value.ToTitleCase(Languages.Turkish)` |
| `value.EqualsTr(other)` | `value.EqualsIgnoreCase(other, Languages.Turkish)` |
| `value.RemoveAccent()` | `value.RemoveDiacritics()` |
| `value.ClearTurkishChars()` | `value.RemoveDiacritics()` |

`RemoveDiacritics` uses a fixed character table, so it gives the same result on every system. It also covers Latin-1 and common Central European letters.

## Validation attributes

| 2.x | 3.0 |
|---|---|
| `[TurkishPhone]`, `[TurkishPhone(MobileOnly = true)]` | `[PhoneNumber("TR")]`, `[PhoneNumber("TR", MobileOnly = true)]` |
| `[PlateTr]` | `[VehiclePlate("TR")]` |
| `[PostalCodeTr]` | `[PostalCode("TR")]` |
| `[TaxIdTr]` | `[TaxId("TR")]` |
| — | `[NationalId("TR")]` |
| `[Tckn]`, `[Vkn]`, `[Iban]`, `[CardNumber]` | unchanged |

## FluentValidation

| 2.x | 3.0 |
|---|---|
| `TurkishPhone(mobileOnly)` | `PhoneNumber(Countries.Turkey, mobileOnly)` |
| `PlateTr()` | `VehiclePlate(Countries.Turkey)` |
| `PostalCodeTr()` | `PostalCode(Countries.Turkey)` |
| `TaxIdTr()` | `TaxId(Countries.Turkey)` |
| — | `NationalId(Countries.Turkey)` |
| `Tckn()`, `Vkn()`, `Iban()`, `CardNumber()` | unchanged |

## New in 3.0

These did not exist in a released version:

- **e-Fatura and e-Arşiv:** `GibInvoiceNumber` (`TryParse`, `Create`, `Next`), `IsValidGibInvoiceNumber`, `IsValidEttn`, the `[GibInvoiceNumber]` and `[Ettn]` attributes and the `GibInvoiceNumber()` and `Ettn()` FluentValidation rules.
- **Turkish sorting without ICU:** `Languages.Turkish.TextRules!.Comparer` and `IgnoreCaseComparer`, used with `Array.Sort`, `OrderBy(key, comparer)` or as a dictionary comparer. They work with invariant globalization.
- **Casing:** `ToUpper(Languages.Turkish)` and `ToLower(Languages.Turkish)`.
- **Your own country, language or currency:** `new Country(...)`, `Countries.Register`, `new Language(...)`, `new Currency(...)`.

## Update checklist

1. Replace `using HeroKit.Turkish;` with `using HeroKit.Globalization;` and add `using HeroKit.Globalization.Turkey;` where you call `IsValidTckn` or `IsValidVkn`.
2. Rename the calls with the tables above. The compiler lists every old name.
3. Replace the country attributes and FluentValidation rules.
