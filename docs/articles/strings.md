# Strings

```csharp
using System.Globalization;
using HeroKit.Globalization;
using HeroKit.Strings;

// Checks
string? name = null;
name.IsNullOrEmpty();                 // true, and the compiler knows name is not null when false
",,,".IsNullOrEmpty(',');             // true: only separators

// Slugs and casing
"Çağdaş Şehir İstanbul".ToSlug();     // "cagdas-sehir-istanbul"
"HeroKitExtensions".ToDashCase();     // "hero-kit-extensions"
"HELLO WORLD".ToTitleCase(CultureInfo.InvariantCulture); // "Hello World"
"istanbul ılıca".ToTitleCase(Languages.Turkish);         // "İstanbul Ilıca"
"ığüşöç".RemoveDiacritics();          // "igusoc"

// Shortening and masking
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

`RemoveDiacritics` uses a fixed character table, not Unicode normalization, so the result is the same on every system, also with invariant globalization. It covers Turkish, Latin-1 and common Central European letters (`"Łódź"` becomes `"Lodz"`) and keeps other characters.

Casing and comparison with the rules of a language (`ToUpper`, `ToLower`, `ToTitleCase`, `EqualsIgnoreCase` with `Languages.Turkish`) are described in [Countries and languages](countries-and-languages.md).

`Truncate` never splits a surrogate pair. `Mask` and `MaskMiddle` keep whitespace so formatted values keep their layout. Use the masking methods to keep personal data (KVKK, GDPR) out of logs and screens: `MaskMiddle` hides a value completely when it is too short to keep both ends.
