namespace HeroKit.Globalization.Turkey;

// Civilian plates: province 01–81, 1–3 letters (no Q, W, X or Turkish letters), 2–4 digits not starting with 0.
// 1 letter + 4 digits, 2 letters + 3–4 digits, 3 letters + 2–3 digits. Spaces and dashes are ignored; output "34 ABC 123".
internal sealed class TurkeyVehiclePlateRules : IVehiclePlateRules
{
    private const int MaxProvinceCode = 81;

    public static TurkeyVehiclePlateRules Instance { get; } = new();

    public bool IsValid(string? value) => Format(value) is not null;

    public string? Format(string? value)
    {
        if (string.IsNullOrEmpty(value))
            return null;

        Span<char> buffer = stackalloc char[16];
        int length = 0;

        foreach (char raw in value)
        {
            if (raw is ' ' or '-')
                continue;

            if (length == buffer.Length)
                return null;

            buffer[length++] = raw is >= 'a' and <= 'z' ? (char)(raw - 32) : raw;
        }

        if (length < 5 || !IsDigit(buffer[0]) || !IsDigit(buffer[1]))
            return null;

        int province = (buffer[0] - '0') * 10 + (buffer[1] - '0');
        if (province is < 1 or > MaxProvinceCode)
            return null;

        int letterEnd = 2;
        while (letterEnd < length && IsPlateLetter(buffer[letterEnd]))
            letterEnd++;

        int letters = letterEnd - 2;
        int digits = length - letterEnd;
        if (letters is < 1 or > 3 || digits == 0 || buffer[letterEnd] == '0')
            return null;

        for (int i = letterEnd; i < length; i++)
        {
            if (!IsDigit(buffer[i]))
                return null;
        }

        bool shapeOk = letters switch
        {
            1 => digits == 4,
            2 => digits is 3 or 4,
            _ => digits is 2 or 3,
        };

        if (!shapeOk)
            return null;

        return new string(buffer.Slice(0, 2)) + " " + new string(buffer.Slice(2, letters)) + " " + new string(buffer.Slice(letterEnd, digits));
    }

    private static bool IsDigit(char c) => c is >= '0' and <= '9';

    private static bool IsPlateLetter(char c) => c is >= 'A' and <= 'Z' and not ('Q' or 'W' or 'X');
}
