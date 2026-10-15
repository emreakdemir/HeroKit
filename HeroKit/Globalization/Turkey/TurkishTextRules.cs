namespace HeroKit.Globalization.Turkey;

// <summary>
// Compares strings in Turkish alphabetical order without depending on ICU or the tr-TR culture,
// so sorting also works in containers with invariant globalization.
// </summary>
// <remarks>
// <para>Letters follow the Turkish alphabet: a b c ç d e f g ğ h ı i j k l m n o ö p q r s ş t u ü v w x y z.
// "I" is the capital of "ı" and "İ" is the capital of "i". Accented Latin letters such as "é" sort with their base letter.</para>
// <para>Order of character groups: whitespace, punctuation and symbols, digits, letters, then other characters by code point.
// Case only decides when two strings are otherwise equal; lower case comes first.</para>
// </remarks>
// <example>
// <code>
// string[] names = ["Zeynep", "Çağla", "Ilgın", "İpek", "Cem", "Şule"];
// Array.Sort(names, TurkishStringComparer.Instance); // Cem, Çağla, Ilgın, İpek, Şule, Zeynep
// </code>
// </example>
internal sealed class TurkishCollation : StringComparer
{
    private const string Alphabet = "abcçdefgğhıijklmnoöpqrsştuüvwxyz";

    // Primary weight per lower-case letter; 0 means "not a Turkish letter".
    private static readonly Dictionary<char, int> Weights = BuildWeights();

    private readonly bool _ignoreCase;

    private TurkishCollation(bool ignoreCase) => _ignoreCase = ignoreCase;

    public static TurkishCollation Instance { get; } = new(ignoreCase: false);

    public static TurkishCollation IgnoreCase { get; } = new(ignoreCase: true);

    public override int Compare(string? x, string? y)
    {
        if (ReferenceEquals(x, y))
            return 0;
        if (x is null)
            return -1;
        if (y is null)
            return 1;

        int primary = ComparePrimary(x, y);
        if (primary != 0 || _ignoreCase)
            return primary;

        // Tertiary level: same letters, decide by case, lower case first.
        for (int i = 0; i < x.Length && i < y.Length; i++)
        {
            bool xUpper = IsUpper(x[i]);
            bool yUpper = IsUpper(y[i]);

            if (xUpper != yUpper)
                return xUpper ? 1 : -1;
        }

        return string.CompareOrdinal(x, y);
    }

    public override bool Equals(string? x, string? y) => Compare(x, y) == 0;

    public override int GetHashCode(string obj)
    {
        ArgumentNullException.ThrowIfNull(obj);

        if (!_ignoreCase)
            return string.GetHashCode(obj, StringComparison.Ordinal);

        var hash = new HashCode();
        foreach (char c in obj)
            hash.Add(Fold(c));

        return hash.ToHashCode();
    }

    private static int ComparePrimary(string x, string y)
    {
        int length = Math.Min(x.Length, y.Length);

        for (int i = 0; i < length; i++)
        {
            int result = Key(x[i]).CompareTo(Key(y[i]));
            if (result != 0)
                return result;
        }

        return x.Length.CompareTo(y.Length);
    }

    // Sort key: group in the high bits, weight inside the group in the low bits.
    private static long Key(char c)
    {
        char folded = Fold(c);

        if (Weights.TryGetValue(folded, out int weight))
            return (3L << 32) | (uint)weight;
        if (char.IsWhiteSpace(c))
            return (0L << 32) | c;
        if (char.IsDigit(c))
            return (2L << 32) | c;
        if (char.IsPunctuation(c) || char.IsSymbol(c))
            return (1L << 32) | c;

        return (4L << 32) | c;
    }

    // Lower-cases with Turkish rules and maps accented Latin letters to their base letter.
    private static char Fold(char c) => c switch
    {
        'I' => 'ı',
        'İ' => 'i',
        'Ç' => 'ç',
        'Ğ' => 'ğ',
        'Ö' => 'ö',
        'Ş' => 'ş',
        'Ü' => 'ü',
        >= 'A' and <= 'Z' => (char)(c + 32),
        'á' or 'à' or 'â' or 'ä' or 'ã' or 'å' or 'Á' or 'À' or 'Â' or 'Ä' or 'Ã' or 'Å' => 'a',
        'é' or 'è' or 'ê' or 'ë' or 'É' or 'È' or 'Ê' or 'Ë' => 'e',
        'í' or 'ì' or 'î' or 'ï' or 'Í' or 'Ì' or 'Î' or 'Ï' => 'i',
        'ó' or 'ò' or 'ô' or 'õ' or 'Ó' or 'Ò' or 'Ô' or 'Õ' => 'o',
        'ú' or 'ù' or 'û' or 'Ú' or 'Ù' or 'Û' => 'u',
        'ñ' or 'Ñ' => 'n',
        _ => c,
    };

    private static bool IsUpper(char c) => c is 'İ' or 'I' || char.IsUpper(c);

    private static Dictionary<char, int> BuildWeights()
    {
        var weights = new Dictionary<char, int>(Alphabet.Length);

        for (int i = 0; i < Alphabet.Length; i++)
            weights[Alphabet[i]] = i + 1;

        return weights;
    }
}

// Turkish casing and alphabetical order without ICU.
internal sealed class TurkishTextRules : ITextRules
{
    public static TurkishTextRules Instance { get; } = new();

    public StringComparer Comparer => TurkishCollation.Instance;

    public StringComparer IgnoreCaseComparer => TurkishCollation.IgnoreCase;

    public char ToUpper(char value) => value switch
    {
        'i' => 'İ',
        'ı' => 'I',
        _ => char.ToUpperInvariant(value),
    };

    public char ToLower(char value) => value switch
    {
        'I' => 'ı',
        'İ' => 'i',
        _ => char.ToLowerInvariant(value),
    };
}
