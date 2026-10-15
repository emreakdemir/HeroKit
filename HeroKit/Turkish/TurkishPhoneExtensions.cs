namespace HeroKit.Turkish;

/// <summary>
/// Contains extension methods for normalizing, validating and formatting Turkish phone numbers.
/// </summary>
/// <remarks>
/// All checks are structural (format and leading digit), not an operator or area-code allow-list.
/// Spaces, dashes, dots and parentheses are ignored. Accepted prefixes are "+90", "0090", "90"
/// (only when exactly 10 digits follow), "0", or none. The national number must have exactly 10 digits
/// and must not start with 0.
/// </remarks>
public static class TurkishPhoneExtensions
{
    private const int NationalLength = 10;

    /// <summary>
    /// Converts a Turkish phone number to E.164 format ("+90" followed by 10 digits).
    /// </summary>
    /// <param name="value">The phone number to convert.</param>
    /// <returns>The E.164 number, or null if the input is not a valid Turkish phone number.</returns>
    /// <remarks>The check is structural (format and prefix), not an operator/area-code allow-list.</remarks>
    /// <example>
    /// <code>
    /// "0532 123 45 67".ToE164Tr(); // "+905321234567"
    /// </code>
    /// </example>
    public static string? ToE164Tr(this string? value)
    {
        Span<char> national = stackalloc char[NationalLength];
        if (!TryGetNational(value, national))
        {
            return null;
        }

        return string.Create(3 + NationalLength, new string(national), static (span, n) =>
        {
            "+90".AsSpan().CopyTo(span);
            n.AsSpan().CopyTo(span[3..]);
        });
    }

    /// <summary>
    /// Formats a Turkish phone number as "0XXX XXX XX XX".
    /// </summary>
    /// <param name="value">The phone number to format.</param>
    /// <returns>The formatted number, or null if the input is not a valid Turkish phone number.</returns>
    /// <remarks>The check is structural (format and prefix), not an operator/area-code allow-list.</remarks>
    /// <example>
    /// <code>
    /// "+90 532 123 45 67".FormatPhoneTr(); // "0532 123 45 67"
    /// </code>
    /// </example>
    public static string? FormatPhoneTr(this string? value)
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

    /// <summary>
    /// Determines whether a string is a valid Turkish landline or mobile phone number.
    /// </summary>
    /// <param name="value">The phone number to check.</param>
    /// <returns>
    /// true if the national number is valid and starts with 2, 3, 4 (landline) or 5 (mobile); otherwise, false.
    /// Null or empty input returns false.
    /// </returns>
    /// <remarks>The check is structural (format and prefix), not an operator/area-code allow-list.</remarks>
    /// <example>
    /// <code>
    /// "(0212) 555 12 34".IsValidPhoneTr(); // true
    /// </code>
    /// </example>
    public static bool IsValidPhoneTr(this string? value)
    {
        Span<char> national = stackalloc char[NationalLength];
        return TryGetNational(value, national) && national[0] is >= '2' and <= '5';
    }

    /// <summary>
    /// Determines whether a string is a valid Turkish mobile phone number.
    /// </summary>
    /// <param name="value">The phone number to check.</param>
    /// <returns>true if the national number is valid and starts with 5; otherwise, false. Null or empty input returns false.</returns>
    /// <remarks>The check is structural (format and prefix), not an operator/area-code allow-list.</remarks>
    /// <example>
    /// <code>
    /// "0532 123 45 67".IsValidMobileTr(); // true
    /// "0212 555 12 34".IsValidMobileTr(); // false
    /// </code>
    /// </example>
    public static bool IsValidMobileTr(this string? value)
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

        if (d.Length - start != NationalLength || d[start] == '0')
        {
            return false;
        }

        d[start..].CopyTo(destination);
        return true;
    }
}
