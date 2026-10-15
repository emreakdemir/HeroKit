using System.Text;

namespace HeroKit.Globalization.Turkey;

// Writes numbers and amounts in Turkish words, as printed on invoices and receipts.
internal sealed class TurkishNumberSpeller : INumberSpeller
{
    public static TurkishNumberSpeller Instance { get; } = new();

    private static readonly string[] Ones = ["", "bir", "iki", "üç", "dört", "beş", "altı", "yedi", "sekiz", "dokuz"];
    private static readonly string[] Tens = ["", "on", "yirmi", "otuz", "kırk", "elli", "altmış", "yetmiş", "seksen", "doksan"];
    private static readonly string[] Scales = ["", "bin", "milyon", "milyar", "trilyon", "katrilyon", "kentilyon"];

    public string Spell(long value)
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
