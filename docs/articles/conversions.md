# Conversions

Conversions never throw for null or empty input. They return the default value instead.

```csharp
using HeroKit.Objects;
using HeroKit.Strings;

"42".ToInt();                       // 42
"abc".ToInt();                      // 0
"SecondValue".ToEnum<MyEnum>();     // MyEnum.SecondValue (name or defined number, case-insensitive)
"1.234,56".ToDecimal();             // 1234.56
"2026-03-07".ToDateTime();          // 7 March 2026, invariant culture
"yes".ToBoolean(trueCondition: "yes"); // true

object source = "42";
source.ConvertTo<int>();            // 42
source.TryCast(out int number);     // true
```

## Culture

Parsing uses the invariant culture unless you pass one:

```csharp
"07.03.2026".ToDateTime(CultureInfo.GetCultureInfo("tr-TR")); // 7 March 2026
```
