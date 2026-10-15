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
"HeroKit".Left(4);                    // "Hero"
"HeroKit".Right(4);                   // "oKit"
"TR33 0006\t1005".RemoveWhitespace(); // "TR3300061005"
```

`RemoveDiacritics` Unicode normalleştirmesi yerine sabit bir karakter tablosu kullanır; bu yüzden sonuç her sistemde aynıdır, değişmez globalleştirmede de. Türkçe, Latin-1 ve yaygın Orta Avrupa harflerini kapsar (`"Łódź"` → `"Lodz"`), diğer karakterleri korur.

Bir dilin kurallarıyla harf büyüklüğü ve karşılaştırma (`Languages.Turkish` ile `ToUpper`, `ToLower`, `ToTitleCase`, `EqualsIgnoreCase`) [Ülkeler ve diller](countries-and-languages.md) sayfasında anlatılır.

`Truncate` vekil çiftini (surrogate pair) asla bölmez. `Mask` boşlukları korur, böylece biçimlendirilmiş değerlerin düzeni bozulmaz.
