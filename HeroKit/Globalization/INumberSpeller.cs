namespace HeroKit.Globalization;

/// <summary>
/// Writes numbers in words in a language.
/// </summary>
public interface INumberSpeller
{
    /// <summary>
    /// Writes an integer in words.
    /// </summary>
    /// <param name="value">The number.</param>
    /// <returns>The number in lower-case words.</returns>
    string Spell(long value);
}
