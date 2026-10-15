namespace HeroKit.Globalization;

/// <summary>
/// Text helpers that use the <see cref="ITextRules"/> and <see cref="INumberSpeller"/> of a <see cref="Language"/>.
/// They do not depend on ICU, so they also work with invariant globalization.
/// </summary>
public static class LanguageExtensions
{
    /// <summary>
    /// Converts a string to upper case with the rules of a language.
    /// </summary>
    /// <param name="value">The string.</param>
    /// <param name="language">The language, for example <see cref="Languages.Turkish"/>.</param>
    /// <returns>The upper-case string, or an empty string if <paramref name="value"/> is null.</returns>
    /// <example>
    /// <code>
    /// "istanbul".ToUpper(Languages.Turkish); // "İSTANBUL"
    /// </code>
    /// </example>
    public static string ToUpper(this string? value, Language language) => Map(value, language, upper: true);

    /// <summary>
    /// Converts a string to lower case with the rules of a language.
    /// </summary>
    /// <param name="value">The string.</param>
    /// <param name="language">The language.</param>
    /// <returns>The lower-case string, or an empty string if <paramref name="value"/> is null.</returns>
    public static string ToLower(this string? value, Language language) => Map(value, language, upper: false);

    /// <summary>
    /// Converts a string to title case with the rules of a language.
    /// </summary>
    /// <remarks>
    /// Words start after whitespace or a hyphen. Letters after an apostrophe stay lower case: "istanbul'da" becomes "İstanbul'da".
    /// </remarks>
    /// <param name="value">The string.</param>
    /// <param name="language">The language.</param>
    /// <returns>The title-cased string, or an empty string if <paramref name="value"/> is null.</returns>
    /// <example>
    /// <code>
    /// "istanbul ılıca".ToTitleCase(Languages.Turkish); // "İstanbul Ilıca"
    /// </code>
    /// </example>
    public static string ToTitleCase(this string? value, Language language)
    {
        ITextRules rules = Rules(language);

        if (string.IsNullOrEmpty(value))
            return string.Empty;

        return string.Create(value.Length, (value, rules), static (destination, state) =>
        {
            bool startOfWord = true;

            for (int i = 0; i < state.value.Length; i++)
            {
                char c = state.value[i];
                destination[i] = startOfWord ? state.rules.ToUpper(c) : state.rules.ToLower(c);
                startOfWord = char.IsWhiteSpace(c) || c == '-';
            }
        });
    }

    /// <summary>
    /// Compares two strings for equality, ignoring case with the rules of a language.
    /// </summary>
    /// <param name="value">The first string.</param>
    /// <param name="other">The second string.</param>
    /// <param name="language">The language. With Turkish, "I" matches "ı" and "İ" matches "i".</param>
    /// <returns>True if both are null or equal ignoring case; otherwise false.</returns>
    public static bool EqualsIgnoreCase(this string? value, string? other, Language language) =>
        Rules(language).IgnoreCaseComparer.Equals(value, other);

    /// <summary>
    /// Writes an integer in words in a language.
    /// </summary>
    /// <param name="value">The number.</param>
    /// <param name="language">The language.</param>
    /// <returns>The number in lower-case words.</returns>
    /// <example>
    /// <code>
    /// 1234L.ToWords(Languages.Turkish); // "bin iki yüz otuz dört"
    /// </code>
    /// </example>
    public static string ToWords(this long value, Language language) => Speller(language).Spell(value);

    /// <summary>
    /// Writes an integer in words in a language.
    /// </summary>
    /// <param name="value">The number.</param>
    /// <param name="language">The language.</param>
    /// <returns>The number in lower-case words.</returns>
    public static string ToWords(this int value, Language language) => Speller(language).Spell(value);

    private static string Map(string? value, Language language, bool upper)
    {
        ITextRules rules = Rules(language);

        if (string.IsNullOrEmpty(value))
            return string.Empty;

        return string.Create(value.Length, (value, rules, upper), static (destination, state) =>
        {
            for (int i = 0; i < state.value.Length; i++)
                destination[i] = state.upper ? state.rules.ToUpper(state.value[i]) : state.rules.ToLower(state.value[i]);
        });
    }

    private static ITextRules Rules(Language language)
    {
        ArgumentNullException.ThrowIfNull(language);
        return language.RequireTextRules();
    }

    private static INumberSpeller Speller(Language language)
    {
        ArgumentNullException.ThrowIfNull(language);
        return language.RequireNumberSpeller();
    }
}
