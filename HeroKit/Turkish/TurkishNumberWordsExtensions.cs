using System.Text;

namespace HeroKit.Turkish;

/// <summary>
/// Writes numbers and amounts in Turkish words, as printed on invoices and receipts.
/// </summary>
public static class TurkishNumberWordsExtensions
{
    private static readonly string[] Ones = ["", "bir", "iki", "üç", "dört", "beş", "altı", "yedi", "sekiz", "dokuz"];
    private static readonly string[] Tens = ["", "on", "yirmi", "otuz", "kırk", "elli", "altmış", "yetmiş", "seksen", "doksan"];
    private static readonly string[] Scales = ["", "bin", "milyon", "milyar", "trilyon", "katrilyon", "kentilyon"];

    /// <summary>
    /// Writes an integer in Turkish words.
    /// </summary>
    /// <param name="value">The number to write.</param>
    /// <returns>The number in lowercase Turkish words separated by spaces, for example "bin iki yüz otuz dört".</returns>
    /// <example>
    /// <code>
    /// 123.ToTurkishWords();   // "yüz yirmi üç"
    /// 1000.ToTurkishWords();  // "bin"
    /// (-5).ToTurkishWords();  // "eksi beş"
    /// </code>
    /// </example>
    public static string ToTurkishWords(this int value) => ((long)value).ToTurkishWords();

    /// <summary>
    /// Writes an integer in Turkish words.
    /// </summary>
    /// <param name="value">The number to write.</param>
    /// <returns>The number in lowercase Turkish words separated by spaces, for example "bir milyon bin".</returns>
    public static string ToTurkishWords(this long value)
    {
        if (value == 0)
            return "sıfır";

        var builder = new StringBuilder();

        if (value < 0)
            builder.Append("eksi");

        // Unsigned magnitude avoids overflow for long.MinValue.
        AppendWords(builder, value < 0 ? (ulong)(-(value + 1)) + 1 : (ulong)value);

        return builder.ToString();
    }

    /// <summary>
    /// Writes an amount of money in Turkish words with its main unit and subunit.
    /// </summary>
    /// <param name="amount">The amount to write. It is rounded to two decimals with <see cref="MidpointRounding.AwayFromZero"/>.</param>
    /// <param name="unit">The name of the main unit. Defaults to "Türk lirası".</param>
    /// <param name="subunit">The name of the subunit (1/100 of the unit). Defaults to "kuruş".</param>
    /// <returns>
    /// The amount in words, for example "yüz yirmi üç Türk lirası elli kuruş".
    /// The subunit part is omitted when it is zero; the unit part is omitted when it is zero and the subunit is not.
    /// </returns>
    /// <example>
    /// <code>
    /// 123.50m.ToTurkishCurrencyWords();                 // "yüz yirmi üç Türk lirası elli kuruş"
    /// 0.75m.ToTurkishCurrencyWords();                   // "yetmiş beş kuruş"
    /// 10m.ToTurkishCurrencyWords("Euro", "sent");       // "on Euro"
    /// </code>
    /// </example>
    public static string ToTurkishCurrencyWords(this decimal amount, string unit = "Türk lirası", string subunit = "kuruş")
    {
        decimal rounded = Math.Round(amount, 2, MidpointRounding.AwayFromZero);
        decimal magnitude = Math.Abs(rounded);
        long whole = (long)decimal.Truncate(magnitude);
        long cents = (long)((magnitude - whole) * 100);

        var builder = new StringBuilder();

        if (rounded < 0)
            builder.Append("eksi");

        if (whole > 0 || cents == 0)
        {
            Append(builder, whole == 0 ? "sıfır" : whole.ToTurkishWords());
            Append(builder, unit);
        }

        if (cents > 0)
        {
            Append(builder, cents.ToTurkishWords());
            Append(builder, subunit);
        }

        return builder.ToString();
    }

    private static void AppendWords(StringBuilder builder, ulong value)
    {
        // Split into groups of three digits, most significant first.
        Span<int> groups = stackalloc int[Scales.Length];
        int count = 0;

        for (ulong rest = value; rest > 0; rest /= 1000)
            groups[count++] = (int)(rest % 1000);

        for (int scale = count - 1; scale >= 0; scale--)
        {
            int group = groups[scale];
            if (group == 0)
                continue;

            // "bin", not "bir bin". Larger scales keep "bir": "bir milyon".
            if (!(scale == 1 && group == 1))
                AppendGroup(builder, group);

            Append(builder, Scales[scale]);
        }
    }

    private static void AppendGroup(StringBuilder builder, int group)
    {
        int hundreds = group / 100;
        int tens = group / 10 % 10;
        int ones = group % 10;

        if (hundreds > 0)
        {
            // "yüz", not "bir yüz".
            if (hundreds > 1)
                Append(builder, Ones[hundreds]);

            Append(builder, "yüz");
        }

        Append(builder, Tens[tens]);
        Append(builder, Ones[ones]);
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
