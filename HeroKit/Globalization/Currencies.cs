namespace HeroKit.Globalization;

/// <summary>
/// Predefined currencies.
/// </summary>
public static class Currencies
{
    /// <summary>
    /// Gets the Turkish lira (TRY, ₺, TL) with Turkish unit names and Turkish separators.
    /// </summary>
    public static Currency TurkishLira { get; } = new("TRY", "₺", Languages.Turkish)
    {
        Abbreviation = "TL",
        UnitName = "Türk lirası",
        SubunitName = "kuruş",
        DecimalSeparator = ",",
        GroupSeparator = ".",
    };

    /// <summary>
    /// Gets the US dollar (USD): "$1,234.50", "one hundred twenty-three dollars and fifty cents".
    /// </summary>
    public static Currency USDollar { get; } = new("USD", "$", Languages.English)
    {
        UnitName = "dollar",
        UnitNamePlural = "dollars",
        SubunitName = "cent",
        SubunitNamePlural = "cents",
        SubunitConjunction = "and",
    };
}
