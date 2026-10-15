using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;
using System.Text.Json;

namespace HeroKit.Globalization.Turkey;

/// <summary>
/// The content of the QR code (karekod) printed on a Turkish e-Fatura or e-Arşiv invoice, in the GİB JSON format.
/// </summary>
/// <remarks>
/// <para>The QR code holds a flat JSON object such as
/// <c>{"vkntckn":"4840847211","avkntckn":"11111111111","senaryo":"EARSIVFATURA","tip":"SATIS","tarih":"2026-10-15","no":"ABC2026000000001","ettn":"51d3bb18-9650-4c8e-825d-5a7f845be630","parabirimi":"TRY","malhizmettoplam":"100.00","kdvmatrah(20)":"100.00","hesaplanankdv(20)":"20.00","vergidahil":"120.00","odenecek":"120.00"}</c>.</para>
/// <para><see cref="TryParse"/> checks the fields that identify the invoice: the seller VKN or TCKN, the buyer VKN or TCKN
/// ("11111111111" for e-Arşiv buyers without one), the invoice number, the ETTN and the date. Amounts are optional.
/// The method reads the text only; it does not ask GİB whether the invoice exists.</para>
/// </remarks>
/// <example>
/// <code>
/// if (GibInvoiceQrCode.TryParse(scannedText, out GibInvoiceQrCode? qr))
/// {
///     qr.Number.ToString(); // "ABC2026000000001"
///     qr.PayableAmount;     // 120.00
///     qr.VatAmounts[20m];   // 20.00
/// }
/// </code>
/// </example>
public sealed class GibInvoiceQrCode
{
    private GibInvoiceQrCode(IReadOnlyDictionary<string, string> fields)
    {
        Fields = fields;
    }

    /// <summary>Gets the VKN or TCKN of the seller ("vkntckn").</summary>
    public required string SellerTaxId { get; init; }

    /// <summary>Gets the VKN or TCKN of the buyer ("avkntckn").</summary>
    public required string BuyerTaxId { get; init; }

    /// <summary>Gets the invoice number ("no").</summary>
    public required GibInvoiceNumber Number { get; init; }

    /// <summary>Gets the ETTN ("ettn").</summary>
    public required Guid Ettn { get; init; }

    /// <summary>Gets the invoice date ("tarih").</summary>
    public required DateOnly Date { get; init; }

    /// <summary>Gets the scenario ("senaryo"), for example "TEMELFATURA", "TICARIFATURA" or "EARSIVFATURA", or null.</summary>
    public string? Scenario => Get("senaryo");

    /// <summary>Gets the invoice type ("tip"), for example "SATIS" or "IADE", or null.</summary>
    public string? InvoiceType => Get("tip");

    /// <summary>Gets the ISO 4217 currency code ("parabirimi"), for example "TRY", or null.</summary>
    public string? CurrencyCode => Get("parabirimi");

    /// <summary>Gets the total of goods and services before tax ("malhizmettoplam"), or null.</summary>
    public decimal? LineTotal => Amount("malhizmettoplam");

    /// <summary>Gets the total including taxes ("vergidahil"), or null.</summary>
    public decimal? TaxInclusiveTotal => Amount("vergidahil");

    /// <summary>Gets the amount to pay ("odenecek"), or null.</summary>
    public decimal? PayableAmount => Amount("odenecek");

    /// <summary>Gets the VAT base per VAT rate ("kdvmatrah(20)" → 20).</summary>
    public IReadOnlyDictionary<decimal, decimal> VatBases => ByRate("kdvmatrah(");

    /// <summary>Gets the calculated VAT per VAT rate ("hesaplanankdv(20)" → 20).</summary>
    public IReadOnlyDictionary<decimal, decimal> VatAmounts => ByRate("hesaplanankdv(");

    /// <summary>Gets every field of the QR code as text, with keys in lower case.</summary>
    public IReadOnlyDictionary<string, string> Fields { get; }

