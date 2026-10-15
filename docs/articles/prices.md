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

## Currency formatting

```csharp
using HeroKit.Globalization;

1234.5m.ToCurrencyString(Currencies.TurkishLira);                             // "₺1.234,50"
1234.5m.ToCurrencyString(Currencies.TurkishLira, CurrencyStyle.Abbreviation); // "1.234,50 TL"
1234.5m.ToCurrencyString(Currencies.TurkishLira, CurrencyStyle.IsoCode);      // "1.234,50 TRY"
123.50m.ToCurrencyWords(Currencies.TurkishLira);                              // "yüz yirmi üç Türk lirası elli kuruş"
```

A `Currency` holds its own symbol, separators and unit names, so the output does not depend on the current culture. Create your own instance for other currencies, for example `new Currency("EUR", "€", Languages.Turkish) { UnitName = "Euro", SubunitName = "sent" }`. See [Countries and languages](countries-and-languages.md).

## Reading amounts

```csharp
using HeroKit.Globalization;

"₺1.234,56".ToAmount(Currencies.TurkishLira);                     // 1234.56
"1.234,56 TL".TryToAmount(Currencies.TurkishLira, out decimal a); // true, 1234.56
"1.5".TryToAmount(Currencies.TurkishLira, out _);                 // false: "." groups thousands
"$1,234.56".ToAmount(Currencies.USDollar);                        // 1234.56
```

`TryToAmount` and `ToAmount` read text with the separators of a currency, with or without its symbol, abbreviation or ISO code, and with a leading sign. Group separators must split the whole part into groups of three digits, so a mistyped price is rejected instead of being read as a different number. The culture of the machine is not used.

## Barcodes

```csharp
using HeroKit.Barcodes;

"8691234567890".IsValidGtin(); // true (EAN-13; GS1 Türkiye prefixes are 868 and 869)
"96385074".IsValidGtin();      // true (EAN-8)
```

`IsValidGtin` checks EAN-8, UPC-A, EAN-13 and GTIN-14 numbers with the GS1 check digit.

## Rounding

VAT helpers return unrounded values. Pass `decimals: 2` to round with `MidpointRounding.AwayFromZero`, the usual rule on invoices.
