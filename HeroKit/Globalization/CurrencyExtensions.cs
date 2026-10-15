using System.Globalization;
using System.Text;

namespace HeroKit.Globalization;

/// <summary>
/// Formats amounts and writes them in words with the notation of a <see cref="Currency"/>.
/// </summary>
public static class CurrencyExtensions
{
    /// <summary>
    /// Formats an amount with two decimals in the notation of a currency.
    /// </summary>
    /// <param name="value">The amount. It is rounded to two decimals with <see cref="MidpointRounding.AwayFromZero"/>.</param>
    /// <param name="currency">The currency, for example <see cref="Currencies.TurkishLira"/>.</param>
    /// <param name="style">Symbol before ("₺1.234,50"), abbreviation after ("1.234,50 TL") or ISO code after ("1.234,50 TRY").</param>
    /// <returns>The formatted amount. Negative amounts start with "-".</returns>
    /// <example>
    /// <code>
    /// 1234.5m.ToCurrencyString(Currencies.TurkishLira);                            // "₺1.234,50"
    /// 1234.5m.ToCurrencyString(Currencies.TurkishLira, CurrencyStyle.Abbreviation); // "1.234,50 TL"
    /// </code>
    /// </example>
    public static string ToCurrencyString(this decimal value, Currency currency, CurrencyStyle style = CurrencyStyle.Symbol)
    {
        ArgumentNullException.ThrowIfNull(currency);

        var format = new NumberFormatInfo
        {
            NumberDecimalSeparator = currency.DecimalSeparator,
            NumberGroupSeparator = currency.GroupSeparator,
        };

        decimal rounded = Math.Round(value, 2, MidpointRounding.AwayFromZero);
        string sign = rounded < 0 ? "-" : string.Empty;
        string number = Math.Abs(rounded).ToString("N2", format);

        return style switch
        {
            CurrencyStyle.Abbreviation => $"{sign}{number} {currency.Abbreviation}",
            CurrencyStyle.IsoCode => $"{sign}{number} {currency.IsoCode}",
            _ => $"{sign}{currency.Symbol}{number}",
        };
    }

    /// <summary>
    /// Writes an amount in words with the unit and subunit names of a currency, as printed on invoices.
    /// </summary>
    /// <param name="amount">The amount. It is rounded to two decimals with <see cref="MidpointRounding.AwayFromZero"/>.</param>
    /// <param name="currency">The currency. Its <see cref="Currency.Language"/> spells the numbers.</param>
    /// <returns>
    /// The amount in words, for example "yüz yirmi üç Türk lirası elli kuruş". The subunit part is omitted when zero;
    /// the unit part is omitted when zero and the subunit is not.
    /// </returns>
    /// <example>
    /// <code>
    /// 123.50m.ToCurrencyWords(Currencies.TurkishLira); // "yüz yirmi üç Türk lirası elli kuruş"
    /// 123.50m.ToCurrencyWords(Currencies.USDollar);    // "one hundred twenty-three dollars and fifty cents"
    /// </code>
    /// </example>
    public static string ToCurrencyWords(this decimal amount, Currency currency)
    {
        ArgumentNullException.ThrowIfNull(currency);
        INumberSpeller speller = currency.Language.RequireNumberSpeller();

        decimal rounded = Math.Round(amount, 2, MidpointRounding.AwayFromZero);
        decimal magnitude = Math.Abs(rounded);
        long whole = (long)decimal.Truncate(magnitude);
        long cents = (long)((magnitude - whole) * 100);
        long sign = rounded < 0 ? -1 : 1;

        var builder = new StringBuilder();

        // The first spelled number carries the sign, so negative amounts use the language's word for minus.
        if (whole > 0 || cents == 0)
        {
            Append(builder, speller.Spell(sign * whole));
            Append(builder, whole == 1 ? currency.UnitName : currency.UnitNamePlural ?? currency.UnitName);

            if (cents > 0)
            {
                Append(builder, currency.SubunitConjunction);
                Append(builder, speller.Spell(cents));
                Append(builder, Subunit(currency, cents));
            }
        }
        else
        {
            Append(builder, speller.Spell(sign * cents));
            Append(builder, Subunit(currency, cents));
        }

        return builder.ToString();
    }

