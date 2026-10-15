# Metinler

```csharp
using System.Globalization;
using HeroKit.Globalization;
using HeroKit.Strings;

// Kontroller
string? name = null;
name.IsNullOrEmpty();                 // true; false döndüğünde derleyici name'in null olmadığını bilir
",,,".IsNullOrEmpty(',');             // true: yalnızca ayırıcılar var

// Slug ve harf büyüklüğü
"Çağdaş Şehir İstanbul".ToSlug();     // "cagdas-sehir-istanbul"
"HeroKitExtensions".ToDashCase();     // "hero-kit-extensions"
"HELLO WORLD".ToTitleCase(CultureInfo.InvariantCulture); // "Hello World"
"istanbul ılıca".ToTitleCase(Languages.Turkish);         // "İstanbul Ilıca"
"ığüşöç".RemoveDiacritics();          // "igusoc"

// Kısaltma ve maskeleme
"HeroKit extensions".Truncate(10);    // "HeroKit e…"
"5321234567".Mask();                  // "******4567"
"0532 123 45 67".Mask(2);             // "**** *** ** 67"
"10000000146".MaskMiddle(2, 2);       // "10*******46"
"TR33 0006 1005 1978 6457 8413 26".MaskMiddle(4, 4); // "TR33 **** **** **** **** **13 26"
"emre.akdemir@example.com".MaskEmail(); // "e***********@example.com"
"Ayşe Nur Yılmaz".MaskName();         // "A*** N** Y*****"
"HeroKit".Left(4);                    // "Hero"
"HeroKit".Right(4);                   // "oKit"
"TR33 0006\t1005".RemoveWhitespace(); // "TR3300061005"
```

`RemoveDiacritics` Unicode normalleştirmesi yerine sabit bir karakter tablosu kullanır; bu yüzden sonuç her sistemde aynıdır, değişmez globalleştirmede de. Türkçe, Latin-1 ve yaygın Orta Avrupa harflerini kapsar (`"Łódź"` → `"Lodz"`), diğer karakterleri korur.

Bir dilin kurallarıyla harf büyüklüğü ve karşılaştırma (`Languages.Turkish` ile `ToUpper`, `ToLower`, `ToTitleCase`, `EqualsIgnoreCase`) [Ülkeler ve diller](countries-and-languages.md) sayfasında anlatılır.

`Truncate` vekil çiftini (surrogate pair) asla bölmez. `Mask` ve `MaskMiddle` boşlukları korur, böylece biçimlendirilmiş değerlerin düzeni bozulmaz. Kişisel verileri (KVKK) loglarda ve ekranlarda gizlemek için maskeleme metotlarını kullanın: `MaskMiddle`, iki ucu göstermeye yetmeyecek kadar kısa bir değeri tamamen gizler.
