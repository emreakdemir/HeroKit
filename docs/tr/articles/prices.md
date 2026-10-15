# Fiyat, KDV ve kartlar

```csharp
using HeroKit.Decimals;

12.37m.RoundToStep(0.05m);          // 12.35
12.30m.ToPsychologicalPrice();      // 12.99
100m.AddPercent(18);                // 118
100m.IncludeVat(20);                // 120
120m.ExcludeVat(20);                // 100
120m.VatAmountFromGross(20);        // 20
```

## Kartlar

```csharp
using HeroKit.Banking;

"4111 1111 1111 1111".IsValidCardNumber(); // true (uzunluk ve Luhn)
"4111111111111111".GetCardBrand();         // CardBrand.Visa
troyCard.GetCardBrand();                   // CardBrand.Troy (önek 9792)
"4111111111111111".Mask();                 // "************1111"
```

Markalar: Visa, Mastercard, American Express, Discover, JCB ve Troy.

## Para birimi biçimlendirme

```csharp
using HeroKit.Globalization;

1234.5m.ToCurrencyString(Currencies.TurkishLira);                             // "₺1.234,50"
1234.5m.ToCurrencyString(Currencies.TurkishLira, CurrencyStyle.Abbreviation); // "1.234,50 TL"
1234.5m.ToCurrencyString(Currencies.TurkishLira, CurrencyStyle.IsoCode);      // "1.234,50 TRY"
123.50m.ToCurrencyWords(Currencies.TurkishLira);                              // "yüz yirmi üç Türk lirası elli kuruş"
```

`Currency` kendi simgesini, ayırıcılarını ve birim adlarını taşır; çıktı geçerli kültüre bağlı değildir. Diğer para birimleri için kendi örneğinizi oluşturun; örneğin `new Currency("EUR", "€", Languages.Turkish) { UnitName = "Euro", SubunitName = "sent" }`. Bkz. [Ülkeler ve diller](countries-and-languages.md).

## Yuvarlama

KDV yardımcıları yuvarlanmamış değer döner. Faturalarda olağan kural olan `MidpointRounding.AwayFromZero` ile yuvarlamak için `decimals: 2` verin.
