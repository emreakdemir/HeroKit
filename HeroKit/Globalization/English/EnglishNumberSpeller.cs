using System.Text;

namespace HeroKit.Globalization.English;

// American English, short scale, no "and": 123 → "one hundred twenty-three", -5 → "minus five".
internal sealed class EnglishNumberSpeller : INumberSpeller
{
    public static EnglishNumberSpeller Instance { get; } = new();

    private static readonly string[] Ones =
    [
        "", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine",
        "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen",
    ];

    private static readonly string[] Tens = ["", "", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety"];

    private static readonly string[] Scales = ["", "thousand", "million", "billion", "trillion", "quadrillion", "quintillion"];

    public string Spell(long value)
    {
        if (value == 0)
            return "zero";

        var builder = new StringBuilder();
        if (value < 0)
            builder.Append("minus");

        // Unsigned magnitude avoids overflow for long.MinValue.
        ulong magnitude = value < 0 ? (ulong)(-(value + 1)) + 1 : (ulong)value;

        Span<int> groups = stackalloc int[Scales.Length];
        int count = 0;
        while (magnitude > 0)
        {
            groups[count++] = (int)(magnitude % 1000);
            magnitude /= 1000;
        }

        for (int i = count - 1; i >= 0; i--)
        {
            if (groups[i] == 0)
                continue;

            AppendGroup(builder, groups[i]);
            if (i > 0)
                Append(builder, Scales[i]);
        }

        return builder.ToString();
    }

    private static void AppendGroup(StringBuilder builder, int value)
    {
        if (value >= 100)
        {
            Append(builder, Ones[value / 100]);
            Append(builder, "hundred");
            value %= 100;
        }

        if (value == 0)
            return;

        if (value < 20)
            Append(builder, Ones[value]);
        else if (value % 10 == 0)
            Append(builder, Tens[value / 10]);
        else
            Append(builder, $"{Tens[value / 10]}-{Ones[value % 10]}");
    }

    private static void Append(StringBuilder builder, string word)
    {
        if (builder.Length > 0)
            builder.Append(' ');

        builder.Append(word);
    }
}
