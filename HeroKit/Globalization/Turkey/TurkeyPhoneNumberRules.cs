namespace HeroKit.Globalization.Turkey;

// Turkish phone numbers.
// All checks are structural (format and leading digit), not an operator or area-code allow-list.
// Spaces, dashes, dots and parentheses are ignored. Accepted prefixes are "+90", "0090", "90"
// (only when exactly 10 digits follow), "0", or none. The national number must have exactly 10 digits
// and must start with 2, 3 or 4 (landline) or 5 (mobile). Every method applies the same rules.
internal sealed class TurkeyPhoneNumberRules : IPhoneNumberRules
{
    public static TurkeyPhoneNumberRules Instance { get; } = new();

    private const int NationalLength = 10;

    public string? ToE164(string? value)
    {
        Span<char> national = stackalloc char[NationalLength];
        if (!TryGetNational(value, national))
        {
            return null;
        }

        return string.Concat("+90", national);
    }

    public string? Format(string? value)
    {
        Span<char> n = stackalloc char[NationalLength];
        if (!TryGetNational(value, n))
        {
            return null;
        }

        Span<char> result = stackalloc char[14];
        result[0] = '0';
        n[..3].CopyTo(result[1..]);
        result[4] = ' ';
        n.Slice(3, 3).CopyTo(result[5..]);
        result[8] = ' ';
        n.Slice(6, 2).CopyTo(result[9..]);
        result[11] = ' ';
        n.Slice(8, 2).CopyTo(result[12..]);
        return new string(result);
    }

    public bool IsValid(string? value)
    {
        Span<char> national = stackalloc char[NationalLength];
        return TryGetNational(value, national);
    }

    public bool IsMobile(string? value)
    {
        Span<char> national = stackalloc char[NationalLength];
        return TryGetNational(value, national) && national[0] == '5';
    }

    // Extracts the 10-digit national number (no leading 0) into destination. Structural checks only.
    private static bool TryGetNational(string? value, Span<char> destination)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        // Digits only, with a leading '+' allowed. Max 14 digits ("0090" + 10).
        Span<char> digits = stackalloc char[14];
        var count = 0;
        var plus = false;
        for (var i = 0; i < value.Length; i++)
        {
            var c = value[i];
            if (c is ' ' or '-' or '.' or '(' or ')')
            {
                continue;
            }

            if (c == '+' && count == 0 && !plus)
            {
                plus = true;
                continue;
            }

            if (c is < '0' or > '9' || count == digits.Length)
            {
                return false;
            }

            digits[count++] = c;
        }

        ReadOnlySpan<char> d = digits[..count];
        int start;
        if (plus)
        {
            if (!d.StartsWith("90"))
            {
                return false;
            }

            start = 2;
        }
        else if (d.StartsWith("0090"))
        {
            start = 4;
        }
        else if (d.Length == NationalLength + 2 && d.StartsWith("90"))
        {
            start = 2;
        }
        else
        {
            start = d.Length > 0 && d[0] == '0' ? 1 : 0;
        }

        // Landline area codes start with 2, 3 or 4; mobile numbers start with 5.
        if (d.Length - start != NationalLength || d[start] is < '2' or > '5')
        {
            return false;
        }

        d[start..].CopyTo(destination);
        return true;
    }
}
