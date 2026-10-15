using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace HeroKit.Globalization.Turkey;

/// <summary>
/// A Turkish e-Fatura or e-Arşiv invoice number in the GİB format: a 3-character series, a 4-digit year and a 9-digit sequence,
/// for example "ABC2026000000001".
/// </summary>
/// <param name="Series">The 3-character series (A–Z, 0–9).</param>
/// <param name="Year">The 4-digit year.</param>
/// <param name="Sequence">The sequence number within the series and year (1–999,999,999).</param>
public readonly record struct GibInvoiceNumber(string Series, int Year, int Sequence)
{
    /// <summary>
    /// The largest sequence number that fits in nine digits.
    /// </summary>
    public const int MaxSequence = 999_999_999;

    /// <summary>
    /// Creates an invoice number after validating its parts.
    /// </summary>
    /// <param name="series">The 3-character series (A–Z, 0–9).</param>
    /// <param name="year">The year (2000–2099).</param>
    /// <param name="sequence">The sequence number (1–999,999,999).</param>
    /// <returns>The invoice number.</returns>
    /// <exception cref="ArgumentException">A part is not valid.</exception>
    /// <example>
    /// <code>
    /// GibInvoiceNumber.Create("ABC", 2026, 1).ToString(); // "ABC2026000000001"
    /// </code>
    /// </example>
    public static GibInvoiceNumber Create(string series, int year, int sequence)
    {
        ArgumentNullException.ThrowIfNull(series);

        if (!IsValidSeries(series))
            throw new ArgumentException("The series must be 3 characters from A–Z and 0–9.", nameof(series));

        ArgumentOutOfRangeException.ThrowIfLessThan(year, 2000);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(year, 2099);
        ArgumentOutOfRangeException.ThrowIfLessThan(sequence, 1);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(sequence, MaxSequence);

        return new GibInvoiceNumber(series, year, sequence);
    }

    /// <summary>
    /// Tries to parse an invoice number in the GİB format.
    /// </summary>
    /// <param name="value">The invoice number, for example "ABC2026000000001". Must be upper case, without spaces.</param>
    /// <param name="result">The parsed invoice number when the method returns true.</param>
    /// <returns>True if the value is a valid invoice number; otherwise false.</returns>
    public static bool TryParse([NotNullWhen(true)] string? value, out GibInvoiceNumber result)
    {
        result = default;

        if (value is not { Length: 16 } || !IsValidSeries(value.AsSpan(0, 3)))
            return false;

        ReadOnlySpan<char> digits = value.AsSpan(3);
        if (digits.ContainsAnyExceptInRange('0', '9'))
            return false;

        int year = int.Parse(digits[..4], NumberStyles.None, CultureInfo.InvariantCulture);
        int sequence = int.Parse(digits[4..], NumberStyles.None, CultureInfo.InvariantCulture);

        if (year is < 2000 or > 2099 || sequence < 1)
            return false;

        result = new GibInvoiceNumber(value[..3], year, sequence);
        return true;
    }

    /// <summary>
    /// Returns the next invoice number in the same series and year.
    /// </summary>
    /// <returns>The invoice number with the sequence increased by one.</returns>
    /// <exception cref="InvalidOperationException">The sequence is already at <see cref="MaxSequence"/>.</exception>
    /// <example>
    /// <code>
    /// GibInvoiceNumber.Create("ABC", 2026, 41).Next().ToString(); // "ABC2026000000042"
    /// </code>
    /// </example>
    public GibInvoiceNumber Next() => Sequence < MaxSequence
        ? this with { Sequence = Sequence + 1 }
        : throw new InvalidOperationException($"The sequence of series {Series} for {Year} is exhausted.");

    /// <summary>
    /// Formats the invoice number as 16 characters, for example "ABC2026000000001".
    /// </summary>
    /// <returns>The formatted invoice number.</returns>
    public override string ToString() => string.Create(CultureInfo.InvariantCulture, $"{Series}{Year:D4}{Sequence:D9}");

    private static bool IsValidSeries(ReadOnlySpan<char> series)
    {
        if (series.Length != 3)
            return false;

        foreach (char c in series)
        {
            if (c is not ((>= 'A' and <= 'Z') or (>= '0' and <= '9')))
                return false;
        }

        return true;
    }
}
