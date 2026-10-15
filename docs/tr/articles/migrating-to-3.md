# 3.0'a geçiş

HeroKit 3.0, yalnızca Türkiye'ye özgü metotların (`...Tr`, `ToTurkish...`) yerine `Country`, `Language` ya da `Currency` alan tek bir metot kümesi getirir. Türkiye `Countries.Turkey`, `Languages.Turkish` ve `Currencies.TurkishLira` olarak hazır gelir. Diğer ülkeler eklendikleri anda aynı metotları kullanır: bkz. [Ülkeler ve diller](countries-and-languages.md).

Değişikliklerin çoğu, derleyicinin gösterdiği yeniden adlandırmalardır. Davranış ve sonuçlar aynıdır.

## Namespace'ler

| 2.x | 3.0 |
|---|---|
| `using HeroKit.Turkish;` | `using HeroKit.Globalization;` |
| `IsValidTckn`, `IsValidVkn` | `using HeroKit.Globalization.Turkey;` |

## Telefon numaraları

| 2.x | 3.0 |
|---|---|
| `value.IsValidPhoneTr()` | `value.IsValidPhone(Countries.Turkey)` |
| `value.IsValidMobileTr()` | `value.IsValidMobilePhone(Countries.Turkey)` |
| `value.ToE164Tr()` | `value.ToE164(Countries.Turkey)` |
| `value.FormatPhoneTr()` | `value.FormatPhone(Countries.Turkey)` |

## Kimlik, vergi, plaka, posta kodu ve bölgeler

| 2.x | 3.0 |
|---|---|
| `value.IsValidTckn()`, `value.IsValidVkn()` | değişmedi (namespace `HeroKit.Globalization.Turkey`) |
| — | `value.IsValidNationalId(Countries.Turkey)` (TCKN) |
| `value.IsValidTaxIdTr()` | `value.IsValidTaxId(Countries.Turkey)` |
| `value.IsValidPlateTr()` | `value.IsValidVehiclePlate(Countries.Turkey)` |
| `value.FormatPlateTr()` | `value.FormatVehiclePlate(Countries.Turkey)` |
| `value.IsValidPostalCodeTr()` | `value.IsValidPostalCode(Countries.Turkey)` |
| `"34".ToProvinceName()` | `Countries.Turkey.FindRegion("34")?.Name` |
| `6.ToProvinceName()` | `Countries.Turkey.FindRegion(6)?.Name` |
| `"sanliurfa".ToProvinceCode()` | `Countries.Turkey.FindRegion("sanliurfa")?.Code` |
| `TurkishProvinces.All` | `Countries.Turkey.Regions` (`Region(Code, Name)` listesi) |

`FindRegion` bilinmeyen bir bölge için `null` döner.

## Tutar, para birimi ve yazıyla sayı

| 2.x | 3.0 |
|---|---|
| `123.ToTurkishWords()` | `123.ToWords(Languages.Turkish)` |
| `amount.ToTurkishCurrencyWords()` | `amount.ToCurrencyWords(Currencies.TurkishLira)` |
| `amount.ToTurkishCurrencyWords("Euro", "sent")` | `amount.ToCurrencyWords(new Currency("EUR", "€", Languages.Turkish) { UnitName = "Euro", SubunitName = "sent" })` |
| `amount.ToTurkishLira()` | `amount.ToCurrencyString(Currencies.TurkishLira)` |
| `amount.ToTurkishLira(TurkishLiraFormat.Code)` | `amount.ToCurrencyString(Currencies.TurkishLira, CurrencyStyle.Abbreviation)` (`"1.234,50 TL"`) |
| — | `CurrencyStyle.IsoCode` (`"1.234,50 TRY"`) |

## Resmî tatiller ve iş günleri

