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
"HeroKit".Left(4);                    // "Hero"
"HeroKit".Right(4);                   // "oKit"
"TR33 0006\t1005".RemoveWhitespace(); // "TR3300061005"
```

`RemoveDiacritics` uses a fixed character table, not Unicode normalization, so the result is the same on every system, also with invariant globalization. It covers Turkish, Latin-1 and common Central European letters (`"Łódź"` becomes `"Lodz"`) and keeps other characters.

Casing and comparison with the rules of a language (`ToUpper`, `ToLower`, `ToTitleCase`, `EqualsIgnoreCase` with `Languages.Turkish`) are described in [Countries and languages](countries-and-languages.md).

`Truncate` never splits a surrogate pair. `Mask` keeps whitespace so formatted values keep their layout.
