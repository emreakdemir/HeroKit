namespace HeroKit.Globalization;

/// <summary>
/// A language and its rules. Predefined languages are in <see cref="Languages"/>.
/// </summary>
/// <remarks>
/// Create your own instance to support another language: implement the rule interfaces and pass them to the properties.
/// </remarks>
/// <param name="code">The ISO 639-1 code, for example "tr".</param>
/// <param name="name">The English name, for example "Turkish".</param>
public sealed class Language(string code, string name)
{
    /// <summary>
    /// Gets the ISO 639-1 code.
    /// </summary>
    public string Code { get; } = code;

    /// <summary>
    /// Gets the English name.
    /// </summary>
    public string Name { get; } = name;

    /// <summary>
    /// Gets the casing and sorting rules, or null if the language has none.
    /// </summary>
    public ITextRules? TextRules { get; init; }

    /// <summary>
    /// Gets the speller that writes numbers in words, or null if the language has none.
    /// </summary>
    public INumberSpeller? NumberSpeller { get; init; }

    /// <inheritdoc />
    public override string ToString() => Name;

    internal ITextRules RequireTextRules() =>
        TextRules ?? throw new NotSupportedException($"{Name} has no text rules.");

    internal INumberSpeller RequireNumberSpeller() =>
        NumberSpeller ?? throw new NotSupportedException($"{Name} has no number speller.");
}
