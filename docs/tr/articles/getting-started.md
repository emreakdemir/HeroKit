# Başlarken

## Kurulum

```bash
dotnet add package HeroKit
```

HeroKit .NET 8 ve .NET 10 hedefler ve hiçbir bağımlılığı yoktur.

## Kullanım

Her yardımcı bir uzantı metodudur. İhtiyacınız olan alanın namespace'ini ekleyin:

| Namespace | Kapsadığı alan |
|---|---|
| `HeroKit.Strings` | Kontroller, dönüşümler, slug, harf büyüklüğü, kısaltma ve maskeleme |
| `HeroKit.Turkish` | TCKN/VKN, telefon numaraları, iller, plakalar, posta kodları, lira, yazıyla tutar, resmî tatiller ve iş günleri, Türkçe harf dönüşümü |
| `HeroKit.Banking` | IBAN ve kart numarası doğrulama |
| `HeroKit.Validation` | Form doğrulama öznitelikleri: `[Tckn]`, `[Iban]`, `[TurkishPhone]`, ... |
| `HeroKit.Enumerables` | Null güvenli kontroller, koşullu filtreler, sayfalama |
| `HeroKit.DateTimes` | Biçimlendirme ve tarih aralıkları |
| `HeroKit.Decimals` | Fiyat yuvarlama ve KDV |
| `HeroKit.Objects`, `HeroKit.Types`, `HeroKit.Enums`, ... | Dönüşümler ve yansıma (reflection) yardımcıları |

```csharp
using HeroKit.Strings;

"HeroKit extensions".ToSlug();     // "herokit-extensions"
"42".ToInt();                      // 42
"HeroKit extensions".Truncate(10); // "HeroKit e…"
```

## Kurallar

- **Hatalı girdi istisna fırlatmaz.** Kontroller `false` döner; dönüşümlerin toleranslı `ToX` (varsayılan değer) ve denetimli `TryToX` biçimleri vardır; metin yardımcıları belgelendiği gibi `null` ya da boş metin döner.
- **Ayrıştırma kültürden bağımsızdır.** Kültür önemliyse bir `CultureInfo` verin. Örneğin `"07.03.2026".ToDateTime(provider: CultureInfo.GetCultureInfo("tr-TR"))`.
- **Kullanımdan kaldırılan API'ler** `[Obsolete]` ile işaretlenir, yerine geçeni mesajda yazar ve yalnızca bir sonraki ana sürümde kaldırılır.
