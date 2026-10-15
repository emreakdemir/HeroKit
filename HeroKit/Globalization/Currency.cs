namespace HeroKit.Globalization;

/// <summary>
/// A currency with its notation and names. Predefined currencies are in <see cref="Currencies"/>.
/// </summary>
/// <remarks>
/// Formatting uses the separators of this instance, not ICU culture data, so the output is the same on every system.
/// Create your own instance for other currencies or for unit names in another language.
/// </remarks>
/// <param name="isoCode">The ISO 4217 code, for example "TRY".</param>
/// <param name="symbol">The symbol, for example "₺".</param>
/// <param name="language">The language of <see cref="UnitName"/> and <see cref="SubunitName"/> and of amounts in words.</param>
public sealed class Currency(string isoCode, string symbol, Language language)
{
    /// <summary>Gets the ISO 4217 code.</summary>
    public string IsoCode { get; } = isoCode;

    /// <summary>Gets the symbol.</summary>
    public string Symbol { get; } = symbol;

    /// <summary>Gets the language of the unit names and of amounts in words.</summary>
    public Language Language { get; } = language;

    /// <summary>Gets the local abbreviation, for example "TL". Defaults to the ISO code.</summary>
    public string Abbreviation { get; init; } = isoCode;

    /// <summary>Gets the name of the main unit in <see cref="Language"/>, for example "Türk lirası".</summary>
    public string UnitName { get; init; } = isoCode;

    /// <summary>Gets the name of the subunit (1/100) in <see cref="Language"/>, for example "kuruş".</summary>
    public string SubunitName { get; init; } = string.Empty;

    /// <summary>Gets the plural of <see cref="UnitName"/>, for example "dollars". Null when the language does not inflect the unit after numbers.</summary>
    public string? UnitNamePlural { get; init; }

    /// <summary>Gets the plural of <see cref="SubunitName"/>, for example "cents". Null when the language does not inflect the subunit after numbers.</summary>
    public string? SubunitNamePlural { get; init; }

    /// <summary>Gets the word between the unit and the subunit in amounts in words, for example "and". Empty by default.</summary>
    public string SubunitConjunction { get; init; } = string.Empty;

    /// <summary>Gets the decimal separator. Defaults to ".".</summary>
    public string DecimalSeparator { get; init; } = ".";

    /// <summary>Gets the group separator. Defaults to ",".</summary>
    public string GroupSeparator { get; init; } = ",";

    /// <inheritdoc />
    public override string ToString() => IsoCode;
}
