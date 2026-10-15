using HeroKit.Globalization.Turkey;

namespace HeroKit.Globalization;

/// <summary>
/// Predefined languages.
/// </summary>
public static class Languages
{
    /// <summary>
    /// Gets Turkish: İ/ı-aware casing, Turkish alphabetical order and numbers in Turkish words.
    /// </summary>
    public static Language Turkish { get; } = new("tr", "Turkish")
    {
        TextRules = TurkishTextRules.Instance,
        NumberSpeller = TurkishNumberSpeller.Instance,
    };
}