    /// <summary>
    /// Reads an amount written with the separators of a currency, with or without its symbol, abbreviation or ISO code.
    /// </summary>
    /// <remarks>
    /// Group separators must split the whole part into groups of three digits, so "1.234,56" is a Turkish lira amount
    /// but "1.5" is rejected instead of being read as 15. The method does not use the culture of the machine.
    /// </remarks>
    /// <param name="value">The text, for example "₺1.234,56", "1.234,56 TL", "-1234,5" or "$1,234.56".</param>
    /// <param name="currency">The currency whose separators, symbol, abbreviation and ISO code are accepted.</param>
    /// <param name="result">The amount when the method returns true; otherwise 0.</param>
    /// <returns>True if the text is a valid amount.</returns>
    /// <example>
    /// <code>
    /// "₺1.234,56".TryToAmount(Currencies.TurkishLira, out decimal amount); // true, 1234.56
    /// "1.234".TryToAmount(Currencies.TurkishLira, out _);                 // true, 1234
    /// "1.5".TryToAmount(Currencies.TurkishLira, out _);                   // false
    /// </code>
    /// </example>
    public static bool TryToAmount(this string? value, Currency currency, out decimal result)
    {
        ArgumentNullException.ThrowIfNull(currency);
        result = 0m;
        if (string.IsNullOrWhiteSpace(value))
            return false;

        string text = value.Trim();
        bool negative = false;

        // Sign and currency marker may come in either order at the start; a marker may also come at the end.
        for (int pass = 0; pass < 2; pass++)
        {
            if (text.Length > 0 && (text[0] == '-' || text[0] == '+'))
            {
                negative = text[0] == '-';
                text = text[1..].TrimStart();
            }

            text = StripMarker(text, currency, atStart: true);
        }

        text = StripMarker(text, currency, atStart: false);
        if (text.Length == 0)
            return false;

        int decimalIndex = text.LastIndexOf(currency.DecimalSeparator, StringComparison.Ordinal);
        string whole = decimalIndex >= 0 ? text[..decimalIndex] : text;
        string fraction = decimalIndex >= 0 ? text[(decimalIndex + currency.DecimalSeparator.Length)..] : string.Empty;

        if (decimalIndex >= 0 && (fraction.Length == 0 || !IsDigits(fraction)))
            return false;

        string[] groups = currency.GroupSeparator.Length > 0 ? whole.Split(currency.GroupSeparator) : [whole];
        if (groups[0].Length is 0 or > 3 && groups.Length > 1)
            return false;

        for (int i = 0; i < groups.Length; i++)
        {
            if (!IsDigits(groups[i]) || (i > 0 && groups[i].Length != 3) || groups[i].Length == 0)
                return false;
        }

        string invariant = string.Concat(groups) + (fraction.Length > 0 ? "." + fraction : string.Empty);
        if (!decimal.TryParse(invariant, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal parsed))
            return false;

        result = negative ? -parsed : parsed;
        return true;
    }

    /// <summary>
    /// Reads an amount written with the separators of a currency, or returns a default value. See <see cref="TryToAmount"/>.
    /// </summary>
    /// <param name="value">The text, for example "₺1.234,56".</param>
    /// <param name="currency">The currency whose separators, symbol, abbreviation and ISO code are accepted.</param>
    /// <param name="defaultValue">The value returned when the text is not a valid amount.</param>
    /// <returns>The amount, or <paramref name="defaultValue"/>.</returns>
    public static decimal ToAmount(this string? value, Currency currency, decimal defaultValue = 0m) =>
        value.TryToAmount(currency, out decimal result) ? result : defaultValue;

    private static string StripMarker(string text, Currency currency, bool atStart)
    {
        foreach (string marker in (ReadOnlySpan<string>)[currency.Symbol, currency.Abbreviation, currency.IsoCode])
        {
            if (marker.Length == 0)
                continue;

            if (atStart && text.StartsWith(marker, StringComparison.OrdinalIgnoreCase))
                return text[marker.Length..].TrimStart();

            if (!atStart && text.EndsWith(marker, StringComparison.OrdinalIgnoreCase))
                return text[..^marker.Length].TrimEnd();
        }

        return text;
    }

    private static bool IsDigits(string value)
    {
        foreach (char c in value)
        {
            if (c is < '0' or > '9')
                return false;
        }

        return true;
    }

    private static string Subunit(Currency currency, long cents) =>
        cents == 1 ? currency.SubunitName : currency.SubunitNamePlural ?? currency.SubunitName;

    private static void Append(StringBuilder builder, string word)
    {
        if (word.Length == 0)
            return;

        if (builder.Length > 0)
            builder.Append(' ');

        builder.Append(word);
    }
}
