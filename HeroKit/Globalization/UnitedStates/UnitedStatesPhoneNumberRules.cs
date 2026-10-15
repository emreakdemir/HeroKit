namespace HeroKit.Globalization.UnitedStates;

// North American Numbering Plan numbers for the United States.
// Spaces, dashes, dots and parentheses are ignored. Accepted prefixes are "+1", "001", "1" (only when exactly 10 digits follow) or none.
// The 10-digit number is NPA-NXX-XXXX: area code and exchange start with 2–9, and the area code is not an N11 service code.
// US numbers do not show whether a line is mobile, so IsMobile accepts every valid number.
internal sealed class UnitedStatesPhoneNumberRules : IPhoneNumberRules
{
    public static UnitedStatesPhoneNumberRules Instance { get; } = new();

    private const int NationalLength = 10;

    public bool IsValid(string? value)
    {
        Span<char> national = stackalloc char[NationalLength];
        return TryGetNational(value, national);
    }

    public bool IsMobile(string? value) => IsValid(value);

    public string? ToE164(string? value)
    {
        Span<char> national = stackalloc char[NationalLength];
        return TryGetNational(value, national) ? string.Concat("+1", national) : null;
    }

    public string? Format(string? value)
    {
        Span<char> n = stackalloc char[NationalLength];
        if (!TryGetNational(value, n))
            return null;

        // (212) 555-0123
        Span<char> result = stackalloc char[14];
        result[0] = '(';
        n[..3].CopyTo(result[1..]);
        result[4] = ')';
        result[5] = ' ';
        n.Slice(3, 3).CopyTo(result[6..]);
        result[9] = '-';
        n.Slice(6, 4).CopyTo(result[10..]);
        return new string(result);
    }

    private static bool TryGetNational(string? value, Span<char> destination)
    {
        if (string.IsNullOrEmpty(value))
            return false;

        // Digits only, with a leading '+' allowed. Max 13 digits ("001" + 10).
        Span<char> digits = stackalloc char[13];
        int count = 0;
        bool plus = false;
        foreach (char c in value)
        {
            if (c is ' ' or '-' or '.' or '(' or ')')
                continue;

            if (c == '+' && count == 0 && !plus)
            {
                plus = true;
                continue;
            }

            if (c is < '0' or > '9' || count == digits.Length)
                return false;

            digits[count++] = c;
        }

        ReadOnlySpan<char> d = digits[..count];
        int start;
        if (plus)
        {
            if (!d.StartsWith("1"))
                return false;

            start = 1;
        }
        else if (d.StartsWith("001"))
        {
            start = 3;
        }
        else
        {
            start = d.Length == NationalLength + 1 && d[0] == '1' ? 1 : 0;
        }

        if (d.Length - start != NationalLength)
            return false;

        ReadOnlySpan<char> n = d[start..];
        bool serviceCode = n[1] == '1' && n[2] == '1';
        if (n[0] is < '2' || n[3] is < '2' || serviceCode)
            return false;

        n.CopyTo(destination);
        return true;
    }
}
