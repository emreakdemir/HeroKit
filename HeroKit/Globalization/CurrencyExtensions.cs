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
            Append(builder, currency.UnitName);

            if (cents > 0)
            {
                Append(builder, speller.Spell(cents));
                Append(builder, currency.SubunitName);
            }
        }
        else
        {
            Append(builder, speller.Spell(sign * cents));
            Append(builder, currency.SubunitName);
        }

        return builder.ToString();
    }

    private static void Append(StringBuilder builder, string word)
    {
        if (word.Length == 0)
            return;

        if (builder.Length > 0)
            builder.Append(' ');

        builder.Append(word);
    }
}
