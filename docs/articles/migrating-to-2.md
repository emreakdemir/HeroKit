# Migrating to 2.0

HeroKit 2.0 applies one error policy to every conversion, makes names consistent and removes helpers that were obsolete or duplicated .NET. Most changes are renames that the compiler points out.

## Conversions

Lenient `ToX` returns a default value; `TryToX` returns `false`. Neither throws for bad input.

| 1.x | 2.0 |
|---|---|
| `"x".ToDecimal(",", true)` (threw on invalid input) | `"x".ToDecimal()` returns 0; `ToDecimal(defaultValue, provider)`; `TryToDecimal(out …)` |
| `"x".ToDateTime(culture, defaultValue)` | `"x".ToDateTime(defaultValue, provider)` |
| `"x".ToBoolean(trueCondition: "yes", defaultvalue: false)` | `"x".ToBoolean(trueText: "yes", defaultValue: false)` |
| `"x".ToEnum<T>()` for any `T` | `T : struct, Enum`; new `TryToEnum<T>(out …)` |
| `"x".ToNullable<T>()` → `(bool status, T? result)` | `"x".ToNullable<T>()` → `T?` (null on failure) |
| `"x".ToByteArray()` (threw on invalid base64) | `"x".FromBase64()` → `byte[]?` |
| `obj.ConvertTo<T>()` (threw on invalid input) | returns the default value; `TryConvertTo<T>(out …)` |
| `obj.TryCast<T>(out …)` | `obj.TryConvertTo<T>(out …)` |
| `property.TrySetConvertedValue<T>(instance, value)` | `property.TrySetConvertedValue(instance, value)` |
| — | `"x".To<T>()` / `TryTo<T>(out …)` for any `IParsable<T>` |

`TryConvertTo` and `TrySetConvertedValue` now return `false` for null input instead of converting it to the default value. `TrySetConvertedValue(instance, null)` clears reference-type and nullable properties.

## Renames

| 1.x | 2.0 |
|---|---|
| `IsEvenNumber()` / `IsOddNumber()` | `IsEven()` / `IsOdd()` |
| `ToMonthString(cultureInfo)` | `ToMonthName(culture)` |
| `flag.ToString("Yes", "No")` | `flag.ToText("Yes", "No")` |
| `status.GetValue<int>()` → `"2"` | `status.ToUnderlying<int>()` → `2` |
| `array.InsertItems(items)` | `array.CopyFrom(items)` (returns the count) |
| `bytes.UrlTokenEncode()` | `bytes.ToBase64Url()` (RFC 4648 URL-safe base64) |
| `date.ToCultureString(formatProvider: …)` | `date.ToCultureString(culture: …)` |

Single-value parameters are named `value` everywhere (was `input`, `datetime`, `target`, `number`, `source`). Update named arguments if you used them.

## Removed

| Removed | Use instead |
|---|---|
| `ToUpperTr()`, `ToLowerTr()` | `ToUpper(CultureInfo.GetCultureInfo("tr-TR"))`, `ToLower(…)` |
| `ToTrString()` | `ToCultureString(CultureInfo.GetCultureInfo("tr-TR"))` |
| `PropertyInfoExtensions.SetValue<T>()` | `TrySetConvertedValue(instance, value)` |
| `ToPixel()` | `point * 4m / 3m` |
| `Seconds()` | `TimeSpan.FromSeconds(n)` |
| `NameEqualityComparer` | `StringComparer.OrdinalIgnoreCase` |
| `array.ToList(mapFunction)` | `array.Select(map).ToList()` |
| `TypesExtensions.GetDefault<T>()` | `default(T)` |

## New in 2.0

Form validation attributes (`[Tckn]`, `[Iban]`, `[Vkn]`, …), provinces, plates and postal codes, card validation with Troy, Turkish lira formatting and enum descriptions. See the guides in the menu. The Turkish helpers were reorganized again in 3.0: see [Migrating to 3.0](migrating-to-3.md).
