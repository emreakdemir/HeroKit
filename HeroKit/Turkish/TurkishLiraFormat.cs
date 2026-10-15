namespace HeroKit.Turkish;

/// <summary>
/// Selects how <see cref="TurkishCurrencyExtensions.ToTurkishLira"/> writes the currency.
/// </summary>
public enum TurkishLiraFormat
{
    /// <summary>Symbol before the amount: "₺1.234,50".</summary>
    Symbol,

    /// <summary>Currency code after the amount: "1.234,50 TL".</summary>
    Code,
}
