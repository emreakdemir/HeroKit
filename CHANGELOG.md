# Changelog

All notable changes to this project are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [3.1.0] - 2026-10-15

### Added
- United States: `Countries.UnitedStates` with NANP phone numbers, ZIP and ZIP+4 codes, SSN (`IsValidNationalId`), SSN, ITIN or EIN (`IsValidTaxId`), the 11 federal holidays with observed days, and the 50 states plus DC (FIPS codes and USPS abbreviations). Vehicle plates differ by state and are not supported.
- `Languages.English`: numbers in American English words and ICU-free casing and sorting.
- `Currencies.USDollar`: "$1,234.50", "one hundred twenty-three dollars and fifty cents".
- `Currency.UnitNamePlural`, `SubunitNamePlural` and `SubunitConjunction` for languages that inflect unit names. Turkish output is unchanged.
- `Region.Abbreviation`; `Country.FindRegion` also matches it ("CA").
- Attributes and FluentValidation rules resolve "US" without extra setup.

### Quality
- Tests for `IsValidNationalId` (TCKN) and for every United States rule, also under invariant globalization; Native AOT smoke test with 20 checks.

## [3.0.0] - 2026-10-15

Breaking release. Country- and language-specific behavior is selected with objects instead of method names, so new countries and languages need no new methods. See the [upgrade guide](https://herokit.emreakdemir.dev/articles/upgrading.html).

### Changed (breaking)
- New model in `HeroKit.Globalization`: `Country` / `Countries`, `Language` / `Languages`, `Currency` / `Currencies` with rule interfaces `IPhoneNumberRules`, `IVehiclePlateRules`, `IIdentifierRules`, `IHolidayCalendar`, `INumberSpeller`, `ITextRules` (#104).
- Türkiye is `Countries.Turkey`, `Languages.Turkish` and `Currencies.TurkishLira`. The `HeroKit.Turkish` namespace is removed.
- Methods lose their `Tr` / `Turkish` names and take the country or language:
  - `IsValidPhoneTr`, `IsValidMobileTr`, `ToE164Tr`, `FormatPhoneTr` → `IsValidPhone`, `IsValidMobilePhone`, `ToE164`, `FormatPhone` (`Countries.Turkey`).
  - `IsValidPlateTr`, `FormatPlateTr`, `IsValidPostalCodeTr`, `IsValidTaxIdTr` → `IsValidVehiclePlate`, `FormatVehiclePlate`, `IsValidPostalCode`, `IsValidTaxId`; new `IsValidNationalId`.
  - `ToProvinceName`, `ToProvinceCode`, `TurkishProvinces` → `Countries.Turkey.FindRegion(...)`, `Countries.Turkey.Regions`.
  - `TurkishHolidays` and the `…Tr` business day methods → `Countries.Turkey.Holidays`, `IsPublicHoliday`, `IsHalfDayHoliday`, `GetPublicHoliday`, `IsBusinessDay`, `AddBusinessDays`, `BusinessDaysUntil` (`Countries.Turkey`).
  - `ToTurkishWords`, `ToTurkishCurrencyWords`, `ToTurkishLira` → `ToWords(Languages.Turkish)`, `ToCurrencyWords(Currencies.TurkishLira)`, `ToCurrencyString(Currencies.TurkishLira, CurrencyStyle)`.
  - `ToTitleCaseTr`, `EqualsTr` → `ToTitleCase(Languages.Turkish)`, `EqualsIgnoreCase(other, Languages.Turkish)`; new `ToUpper` / `ToLower(Languages.Turkish)`.
- Attributes take an ISO country code: `[PhoneNumber("TR", MobileOnly = …)]`, `[VehiclePlate("TR")]`, `[PostalCode("TR")]`, `[TaxId("TR")]`, new `[NationalId("TR")]`. FluentValidation rules take a `Country`: `PhoneNumber(Countries.Turkey, mobileOnly)`, `VehiclePlate`, `PostalCode`, `TaxId`, `NationalId`.
- `RemoveAccent` and `ClearTurkishChars` → `RemoveDiacritics`.
- IBAN national rules come from the registered country (`Country.IbanRules`) instead of a hard-coded check.

### Added
- Countries can be added without changing HeroKit: implement the rule interfaces and call `Countries.Register`. Attributes resolve countries by ISO code.
- e-Fatura / e-Arşiv: `GibInvoiceNumber` (parse, create, next number, format), `IsValidGibInvoiceNumber`, `IsValidEttn`, `[GibInvoiceNumber]`, `[Ettn]`, matching FluentValidation rules (#96).
- Turkish alphabetical sorting without ICU: `Languages.Turkish.TextRules.Comparer` and `IgnoreCaseComparer` (#97).
- `CurrencyStyle.IsoCode` ("1.234,50 TRY").
- Turkish documentation site (#95).

### Fixed
- Text rules, sorting and `RemoveDiacritics` no longer depend on ICU, so they work with invariant globalization (containers).
- `ToSlug` no longer returns leading or trailing dashes: `"  ---  "` becomes `""` and `"- Hello -"` becomes `"hello"`.

### Quality
- Test coverage 98% lines and 96% branches (#98); Native AOT smoke test with 19 checks in CI.

## 2.x (2.0.0 – 2.2.0) - 2026-10-15

Superseded by 3.0.0. Highlights:
- One error policy for conversions: lenient `ToX(defaultValue)` and checked `TryToX(out …)`; generic `To<T>` / `TryTo<T>` for any `IParsable<T>`.
- Consistent names (`IsEven`, `ToMonthName`, `ToText`, `CopyFrom`, `ToBase64Url`, …) and removal of obsolete helpers.
- Validation attributes and the `HeroKit.FluentValidation` package.
- Provinces, plates, postal codes, card numbers with Troy, lira formatting, enum descriptions, public holidays and business days (Diyanet calendar 2022–2027).

## 1.x (1.0.0 – 1.3.0) - 2026-10-15

Superseded by 3.0.0. Highlights:
- First stable release for .NET 8 and .NET 10; culture-invariant parsing and many bug fixes.
- Nullable annotations, trimming and Native AOT support, public API tracking, benchmarks.
- TCKN/VKN, IBAN, Turkish phone numbers, amounts in words, Turkish casing.
- Enumerable, date, price/VAT and string helpers; documentation site.

## Pre-releases (2023 – 2024)

1.0.0-alpha to 1.0.0-alpha.6: the first array, boolean, string, type and conversion helpers (contributors: @emreakdemir, @jepozdemir).

[Unreleased]: https://github.com/emreakdemir/HeroKit/compare/3.1.0...HEAD
[3.1.0]: https://github.com/emreakdemir/HeroKit/compare/3.0.0...3.1.0
[3.0.0]: https://github.com/emreakdemir/HeroKit/compare/2.2.0...3.0.0