    /// <summary>
    /// Reads the JSON text of an e-Fatura or e-Arşiv QR code.
    /// </summary>
    /// <param name="json">The text read from the QR code.</param>
    /// <param name="result">The QR code content when the method returns true; otherwise null.</param>
    /// <returns>True if the text is a JSON object with a valid seller and buyer tax id, invoice number, ETTN and date.</returns>
    public static bool TryParse([NotNullWhen(true)] string? json, [NotNullWhen(true)] out GibInvoiceQrCode? result)
    {
        result = null;
        if (string.IsNullOrWhiteSpace(json) || !TryReadFields(json, out Dictionary<string, string>? fields))
            return false;

        if (!fields.TryGetValue("vkntckn", out string? seller) || !IsTaxId(seller)
            || !fields.TryGetValue("avkntckn", out string? buyer) || !(IsTaxId(buyer) || buyer == "11111111111")
            || !GibInvoiceNumber.TryParse(fields.GetValueOrDefault("no"), out GibInvoiceNumber number)
            || !fields.TryGetValue("ettn", out string? ettnText) || !ettnText.IsValidEttn()
            || !TryParseDate(fields.GetValueOrDefault("tarih"), out DateOnly date))
        {
            return false;
        }

        result = new GibInvoiceQrCode(fields)
        {
            SellerTaxId = seller,
            BuyerTaxId = buyer,
            Number = number,
            Ettn = Guid.ParseExact(ettnText, "D"),
            Date = date,
        };
        return true;
    }

    private string? Get(string key) => Fields.TryGetValue(key, out string? value) && value.Length > 0 ? value : null;

    private decimal? Amount(string key) => TryParseAmount(Get(key), out decimal amount) ? amount : null;

    private Dictionary<decimal, decimal> ByRate(string prefix)
    {
        var result = new Dictionary<decimal, decimal>();
        foreach ((string key, string value) in Fields)
        {
            if (!key.StartsWith(prefix, StringComparison.Ordinal) || !key.EndsWith(')'))
                continue;

            string rateText = key[prefix.Length..^1];
            if (TryParseAmount(rateText, out decimal rate) && TryParseAmount(value, out decimal amount))
                result[rate] = amount;
        }

        return result;
    }

    private static bool IsTaxId(string value) => value.Length switch
    {
        10 => value.IsValidVkn(),
        11 => value.IsValidTckn(),
        _ => false,
    };

    private static bool TryParseDate(string? value, out DateOnly date) =>
        DateOnly.TryParseExact(value, ["yyyy-MM-dd", "dd-MM-yyyy", "dd.MM.yyyy"], CultureInfo.InvariantCulture, DateTimeStyles.None, out date);

    // Amounts use "." as the decimal separator; "," is accepted when no "." is present.
    private static bool TryParseAmount(string? value, out decimal amount)
    {
        amount = 0m;
        if (string.IsNullOrEmpty(value))
            return false;

        string text = value.Contains('.') ? value : value.Replace(',', '.');
        return decimal.TryParse(text, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out amount);
    }

    private static bool TryReadFields(string json, [NotNullWhen(true)] out Dictionary<string, string>? fields)
    {
        fields = null;
        var result = new Dictionary<string, string>(StringComparer.Ordinal);
        try
        {
            var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(json.Trim()));
            if (!reader.Read() || reader.TokenType != JsonTokenType.StartObject)
                return false;

            while (reader.Read() && reader.TokenType == JsonTokenType.PropertyName)
            {
                string key = reader.GetString()!.Trim().ToLowerInvariant();
                if (!reader.Read())
                    return false;

                switch (reader.TokenType)
                {
                    case JsonTokenType.String:
                        result[key] = reader.GetString()!.Trim();
                        break;
                    case JsonTokenType.Number:
                        result[key] = Encoding.UTF8.GetString(reader.ValueSpan);
                        break;
                    case JsonTokenType.Null:
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }

            if (reader.TokenType != JsonTokenType.EndObject)
                return false;
        }
        catch (JsonException)
        {
            return false;
        }

        fields = result;
        return true;
    }
}
