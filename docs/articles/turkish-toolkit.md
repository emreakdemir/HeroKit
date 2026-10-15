# Turkish toolkit

Validation, formatting and text helpers for Turkish data.

## Identity and tax numbers

```csharp
using HeroKit.Turkish;

"10000000146".IsValidTckn();   // true (TCKN)
"4840847211".IsValidVkn();     // true (VKN)
"4840847211".IsValidTaxIdTr(); // true: VKN or TCKN
```

These are checksum checks. They confirm that a number is well formed, not that it is issued. Input must contain digits only. ISO 13616 mod-97 is used for IBANs; Turkish IBANs also get a length check (26).

## IBAN

```csharp
using HeroKit.Banking;

var iban = "tr33 0006 1005 1978 6457 8413 26";

iban.IsValidIban();   // true
iban.NormalizeIban(); // "TR330006100519786457841326"
iban.FormatIban();    // "TR33 0006 1005 1978 6457 8413 26"
```

IBAN validation works for every country.

## Phone numbers

```csharp
"0532 123 45 67".ToE164Tr();        // "+905321234567"
"+905321234567".FormatPhoneTr();    // "0532 123 45 67"
"(0212) 555 12 34".IsValidPhoneTr(); // true
"0212 555 12 34".IsValidMobileTr();  // false
```

Accepted prefixes: `+90`, `0090`, `90`, `0` or none. The check is structural, not an operator or area-code allow-list.

## Amounts in words

```csharp
123.ToTurkishWords();                          // "yüz yirmi üç"
1_001_000L.ToTurkishWords();                   // "bir milyon bin"
123.50m.ToTurkishCurrencyWords();
// "yüz yirmi üç Türk lirası elli kuruş"

10.20m.ToTurkishCurrencyWords("Euro", "sent");
// "on Euro yirmi sent"
```

## Casing and comparison

```csharp
"istanbul ılıca".ToTitleCaseTr(); // "İstanbul Ilıca"
"IŞIK".EqualsTr("ışık");          // true
"ISIK".EqualsTr("isik");          // false: I is the capital of ı
```

For other cultures use `ToTitleCase(culture)` and `EqualsIgnoreCase(other, culture)` from `HeroKit.Strings`.
