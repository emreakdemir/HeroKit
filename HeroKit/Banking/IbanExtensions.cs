using HeroKit.Globalization;

namespace HeroKit.Banking;

/// <summary>
/// Contains extension methods for validating and formatting IBANs (ISO 13616).
/// </summary>
public static class IbanExtensions
{
    private const int MinLength = 15;
    private const int MaxLength = 34;

    /// <summary>
    /// Determines whether a string is a valid IBAN (ISO 13616).
    /// Spaces are ignored and lowercase letters are accepted.
    /// </summary>
    /// <param name="value">The string to check.</param>
    /// <returns>true if the string is a valid IBAN; otherwise, false. Null or empty input returns false.</returns>
    /// <remarks>
    /// The check covers structure (country letters, check digits, alphanumeric BBAN, length 15 to 34)
    /// and the mod-97 checksum. For Turkey ("TR") the length must be 26 and the reserved fifth character must be '0'.
    /// </remarks>
    /// <example>
    /// <code>
    /// "TR33 0006 1005 1978 6457 8413 26".IsValidIban(); // true
    /// "TR330006100519786457841327".IsValidIban();       // false
    /// </code>
    /// </example>
    public static bool IsValidIban(this string? value)
    {
        Span<char> buffer = stackalloc char[MaxLength];
        return TryNormalize(value, buffer, out _);
    }

    /// <summary>
    /// Formats a valid IBAN in uppercase blocks of four characters separated by single spaces.
    /// </summary>
    /// <param name="value">The IBAN to format. Spaces are ignored and lowercase letters are accepted.</param>
    /// <returns>The formatted IBAN, or null if the input is not a valid IBAN.</returns>
    /// <example>
    /// <code>
    /// "tr330006100519786457841326".FormatIban(); // "TR33 0006 1005 1978 6457 8413 26"
    /// </code>
    /// </example>
    public static string? FormatIban(this string? value)
    {
        Span<char> buffer = stackalloc char[MaxLength];
        if (!TryNormalize(value, buffer, out var length))
        {
            return null;
        }

        var source = buffer[..length];
        var groups = (length + 3) / 4;
        Span<char> result = stackalloc char[MaxLength + MaxLength / 4];
        var position = 0;
        for (var i = 0; i < groups; i++)
        {
            if (i > 0)
            {
                result[position++] = ' ';
            }

            var chunk = source.Slice(i * 4, Math.Min(4, length - (i * 4)));
            chunk.CopyTo(result[position..]);
            position += chunk.Length;
        }

        return new string(result[..position]);
    }

    /// <summary>
    /// Normalizes a valid IBAN to uppercase without spaces.
    /// </summary>
    /// <param name="value">The IBAN to normalize. Spaces are ignored and lowercase letters are accepted.</param>
    /// <returns>The normalized IBAN, or null if the input is not a valid IBAN.</returns>
    /// <example>
    /// <code>
    /// "tr33 0006 1005 1978 6457 8413 26".NormalizeIban(); // "TR330006100519786457841326"
    /// </code>
    /// </example>
    public static string? NormalizeIban(this string? value)
    {
        Span<char> buffer = stackalloc char[MaxLength];
        return TryNormalize(value, buffer, out var length) ? new string(buffer[..length]) : null;
    }

    private static bool TryNormalize(string? value, Span<char> buffer, out int length)
    {
        length = 0;
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        foreach (var c in value)
        {
            if (c == ' ')
            {
                continue;
            }

            if (length == MaxLength)
            {
                return false;
            }

            buffer[length++] = c is >= 'a' and <= 'z' ? (char)(c - 32) : c;
        }

        if (length < MinLength)
        {
            return false;
        }

        var iban = buffer[..length];
        if (!IsUpperLetter(iban[0]) || !IsUpperLetter(iban[1]) || !IsDigit(iban[2]) || !IsDigit(iban[3]))
        {
            return false;
        }

        for (var i = 4; i < iban.Length; i++)
        {
            if (!IsDigit(iban[i]) && !IsUpperLetter(iban[i]))
            {
                return false;
            }
        }

        // National rules (length, fixed positions) come from the registered country, so new countries need no change here.
        if (Countries.TryGet(new string(iban[..2]), out Country? country) && country.IbanRules is { } rules && !rules.IsValid(new string(iban)))
        {
            return false;
        }

        return ComputeMod97(iban) == 1;
    }

    private static int ComputeMod97(ReadOnlySpan<char> iban)
    {
        var remainder = 0;
        for (var i = 0; i < iban.Length; i++)
        {
            // Rotate: first four characters are processed last.
            var c = iban[(i + 4) % iban.Length];
            remainder = IsDigit(c)
                ? ((remainder * 10) + (c - '0')) % 97
                : ((remainder * 100) + (c - 'A' + 10)) % 97;
        }

        return remainder;
    }

    private static bool IsDigit(char c) => c is >= '0' and <= '9';

    private static bool IsUpperLetter(char c) => c is >= 'A' and <= 'Z';
}
