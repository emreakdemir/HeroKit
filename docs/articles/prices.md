# Prices, VAT and cards

```csharp
using HeroKit.Decimals;

12.37m.RoundToStep(0.05m);          // 12.35
12.30m.ToPsychologicalPrice();      // 12.99
100m.AddPercent(18);                // 118
100m.IncludeVat(20);                // 120
120m.ExcludeVat(20);                // 100
120m.VatAmountFromGross(20);        // 20
```

## Cards

```csharp
using HeroKit.Banking;

"4111 1111 1111 1111".IsValidCardNumber(); // true (length and Luhn)
"4111111111111111".GetCardBrand();         // CardBrand.Visa
troyCard.GetCardBrand();                   // CardBrand.Troy (prefix 9792)
"4111111111111111".Mask();                 // "************1111"
```

Brands: Visa, Mastercard, American Express, Discover, JCB and Troy.

## Rounding

VAT helpers return unrounded values. Pass `decimals: 2` to round with `MidpointRounding.AwayFromZero`, the usual rule on invoices.
