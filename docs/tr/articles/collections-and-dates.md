# Koleksiyonlar ve tarihler

## Koleksiyonlar

```csharp
using HeroKit.Enumerables;

items.IsNullOrEmpty();                               // null güvenli; tembel (lazy) bir dizide en fazla bir öğe okur
products.WhereIf(onlyInStock, p => p.Stock > 0);     // yalnızca koşul true ise filtreler
query.OrderBy(x => x.Id).Page(pageNumber: 2, pageSize: 20); // 1 tabanlı sayfalama
source.ToReadOnlyList();                             // salt okunur kopya
```

`WhereIf` ve `Page`, `IQueryable<T>` üzerinde de çalışır; bu yüzden Entity Framework bunları SQL'e çevirir.

## Tarihler

```csharp
using HeroKit.DateTimes;

var date = new DateTime(2024, 2, 15, 13, 45, 0);

date.StartOfMonth();     // 2024-02-01 00:00:00
date.EndOfMonth();       // 2024-02-29 23:59:59.9999999
date.StartOfWeek();      // Pazartesi 2024-02-12
date.IsWeekend();        // false
new DateTime(1990, 2, 15).Age(asOf: date); // 34

date.AsFileName("Monthly Report"); // "monthly-report-2024-02-15-13-45-00"
```

Sonuçlar girdinin `DateTimeKind` değerini korur. Hafta, ay, hafta sonu, aralık ve yaş yardımcıları için `DateOnly` aşırı yüklemeleri de vardır.
