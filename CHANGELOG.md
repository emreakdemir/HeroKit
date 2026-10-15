# Changelog

All notable changes to this project are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- `ToTitleCase(culture)` and `EqualsIgnoreCase(other, culture)` for any culture. `ToTitleCaseTr` and `EqualsTr` now call them with tr-TR (#67).

### Deprecated
- `ToUpperTr`, `ToLowerTr` and `ToTrString`: use `ToUpper(culture)`, `ToLower(culture)` and `ToCultureString(culture)`. Removal planned for 2.0.0 (#67, #42).

## [1.2.0] - 2026-10-15

### Added
- Turkish identity and tax number validation: `IsValidTckn`, `IsValidVkn`, `IsValidTaxIdTr` (#30).
- IBAN validation and formatting: `IsValidIban`, `FormatIban`, `NormalizeIban` (#31).
- Turkish phone numbers: `ToE164Tr`, `FormatPhoneTr`, `IsValidPhoneTr`, `IsValidMobileTr` (#32).
- Numbers and amounts in Turkish words: `ToTurkishWords`, `ToTurkishCurrencyWords` (#33).
- Turkish-aware text: `ToTitleCaseTr`, `EqualsTr` (#34).
- Coverage badge published by CI without an external service; questions go to GitHub Discussions (#25).

## [1.1.0] - 2026-10-15

### Added
- Nullable reference type annotations on the whole public API. Null-check helpers such as `IsNullOrEmpty`, `IsNotNull` and array `IsNotNullOrEmpty` carry `[NotNullWhen]` (#20).
- Trimming and Native AOT compatibility (`IsAotCompatible`). Reflection-based APIs carry `[RequiresUnreferencedCode]` / `[RequiresDynamicCode]` (#24).
- Public API tracking with `Microsoft.CodeAnalysis.PublicApiAnalyzers` (#21).
- Benchmark project (`HeroKit.Benchmarks`) and Native AOT smoke test in CI (#26, #24).
- Code coverage summary in CI (#25).
- Dependabot, CHANGELOG, CONTRIBUTING, issue forms and pull request template (#27, #28).

### Changed
- `ToSlug` and `ToDashCase` use source-generated regex. `ClearTurkishChars` and `RemoveAccent` run in a single pass. `ClearTurkishChars` is up to 3x faster (#26).
- `EnumExtensions.ToList<TEnum>()` no longer uses `Enum.GetValues(Type)` (#24).
- `ConvertTo<T>` and `TryCast<T>` return `T?`: a null source converts to the default value (#20).

## [1.0.2] - 2026-10-15

### Fixed
- Conversions no longer depend on the machine culture. `ToDateTime` defaults to the invariant culture. `ToInt`, `ToLong`, `ToShort` and `IsNumeric` parse with invariant formats (#45, closes #22).
- `GetExtensionMethod(type, assembly, name, types)` matches overloads by parameter type correctly (#44).
- `IsNullOrEmpty(string, char)` treats a string made only of separators as empty (#44).
- `ToBoolean` returns the default value for null or empty input instead of throwing (#44).
- `NameEqualityComparer.Default` returns a cached instance (#44).

### Added
- `PropertyInfoExtensions.TrySetConvertedValue<T>` (#46, closes #23).

### Deprecated
- `PropertyInfoExtensions.SetValue<T>`: the built-in `PropertyInfo.SetValue(object, object)` hides it in instance syntax. Use `TrySetConvertedValue`. Removal planned for 2.0.0.

### Changed
- `"07.03.2026".ToDateTime()` no longer uses the machine culture. Pass a culture explicitly for non-ISO formats.
- `IsNullOrEmpty("  ", ' ')` now returns true.

## [1.0.1] - 2026-10-15

### Fixed
- `ToEnum` parses enum names (case-insensitive) as well as defined numeric values (#17).
- `ConvertTo<TEnum>` respects the supplied default value (#17).
- `IsOddNumber` returns true for negative odd numbers (#17).
- `ToPixel` uses the 96 DPI ratio (1pt = 4/3px) (#17).
- `ToDecimal` keeps thousands separators such as `1.234,56` (#17).
- `EnumExtensions.ToList<TEnum>()` no longer throws for enums (#18).

### Changed
- `ToEnum` and `ToPixel` return different results than 1.0.0 for affected inputs.

### Added
- Tests for Enums, Guids, Objects, DateTimes, Integers, Properties and Assemblies (#18).

## [1.0.0] - 2026-10-15

First stable release. Targets .NET 8 and .NET 10.

[Unreleased]: https://github.com/emreakdemir/HeroKit/compare/1.2.0...HEAD
[1.2.0]: https://github.com/emreakdemir/HeroKit/compare/1.1.0...1.2.0
[1.1.0]: https://github.com/emreakdemir/HeroKit/compare/1.0.2...1.1.0
[1.0.2]: https://github.com/emreakdemir/HeroKit/compare/1.0.1...1.0.2
[1.0.1]: https://github.com/emreakdemir/HeroKit/compare/1.0.0...1.0.1
[1.0.0]: https://github.com/emreakdemir/HeroKit/releases/tag/1.0.0
