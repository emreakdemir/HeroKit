# Collections and dates

## Collections

```csharp
using HeroKit.Enumerables;

items.IsNullOrEmpty();                               // null-safe; reads at most one element of a lazy sequence
products.WhereIf(onlyInStock, p => p.Stock > 0);     // filter only when the condition is true
query.OrderBy(x => x.Id).Page(pageNumber: 2, pageSize: 20); // 1-based paging
source.ToReadOnlyList();                             // read-only copy
```

`WhereIf` and `Page` also work on `IQueryable<T>`, so Entity Framework translates them to SQL.

## Dates

```csharp
using HeroKit.DateTimes;

var date = new DateTime(2024, 2, 15, 13, 45, 0);

date.StartOfMonth();     // 2024-02-01 00:00:00
date.EndOfMonth();       // 2024-02-29 23:59:59.9999999
date.StartOfWeek();      // Monday 2024-02-12
date.IsWeekend();        // false
new DateTime(1990, 2, 15).Age(asOf: date); // 34

date.AsFileName("Monthly Report"); // "monthly-report-2024-02-15-13-45-00"
```

Results keep the `DateTimeKind` of the input. `DateOnly` overloads are available for week, month, weekend, range and age helpers.
