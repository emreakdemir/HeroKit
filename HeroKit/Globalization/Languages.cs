using HeroKit.Globalization.English;
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

    /// <summary>
    /// Gets English: invariant casing, case-insensitive alphabetical order and numbers in American English words.
    /// </summary>
    public static Language English { get; } = new("en", "English")
    {
        TextRules = EnglishTextRules.Instance,
        NumberSpeller = EnglishNumberSpeller.Instance,
    };
}
