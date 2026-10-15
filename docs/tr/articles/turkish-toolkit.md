# Türkiye araç seti

Türkiye verisi için doğrulama, biçimlendirme ve metin yardımcıları.

## Kimlik ve vergi numaraları

```csharp
using HeroKit.Turkish;

"10000000146".IsValidTckn();   // true (TCKN)
"4840847211".IsValidVkn();     // true (VKN)
"4840847211".IsValidTaxIdTr(); // true: VKN veya TCKN
```

Bunlar sağlama toplamı kontrolleridir. Bir numaranın biçiminin doğru olduğunu gösterir; numaranın verilmiş olduğunu göstermez. Girdi yalnızca rakam içermelidir. IBAN için ISO 13616 mod-97 kullanılır; Türkiye IBAN'larında ayrıca uzunluk (26) kontrol edilir.

## IBAN

```csharp
using HeroKit.Banking;

var iban = "tr33 0006 1005 1978 6457 8413 26";

iban.IsValidIban();   // true
iban.NormalizeIban(); // "TR330006100519786457841326"
iban.FormatIban();    // "TR33 0006 1005 1978 6457 8413 26"
```

IBAN doğrulama her ülke için çalışır.

## İller, plakalar ve posta kodları

```csharp
"34".ToProvinceName();          // "İstanbul"
6.ToProvinceName();             // "Ankara"
"sanliurfa".ToProvinceCode();   // 63 (büyük-küçük harf ve aksanlar yok sayılır)
TurkishProvinces.All.Count;     // 81

"34abc123".FormatPlateTr();     // "34 ABC 123"
"06 A 1234".IsValidPlateTr();   // true
"34 Q 1234".IsValidPlateTr();   // false: Q plakada kullanılan bir harf değil

"34710".IsValidPostalCodeTr();  // true
```

## Telefon numaraları

```csharp
"0532 123 45 67".ToE164Tr();        // "+905321234567"
"+905321234567".FormatPhoneTr();    // "0532 123 45 67"
"(0212) 555 12 34".IsValidPhoneTr(); // true
"0212 555 12 34".IsValidMobileTr();  // false
```

Kabul edilen önekler: `+90`, `0090`, `90`, `0` veya öneksiz. Kontrol yapısaldır; operatör ya da alan kodu izin listesi değildir.

## Yazıyla tutar

```csharp
123.ToTurkishWords();                          // "yüz yirmi üç"
1_001_000L.ToTurkishWords();                   // "bir milyon bin"
123.50m.ToTurkishCurrencyWords();
// "yüz yirmi üç Türk lirası elli kuruş"

10.20m.ToTurkishCurrencyWords("Euro", "sent");
// "on Euro yirmi sent"
```

## Türk lirası

```csharp
1234.5m.ToTurkishLira();                       // "₺1.234,50"
1234.5m.ToTurkishLira(TurkishLiraFormat.Code); // "1.234,50 TL"
```

## Resmî tatiller ve iş günleri

```csharp
new DateOnly(2026, 10, 29).IsPublicHolidayTr();   // true  (Cumhuriyet Bayramı)
new DateOnly(2026, 10, 28).IsHalfDayHolidayTr();  // true  (öğleden sonra tatil)
new DateOnly(2026, 3, 20).GetPublicHolidayTr()?.Name; // "Ramazan Bayramı 1. Gün"

new DateOnly(2026, 3, 19).AddBusinessDaysTr(1);   // 2026-03-23, bayramı ve hafta sonunu atlar
new DateOnly(2026, 10, 26).BusinessDaysUntilTr(new DateOnly(2026, 10, 30)); // 3

foreach (PublicHoliday holiday in TurkishHolidays.GetHolidays(2026))
    Console.WriteLine($"{holiday.Date:dd.MM.yyyy} {holiday.Name}{(holiday.IsHalfDay ? " (yarım gün)" : "")}");
```

- Sabit tatiller 2429 sayılı Kanun'a göre belirlenir. Ramazan ve Kurban Bayramı tarihleri Diyanet takviminden gelir ve `TurkishHolidays.FirstSupportedYear`–`LastSupportedYear` (2022–2027) aralığı için vardır. Diğer yıllarda eksik veri döndürmek yerine istisna fırlatılır.
- İş günleri, tam gün tatiller hariç Pazartesi'den Cuma'ya kadardır. Yarım günler (arife ve 28 Ekim) iş günü sayılır.
- İdari izin köprü günleri duruma göre ilan edilir ve dahil değildir.

## Harf büyüklüğü ve karşılaştırma

```csharp
"istanbul ılıca".ToTitleCaseTr(); // "İstanbul Ilıca"
"IŞIK".EqualsTr("ışık");          // true
"ISIK".EqualsTr("isik");          // false: I, ı harfinin büyüğüdür
```

Diğer kültürler için `HeroKit.Strings` içindeki `ToTitleCase(culture)` ve `EqualsIgnoreCase(other, culture)` metotlarını kullanın.
