namespace HeroKit.Globalization.Turkey;

/// <summary>
/// Validation helpers for Turkish e-Fatura and e-Arşiv documents.
/// </summary>
public static class EInvoiceExtensions
{
    /// <summary>
    /// Checks whether a string is a valid e-Fatura or e-Arşiv invoice number in the GİB format (3-character series, 4-digit year, 9-digit sequence).
    /// </summary>
    /// <param name="value">The invoice number, for example "ABC2026000000001".</param>
    /// <returns>True if the value has the GİB format; otherwise false.</returns>
    /// <example>
    /// <code>
    /// "ABC2026000000001".IsValidGibInvoiceNumber(); // true
    /// "ABC2026000000000".IsValidGibInvoiceNumber(); // false: sequence starts at 1
    /// </code>
    /// </example>
    public static bool IsValidGibInvoiceNumber(this string? value) => GibInvoiceNumber.TryParse(value, out _);

    /// <summary>
    /// Checks whether a string is a valid ETTN (Evrensel Tekil Tanımlama Numarası): a UUID in the 8-4-4-4-12 hexadecimal format.
    /// </summary>
    /// <param name="value">The ETTN, for example "51d3bb18-9650-4c8e-825d-5a7f845be630".</param>
    /// <returns>True if the value is a non-empty UUID with hyphens; otherwise false.</returns>
    public static bool IsValidEttn(this string? value) =>
        Guid.TryParseExact(value, "D", out Guid ettn) && ettn != Guid.Empty;
}
