# 2.0'a geçiş

HeroKit 2.0 her dönüşüme tek bir hata politikası uygular, adları tutarlı hâle getirir ve eski ya da .NET'in kendisiyle çakışan yardımcıları kaldırır. Değişikliklerin çoğu, derleyicinin gösterdiği yeniden adlandırmalardır.

## Dönüşümler

Toleranslı `ToX` varsayılan değer döner; `TryToX` `false` döner. İkisi de hatalı girdide istisna fırlatmaz.

| 1.x | 2.0 |
|---|---|
| `"x".ToDecimal(",", true)` (geçersiz girdide istisna fırlatırdı) | `"x".ToDecimal()` 0 döner; `ToDecimal(defaultValue, provider)`; `TryToDecimal(out …)` |
| `"x".ToDateTime(culture, defaultValue)` | `"x".ToDateTime(defaultValue, provider)` |
| `"x".ToBoolean(trueCondition: "yes", defaultvalue: false)` | `"x".ToBoolean(trueText: "yes", defaultValue: false)` |
| `"x".ToEnum<T>()` her `T` için | `T : struct, Enum`; yeni `TryToEnum<T>(out …)` |
| `"x".ToNullable<T>()` → `(bool status, T? result)` | `"x".ToNullable<T>()` → `T?` (başarısızlıkta null) |
| `"x".ToByteArray()` (geçersiz base64'te istisna fırlatırdı) | `"x".FromBase64()` → `byte[]?` |
| `obj.ConvertTo<T>()` (geçersiz girdide istisna fırlatırdı) | varsayılan değeri döner; `TryConvertTo<T>(out …)` |
| `obj.TryCast<T>(out …)` | `obj.TryConvertTo<T>(out …)` |
| `property.TrySetConvertedValue<T>(instance, value)` | `property.TrySetConvertedValue(instance, value)` |
| — | Her `IParsable<T>` için `"x".To<T>()` / `TryTo<T>(out …)` |

`TryConvertTo` ve `TrySetConvertedValue` artık null girdide, onu varsayılan değere çevirmek yerine `false` döner. `TrySetConvertedValue(instance, null)` referans tipli ve null olabilen özellikleri temizler.

## Yeniden adlandırmalar

| 1.x | 2.0 |
|---|---|
| `IsEvenNumber()` / `IsOddNumber()` | `IsEven()` / `IsOdd()` |
| `ToMonthString(cultureInfo)` | `ToMonthName(culture)` |
| `flag.ToString("Yes", "No")` | `flag.ToText("Yes", "No")` |
| `status.GetValue<int>()` → `"2"` | `status.ToUnderlying<int>()` → `2` |
| `array.InsertItems(items)` | `array.CopyFrom(items)` (sayıyı döner) |
| `bytes.UrlTokenEncode()` | `bytes.ToBase64Url()` (RFC 4648 URL güvenli base64) |
| `date.ToCultureString(formatProvider: …)` | `date.ToCultureString(culture: …)` |

Tek değerli parametreler her yerde `value` olarak adlandırılır (önceden `input`, `datetime`, `target`, `number`, `source`). Adlandırılmış argüman kullandıysanız güncelleyin.

## Kaldırılanlar

| Kaldırılan | Yerine kullanın |
|---|---|
| `ToUpperTr()`, `ToLowerTr()` | `ToUpper(CultureInfo.GetCultureInfo("tr-TR"))`, `ToLower(…)` |
| `ToTrString()` | `ToCultureString(CultureInfo.GetCultureInfo("tr-TR"))` |
| `PropertyInfoExtensions.SetValue<T>()` | `TrySetConvertedValue(instance, value)` |
| `ToPixel()` | `point * 4m / 3m` |
| `Seconds()` | `TimeSpan.FromSeconds(n)` |
| `NameEqualityComparer` | `StringComparer.OrdinalIgnoreCase` |
| `array.ToList(mapFunction)` | `array.Select(map).ToList()` |
| `TypesExtensions.GetDefault<T>()` | `default(T)` |

## 2.0'da yeni

Form doğrulama öznitelikleri (`[Tckn]`, `[Iban]`, `[TurkishPhone]`, …), iller, plakalar ve posta kodları, Troy destekli kart doğrulama, Türk lirası biçimlendirme ve enum açıklamaları. Menüdeki rehberlere bakın.
