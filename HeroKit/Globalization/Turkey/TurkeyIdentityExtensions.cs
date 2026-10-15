namespace HeroKit.Globalization.Turkey;

/// <summary>
/// Provides validation for Turkish identity and tax numbers.
/// </summary>
/// <remarks>
/// The checks are checksum checks. They confirm that a number is well formed, not that it is issued to a person or company.
/// Input must contain digits only: spaces and separators are not accepted.
/// </remarks>
public static class TurkeyIdentityExtensions
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

    private static bool IsAllDigits(ReadOnlySpan<char> value) => !value.ContainsAnyExceptInRange('0', '9');

    private static int Digit(string value, int index) => value[index] - '0';

    /// <summary>
    /// Checks whether a string is a valid MERSİS number (Central Registry Record System number of a Turkish company).
    /// </summary>
    /// <remarks>
    /// A MERSİS number has 16 digits: "0", the 10-digit VKN of the company and a 5-digit serial. The check is structural:
    /// it verifies the embedded VKN, not that the number is registered.
    /// </remarks>
    /// <param name="value">The number to check, for example "0879001756600379".</param>
    /// <returns>True if the value has 16 digits, starts with 0 and contains a valid VKN; otherwise false.</returns>
    /// <example>
    /// <code>
    /// "0879001756600379".IsValidMersis(); // true
    /// "0879001756700379".IsValidMersis(); // false: the embedded VKN is invalid
    /// </code>
    /// </example>
    public static bool IsValidMersis(this string? value) =>
        value is { Length: 16 } && value[0] == '0' && IsAllDigits(value) && value.Substring(1, 10).IsValidVkn();
}
