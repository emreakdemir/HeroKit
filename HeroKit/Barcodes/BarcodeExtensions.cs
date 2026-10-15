namespace HeroKit.Barcodes;

/// <summary>
/// Validation helpers for GS1 product barcodes.
/// </summary>
public static class BarcodeExtensions
{
    /// <summary>
    /// Checks whether a string is a valid GTIN: an EAN-8, UPC-A (12), EAN-13 or GTIN-14 barcode number with a correct GS1 check digit.
    /// </summary>
    /// <remarks>
    /// The first digits of an EAN-13 are the GS1 prefix of the issuing organization, not the country of origin. GS1 Türkiye uses 868 and 869.
    /// </remarks>
    /// <param name="value">The barcode number, digits only.</param>
    /// <returns>True if the value has 8, 12, 13 or 14 digits and the check digit is correct; otherwise false.</returns>
    /// <example>
    /// <code>
    /// "8691234567890".IsValidGtin(); // true
    /// "8691234567891".IsValidGtin(); // false
    /// </code>
    /// </example>
    public static bool IsValidGtin(this string? value)
    {
        if (value is not { Length: 8 or 12 or 13 or 14 })
            return false;

        int sum = 0;
        for (int i = 0; i < value.Length - 1; i++)
        {
            char c = value[i];
            if (c is < '0' or > '9')
                return false;

            // Weights 3 and 1 alternate from the digit next to the check digit.
            int position = value.Length - 1 - i;
            sum += (c - '0') * (position % 2 == 1 ? 3 : 1);
        }

        char check = value[^1];
        return check is >= '0' and <= '9' && (10 - sum % 10) % 10 == check - '0';
    }
}
