namespace HeroKit.Globalization.English;

// English casing and alphabetical order without ICU. Casing is invariant; sorting ignores case first
// and puts lower case first when two strings differ only in case ("apple", "Apple", "banana").
internal sealed class EnglishTextRules : ITextRules
{
    public static EnglishTextRules Instance { get; } = new();

    public char ToUpper(char value) => char.ToUpperInvariant(value);

    public char ToLower(char value) => char.ToLowerInvariant(value);

    public StringComparer Comparer { get; } = new EnglishComparer();

    public StringComparer IgnoreCaseComparer => StringComparer.OrdinalIgnoreCase;

    private sealed class EnglishComparer : StringComparer
    {
        public override int Compare(string? x, string? y)
        {
            int result = StringComparer.OrdinalIgnoreCase.Compare(x, y);
            return result != 0 ? result : -string.CompareOrdinal(x, y);
        }

        public override bool Equals(string? x, string? y) => string.Equals(x, y, StringComparison.Ordinal);

        public override int GetHashCode(string obj) => StringComparer.Ordinal.GetHashCode(obj);
    }
}
