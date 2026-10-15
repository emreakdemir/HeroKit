namespace HeroKit.Globalization;

/// <summary>
/// Selects how a currency is written next to an amount.
/// </summary>
public enum CurrencyStyle
{
    /// <summary>Symbol before the amount: "₺1.234,50".</summary>
    Symbol,

    /// <summary>Local abbreviation after the amount: "1.234,50 TL".</summary>
    Abbreviation,

    /// <summary>ISO 4217 code after the amount: "1.234,50 TRY".</summary>
    IsoCode,
}
