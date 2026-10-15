using System.Globalization;

namespace HeroKit.Turkish;

/// <summary>
/// Formats amounts as Turkish lira.
/// </summary>
public static class TurkishCurrencyExtensions
{
    // Fixed separators, so the output does not depend on the ICU version or the machine culture.
    private static readonly NumberFormatInfo TurkishNumbers = new()
    {
        NumberDecimalSeparator = ",",
        NumberGroupSeparator = ".",
        NegativeSign = "-",
    };

    /// <summary>
    /// Formats an amount as Turkish lira with two decimals.
    /// </summary>
    /// <param name="value">The amount. It is rounded to two decimals with <see cref="MidpointRounding.AwayFromZero"/>.</param>
    /// <param name="format">Symbol before ("₺1.234,50") or code after ("1.234,50 TL"). Defaults to <see cref="TurkishLiraFormat.Symbol"/>.</param>
    /// <returns>The formatted amount. Negative amounts start with "-".</returns>
    /// <example>
    /// <code>
    /// 1234.5m.ToTurkishLira();                         // "₺1.234,50"
    /// 1234.5m.ToTurkishLira(TurkishLiraFormat.Code);   // "1.234,50 TL"
    /// (-0.005m).ToTurkishLira();                       // "-₺0,01"
    /// </code>
    /// </example>
    public static string ToTurkishLira(this decimal value, TurkishLiraFormat format = TurkishLiraFormat.Symbol)
    {
        decimal rounded = Math.Round(value, 2, MidpointRounding.AwayFromZero);
        string sign = rounded < 0 ? "-" : string.Empty;
        string number = Math.Abs(rounded).ToString("N2", TurkishNumbers);

        return format == TurkishLiraFormat.Code ? $"{sign}{number} TL" : $"{sign}₺{number}";
    }
}
