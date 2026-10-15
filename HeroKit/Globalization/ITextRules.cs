namespace HeroKit.Globalization;

/// <summary>
/// Casing and sorting rules of a language that do not depend on ICU or the operating system's culture data.
/// </summary>
public interface ITextRules
{
    /// <summary>
    /// Converts a character to upper case.
    /// </summary>
    /// <param name="value">The character.</param>
    /// <returns>The upper-case character.</returns>
    char ToUpper(char value);

    /// <summary>
    /// Converts a character to lower case.
    /// </summary>
    /// <param name="value">The character.</param>
    /// <returns>The lower-case character.</returns>
    char ToLower(char value);

    /// <summary>
    /// Gets a comparer that sorts in the language's alphabetical order, lower case first.
    /// </summary>
    StringComparer Comparer { get; }

    /// <summary>
    /// Gets a comparer that sorts in the language's alphabetical order and ignores case.
    /// </summary>
    StringComparer IgnoreCaseComparer { get; }
}
