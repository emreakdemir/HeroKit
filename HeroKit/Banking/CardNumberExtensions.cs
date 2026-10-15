namespace HeroKit.Banking;

/// <summary>
/// Contains extension methods for validating payment card numbers and detecting the card brand.
/// </summary>
/// <remarks>
/// Spaces and dashes are ignored. Any other non-digit character makes the number invalid.
/// The checks cover format and the Luhn checksum. They do not confirm that a card exists.
/// </remarks>
public static class CardNumberExtensions
{
    private const int MinLength = 12;
    private const int MaxLength = 19;

    /// <summary>
    /// Determines whether a string is a valid payment card number.
    /// </summary>
    /// <param name="value">The card number to check. Spaces and dashes are ignored.</param>
    /// <returns>true if the number has 12 to 19 digits and passes the Luhn check; otherwise, false. Null or empty input returns false.</returns>
    /// <example>
    /// <code>
    /// "4111 1111 1111 1111".IsValidCardNumber(); // true
    /// "4111111111111112".IsValidCardNumber();    // false
    /// </code>
    /// </example>
    public static bool IsValidCardNumber(this string? value)
    {
        Span<char> digits = stackalloc char[MaxLength];
        int length = Normalize(value, digits);
        if (length is < MinLength or > MaxLength)
            return false;

        int sum = 0;
        bool doubleIt = false;

        for (int i = length - 1; i >= 0; i--)
        {
            int digit = digits[i] - '0';

            if (doubleIt)
            {
                digit *= 2;
                if (digit > 9)
                    digit -= 9;
            }

            sum += digit;
            doubleIt = !doubleIt;
        }

        return sum % 10 == 0;
    }

    /// <summary>
    /// Detects the brand of a payment card from its prefix and length.
    /// </summary>
    /// <param name="value">The card number. Spaces and dashes are ignored.</param>
    /// <returns>The detected brand, or <see cref="CardBrand.Unknown"/> if no brand matches or the input is not made of digits.</returns>
    /// <remarks>
    /// The Luhn check is not required here. Use <see cref="IsValidCardNumber"/> for that.
    /// </remarks>
    /// <example>
    /// <code>
    /// "4111111111111111".GetCardBrand(); // CardBrand.Visa
    /// "378282246310005".GetCardBrand();   // CardBrand.AmericanExpress
    /// </code>
    /// </example>
    public static CardBrand GetCardBrand(this string? value)
    {
        Span<char> digits = stackalloc char[MaxLength];
        int length = Normalize(value, digits);
        if (length == 0 || length > MaxLength)
            return CardBrand.Unknown;

        int p2 = Prefix(digits, length, 2);
        int p3 = Prefix(digits, length, 3);
        int p4 = Prefix(digits, length, 4);

        if (length == 16 && p4 == 9792)
            return CardBrand.Troy;

        if (digits[0] == '4' && length is 13 or 16 or 19)
            return CardBrand.Visa;

        if (length == 16 && (p2 is >= 51 and <= 55 || p4 is >= 2221 and <= 2720))
            return CardBrand.Mastercard;

        if (length == 15 && p2 is 34 or 37)
            return CardBrand.AmericanExpress;

        if (length is >= 16 and <= 19)
        {
            if (p4 == 6011 || p3 is >= 644 and <= 649 || p2 == 65)
                return CardBrand.Discover;

            if (p4 is >= 3528 and <= 3589)
                return CardBrand.Jcb;
        }

        return CardBrand.Unknown;
    }

    // Copies the digits into the buffer. Returns 0 for null, empty or non-digit input.
    // Returns MaxLength + 1 when the number has too many digits.
    private static int Normalize(string? value, Span<char> digits)
    {
        if (string.IsNullOrEmpty(value))
            return 0;

        int length = 0;

        foreach (char c in value)
        {
            if (c is ' ' or '-')
                continue;

            if (c is < '0' or > '9')
                return 0;

            if (length == MaxLength)
                return MaxLength + 1;

            digits[length++] = c;
        }

        return length;
    }

    // Returns the first count digits as a number, or -1 if there are fewer digits.
    private static int Prefix(ReadOnlySpan<char> digits, int length, int count)
    {
        if (length < count)
            return -1;

        int result = 0;
        for (int i = 0; i < count; i++)
            result = result * 10 + (digits[i] - '0');

        return result;
    }
}
