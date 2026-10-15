# Changelog

All notable changes to this project are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

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

[Unreleased]: https://github.com/emreakdemir/HeroKit/compare/1.0.2...HEAD
[1.0.2]: https://github.com/emreakdemir/HeroKit/compare/1.0.1...1.0.2
[1.0.1]: https://github.com/emreakdemir/HeroKit/compare/1.0.0...1.0.1
[1.0.0]: https://github.com/emreakdemir/HeroKit/releases/tag/1.0.0
