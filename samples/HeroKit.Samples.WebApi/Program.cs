using HeroKit.FileTypes;
using HeroKit.Globalization;
using HeroKit.Globalization.Turkey;

var builder = WebApplication.CreateBuilder(args);

// One validator per policy, shared by all requests. It is immutable and thread-safe.
builder.Services.AddSingleton(new UploadValidator(new UploadPolicy
{
    AllowedTypes = [FileTypes.Jpeg, FileTypes.Png, FileTypes.WebP, FileTypes.Pdf],
}));

var app = builder.Build();

// Accepts an upload only when its content is an allowed type and fits its extension.
// Antiforgery is off because this sample is called by API clients; keep it on for browser forms.
app.MapPost("/documents", (IFormFile file, UploadValidator validator) =>
{
    using Stream stream = file.OpenReadStream();
    UploadCheckResult result = validator.Validate(stream, file.FileName);

    return result.IsSafe
        ? Results.Ok(new { file.FileName, result.DetectedType!.MimeType })
        : Results.BadRequest(new { Rejection = result.Rejection.ToString(), result.Message });
}).DisableAntiforgery();

// Tckn implements IParsable, so an invalid number in the route returns 400 before the handler runs.
app.MapGet("/people/{id}", (Tckn id) => Results.Ok(new { Id = id.ToMaskedString() }));

// Province by plate code or name ("34", "istanbul"), with its districts in Turkish alphabetical order.
app.MapGet("/provinces/{province}", (string province) =>
    Countries.Turkey.FindRegion(province) is { } region
        ? Results.Ok(new { region.Code, region.Name, Districts = Countries.Turkey.GetDistricts(region.Code).Select(d => d.Name) })
        : Results.NotFound());

// Amounts typed in Turkish format: "₺1.234,56" or "1.234,56 TL".
app.MapGet("/amounts", (string text) =>
    text.TryToAmount(Currencies.TurkishLira, out decimal amount)
        ? Results.Ok(new { Amount = amount, Words = amount.ToCurrencyWords(Currencies.TurkishLira) })
        : Results.BadRequest(new { Message = "Not a Turkish lira amount." }));

app.Run();

/// <summary>The entry point; public so integration tests can start the app.</summary>
public partial class Program;