| 2.x | 3.0 |
|---|---|
| `TurkishHolidays.GetHolidays(2026)` | `Countries.Turkey.Holidays!.GetHolidays(2026)` |
| `date.IsPublicHolidayTr()` | `date.IsPublicHoliday(Countries.Turkey)` |
| `date.IsHalfDayHolidayTr()` | `date.IsHalfDayHoliday(Countries.Turkey)` |
| `date.GetPublicHolidayTr()` | `date.GetPublicHoliday(Countries.Turkey)` |
| `date.IsBusinessDayTr()` | `date.IsBusinessDay(Countries.Turkey)` |
| `date.AddBusinessDaysTr(n)` | `date.AddBusinessDays(n, Countries.Turkey)` |
| `date.BusinessDaysUntilTr(end)` | `date.BusinessDaysUntil(end, Countries.Turkey)` |

## Harf büyüklüğü ve metin

| 2.x | 3.0 |
|---|---|
| `value.ToTitleCaseTr()` | `value.ToTitleCase(Languages.Turkish)` |
| `value.EqualsTr(other)` | `value.EqualsIgnoreCase(other, Languages.Turkish)` |
| `value.RemoveAccent()` | `value.RemoveDiacritics()` |
| `value.ClearTurkishChars()` | `value.RemoveDiacritics()` |

`RemoveDiacritics` sabit bir karakter tablosu kullanır; bu yüzden her sistemde aynı sonucu verir. Latin-1 ve yaygın Orta Avrupa harflerini de kapsar.

## Doğrulama öznitelikleri

| 2.x | 3.0 |
|---|---|
| `[TurkishPhone]`, `[TurkishPhone(MobileOnly = true)]` | `[PhoneNumber("TR")]`, `[PhoneNumber("TR", MobileOnly = true)]` |
| `[PlateTr]` | `[VehiclePlate("TR")]` |
| `[PostalCodeTr]` | `[PostalCode("TR")]` |
| `[TaxIdTr]` | `[TaxId("TR")]` |
| — | `[NationalId("TR")]` |
| `[Tckn]`, `[Vkn]`, `[Iban]`, `[CardNumber]` | değişmedi |

## FluentValidation

| 2.x | 3.0 |
|---|---|
| `TurkishPhone(mobileOnly)` | `PhoneNumber(Countries.Turkey, mobileOnly)` |
| `PlateTr()` | `VehiclePlate(Countries.Turkey)` |
| `PostalCodeTr()` | `PostalCode(Countries.Turkey)` |
| `TaxIdTr()` | `TaxId(Countries.Turkey)` |
| — | `NationalId(Countries.Turkey)` |
| `Tckn()`, `Vkn()`, `Iban()`, `CardNumber()` | değişmedi |

## 3.0'da yeni

Bunlar daha önce yayımlanmış bir sürümde yoktu:

- **e-Fatura ve e-Arşiv:** `GibInvoiceNumber` (`TryParse`, `Create`, `Next`), `IsValidGibInvoiceNumber`, `IsValidEttn`, `[GibInvoiceNumber]` ve `[Ettn]` öznitelikleri, `GibInvoiceNumber()` ve `Ettn()` FluentValidation kuralları.
- **ICU olmadan Türkçe sıralama:** `Languages.Turkish.TextRules!.Comparer` ve `IgnoreCaseComparer`; `Array.Sort`, `OrderBy(key, comparer)` ile ya da sözlük karşılaştırıcısı olarak kullanılır. Değişmez globalleştirmeyle de çalışır.
- **Harf büyüklüğü:** `ToUpper(Languages.Turkish)` ve `ToLower(Languages.Turkish)`.
- **Kendi ülkeniz, diliniz ya da para biriminiz:** `new Country(...)`, `Countries.Register`, `new Language(...)`, `new Currency(...)`.

## Güncelleme listesi

1. `using HeroKit.Turkish;` satırını `using HeroKit.Globalization;` ile değiştirin; `IsValidTckn` ya da `IsValidVkn` çağırdığınız yere `using HeroKit.Globalization.Turkey;` ekleyin.
2. Çağrıları yukarıdaki tablolara göre yeniden adlandırın. Derleyici eski adların hepsini listeler.
3. Ülke özniteliklerini ve FluentValidation kurallarını değiştirin.
