# Dönüşümler

Her dönüşümün iki biçimi vardır. İkisi de null, boş ya da geçersiz girdide istisna fırlatmaz.

| Biçim | Örnek | Hatalı girdide |
|---|---|---|
| Toleranslı `ToX` | `"abc".ToInt(-1)` | varsayılan değeri döner |
| Denetimli `TryToX` | `"abc".TryTo(out int n)` | `false` döner |

Bir provider vermezseniz ayrıştırma değişmez kültürü (invariant culture) kullanır.

## Metinden değere

```csharp
using System.Globalization;
using HeroKit.Strings;

// Herhangi bir IParsable<T>: int, long, decimal, double, DateTime, DateOnly, TimeOnly, Guid, ...
"42".To<int>();                         // 42
"x".To(defaultValue: -1);               // -1
"2026-03-07".To<DateOnly>();            // 2026-03-07
"1,5".To<decimal>(provider: CultureInfo.GetCultureInfo("tr-TR")); // 1.5
"42".TryTo(out int number);             // true, number == 42
"abc".ToNullable<int>();                // null

// Kısayollar
"42".ToInt();                           // 42 (ToLong ve ToShort da var)
"2026-03-07".ToDateTime();              // 7 Mart 2026
"07.03.2026".ToDateTime(provider: CultureInfo.GetCultureInfo("tr-TR"));
"yes".ToBoolean(trueText: "yes");       // true
"SGVyb0tpdA==".FromBase64();            // bayt dizisi, geçersizse null
```

## Decimal değerler

Provider olmadan `ToDecimal` esnektir: virgül varsa ondalık ayırıcı virgüldür, yoksa noktadır.

```csharp
"1.234,56".ToDecimal();                 // 1234.56
"1234.56".ToDecimal();                  // 1234.56
"1,234.56".ToDecimal(provider: CultureInfo.InvariantCulture); // katı: 1234.56
"abc".TryToDecimal(out decimal amount); // false
```

## Enum değerleri

```csharp
"friday".ToEnum<DayOfWeek>();           // DayOfWeek.Friday (ad, büyük-küçük harf yok sayılır)
"5".ToEnum<DayOfWeek>();                // DayOfWeek.Friday (tanımlı sayı)
"99".ToEnum(DayOfWeek.Monday);          // DayOfWeek.Monday (tanımlı değil: varsayılan)
"Sunday".TryToEnum(out DayOfWeek day);  // true
```

## Nesneler

```csharp
using HeroKit.Objects;

object cell = "42";
cell.ConvertTo<int>();                  // 42
((object)"abc").ConvertTo(-1);          // -1
row["Count"].TryConvertTo(out int count);
```

`ConvertTo`; `DBNull`, null olabilen hedefleri, enum değerlerini ve metinden ya da 16 baytlık dizilerden GUID'leri işler.
