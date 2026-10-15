namespace HeroKit.Turkish;

/// <summary>
/// Provides validation for Turkish identity and tax numbers.
/// </summary>
/// <remarks>
/// The checks are checksum checks. They confirm that a number is well formed, not that it is issued to a person or company.
/// Input must contain digits only: spaces and separators are not accepted.
/// </remarks>
public static class TurkishIdentityExtensions
{
    /// <summary>
    /// Checks whether a string is a valid Turkish identity number (T.C. Kimlik No, TCKN).
    /// </summary>
    /// <param name="value">The number to check.</param>
    /// <returns>True if the value has 11 digits, does not start with 0 and both check digits are correct; otherwise false.</returns>
    /// <example>
    /// <code>
    /// "10000000146".IsValidTckn(); // true
    /// "10000000147".IsValidTckn(); // false
    /// </code>
    /// </example>
    public static bool IsValidTckn(this string? value)
    {
        if (value is not { Length: 11 } || value[0] == '0' || !IsAllDigits(value))
            return false;

        int oddSum = 0;
        int evenSum = 0;

        // Positions 1, 3, 5, 7, 9 (indexes 0, 2, 4, 6, 8) and positions 2, 4, 6, 8 (indexes 1, 3, 5, 7).
        for (int i = 0; i < 9; i++)
        {
            if (i % 2 == 0)
                oddSum += Digit(value, i);
            else
                evenSum += Digit(value, i);
        }

        int tenth = ((oddSum * 7 - evenSum) % 10 + 10) % 10;
        if (tenth != Digit(value, 9))
            return false;

        int eleventh = (oddSum + evenSum + tenth) % 10;
        return eleventh == Digit(value, 10);
    }

    /// <summary>
    /// Checks whether a string is a valid Turkish tax number (Vergi Kimlik No, VKN).
    /// </summary>
    /// <param name="value">The number to check.</param>
    /// <returns>True if the value has 10 digits and the check digit is correct; otherwise false.</returns>
    /// <example>
    /// <code>
    /// "4840847211".IsValidVkn(); // true
    /// "4840847212".IsValidVkn(); // false
    /// </code>
    /// </example>
    public static bool IsValidVkn(this string? value)
    {
        if (value is not { Length: 10 } || !IsAllDigits(value))
            return false;

        int sum = 0;

        for (int i = 0; i < 9; i++)
        {
            int shifted = (Digit(value, i) + 9 - i) % 10;
            int weighted = shifted * (1 << (9 - i)) % 9;

            if (shifted != 0 && weighted == 0)
                weighted = 9;

            sum += weighted;
        }

        int check = (10 - sum % 10) % 10;
        return check == Digit(value, 9);
    }

    /// <summary>
    /// Checks whether a string is a valid Turkish tax identifier: a VKN (10 digits) for companies or a TCKN (11 digits) for individuals.
    /// </summary>
    /// <param name="value">The number to check.</param>
    /// <returns>True if the value is a valid VKN or TCKN; otherwise false.</returns>
    public static bool IsValidTaxIdTr(this string? value) => value?.Length switch
    {
        10 => value.IsValidVkn(),
        11 => value.IsValidTckn(),
        _ => false,
    };

    private static bool IsAllDigits(ReadOnlySpan<char> value) => !value.ContainsAnyExceptInRange('0', '9');

    private static int Digit(string value, int index) => value[index] - '0';
}
