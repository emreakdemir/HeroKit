---
_layout: landing
---

# HeroKit

Extension methods for everyday .NET types, with first-class support for Turkish data.

```bash
dotnet add package HeroKit
```

```csharp
using HeroKit.Turkish;
using HeroKit.Banking;
using HeroKit.Strings;

"10000000146".IsValidTckn();                  // true
"TR330006100519786457841326".FormatIban();   // "TR33 0006 1005 1978 6457 8413 26"
123.50m.ToTurkishCurrencyWords();             // "yüz yirmi üç Türk lirası elli kuruş"
"0532 123 45 67".Mask(2);                     // "**** *** ** 67"
```

- **Safe by default:** nullable annotations on every API, culture-invariant parsing.
- **Trimming and Native AOT ready:** verified in CI with a real AOT publish.
- **Stable API:** tracked with analyzers; nothing is removed without an `[Obsolete]` release first.
- **No dependencies.** Targets .NET 8 and .NET 10.

[Get started](articles/getting-started.md) · [API reference](api/HeroKit.Strings.yml) · [NuGet](https://www.nuget.org/packages/HeroKit)
