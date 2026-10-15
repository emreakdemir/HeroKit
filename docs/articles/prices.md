# Prices and VAT

```csharp
using HeroKit.Decimals;

12.37m.RoundToStep(0.05m);          // 12.35
12.30m.ToPsychologicalPrice();      // 12.99
100m.AddPercent(18);                // 118
100m.IncludeVat(20);                // 120
120m.ExcludeVat(20);                // 100
120m.VatAmountFromGross(20);        // 20
```

VAT helpers return unrounded values. Pass `decimals: 2` to round with `MidpointRounding.AwayFromZero`, the usual rule on invoices.
