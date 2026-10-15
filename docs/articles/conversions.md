# Conversions

Every conversion comes in two forms. Neither throws for null, empty or invalid input.

| Form | Example | On bad input |
|---|---|---|
| Lenient `ToX` | `"abc".ToInt(-1)` | returns the default value |
| Checked `TryToX` | `"abc".TryTo(out int n)` | returns `false` |

Parsing uses the invariant culture unless you pass a provider.

## Strings to values

```csharp
using System.Globalization;
using HeroKit.Strings;

// Any IParsable<T>: int, long, decimal, double, DateTime, DateOnly, TimeOnly, Guid, ...
"42".To<int>();                         // 42
"x".To(defaultValue: -1);               // -1
"2026-03-07".To<DateOnly>();            // 2026-03-07
"1,5".To<decimal>(provider: CultureInfo.GetCultureInfo("tr-TR")); // 1.5
"42".TryTo(out int number);             // true, number == 42
"abc".ToNullable<int>();                // null

// Shortcuts
"42".ToInt();                           // 42 (also ToLong, ToShort)
"2026-03-07".ToDateTime();              // 7 March 2026
"07.03.2026".ToDateTime(provider: CultureInfo.GetCultureInfo("tr-TR"));
"yes".ToBoolean(trueText: "yes");       // true
"SGVyb0tpdA==".FromBase64();            // bytes, or null if invalid
```

## Decimals

Without a provider `ToDecimal` is flexible: a comma is the decimal separator when present, otherwise a dot is.

```csharp
"1.234,56".ToDecimal();                 // 1234.56
"1234.56".ToDecimal();                  // 1234.56
"1,234.56".ToDecimal(provider: CultureInfo.InvariantCulture); // strict: 1234.56
"abc".TryToDecimal(out decimal amount); // false
```

## Enums

```csharp
"friday".ToEnum<DayOfWeek>();           // DayOfWeek.Friday (name, ignoring case)
"5".ToEnum<DayOfWeek>();                // DayOfWeek.Friday (defined number)
"99".ToEnum(DayOfWeek.Monday);          // DayOfWeek.Monday (not defined: default)
"Sunday".TryToEnum(out DayOfWeek day);  // true
```

## Objects

```csharp
using HeroKit.Objects;

object cell = "42";
cell.ConvertTo<int>();                  // 42
((object)"abc").ConvertTo(-1);          // -1
row["Count"].TryConvertTo(out int count);
```

`ConvertTo` handles `DBNull`, nullable targets, enums and GUIDs from strings or 16-byte arrays.
