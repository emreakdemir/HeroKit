# HeroKit sample: ASP.NET Core minimal API

A small API that shows HeroKit and HeroKit.FileTypes in an ASP.NET Core app:

| Endpoint | Shows |
|---|---|
| `POST /documents` | `UploadValidator` from dependency injection: content detection, allow-list, programs, double extensions, SVG and macros |
| `GET /people/{id}` | `Tckn` as a route value: invalid numbers return 400 before the handler runs |
| `GET /provinces/{province}` | `FindRegion` and `GetDistricts` |
| `GET /amounts?text=₺1.234,56` | `TryToAmount` and `ToCurrencyWords` |

```bash
dotnet run --project samples/HeroKit.Samples.WebApi
```

Then send the requests in `requests.http`. The integration tests in `HeroKit.Tests/Samples` call the same endpoints.
