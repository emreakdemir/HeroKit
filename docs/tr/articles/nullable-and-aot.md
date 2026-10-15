# Nullable ve AOT

## Nullable açıklamaları

Her genel API açıklamalıdır. Null kontrol yardımcıları sonucu derleyiciye bildirir:

```csharp
string? input = GetInput();

if (input.IsNotNullOrEmpty())
    Console.WriteLine(input.Length); // uyarı yok: input burada null değil
```

## Trimming ve Native AOT

Paket `IsAotCompatible` olarak işaretlidir. CI her pull request'te Native AOT ile bir test uygulaması yayımlar ve herhangi bir trimming uyarısında başarısız olur.

Yansımaya (reflection) dayanan bazı yardımcılar tam olarak kırpılamaz. Bunlar `[RequiresUnreferencedCode]` veya `[RequiresDynamicCode]` taşır, bu yüzden çağrı yerinde uyarı alırsınız:

- `TypesExtensions.GetExtensionMethods`, `GetExtensionMethod`, `GetRealType`

Dönüşümler, enum yardımcıları ve `TrySetConvertedValue` 2.0'dan beri AOT güvenlidir: yansıma yerine `IParsable<T>` ve genel enum API'lerini kullanırlar.
