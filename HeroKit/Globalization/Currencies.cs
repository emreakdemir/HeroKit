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
}
