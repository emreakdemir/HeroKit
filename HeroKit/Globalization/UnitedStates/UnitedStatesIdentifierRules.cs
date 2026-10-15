namespace HeroKit.Globalization.UnitedStates;

// ZIP code: "12345" or ZIP+4 "12345-6789". "00000" is not assigned.
internal sealed class UnitedStatesPostalCodeRules : IIdentifierRules
{
    public static UnitedStatesPostalCodeRules Instance { get; } = new();

    public bool IsValid(string? value)
    {
        if (value is null || (value.Length != 5 && value.Length != 10))
            return false;

        ReadOnlySpan<char> zip = value.AsSpan(0, 5);
        if (zip.ContainsAnyExceptInRange('0', '9') || zip.SequenceEqual("00000"))
            return false;

        return value.Length == 5 || (value[5] == '-' && !value.AsSpan(6).ContainsAnyExceptInRange('0', '9'));
    }
}

// Social Security number: "123-45-6789" or "123456789". The area is not 000, 666 or 900–999,
// the group is not 00 and the serial is not 0000. The check is structural; it does not prove that the number was issued.
internal sealed class UnitedStatesNationalIdRules : IIdentifierRules
{
    public static UnitedStatesNationalIdRules Instance { get; } = new();

    public bool IsValid(string? value) =>
        UnitedStatesTaxNumbers.TryGetSsnParts(value, out int area, out int group, out int serial)
        && area is > 0 and < 900 and not 666 && group > 0 && serial > 0;
}

// Tax identifier: SSN, ITIN ("9XX-7X-XXXX") or EIN ("12-3456789").
internal sealed class UnitedStatesTaxIdRules : IIdentifierRules
{
    public static UnitedStatesTaxIdRules Instance { get; } = new();

    public bool IsValid(string? value) =>
        UnitedStatesNationalIdRules.Instance.IsValid(value) || UnitedStatesTaxNumbers.IsItin(value) || UnitedStatesTaxNumbers.IsEin(value);
}

internal static class UnitedStatesTaxNumbers
{
    // EIN prefixes that the IRS has never assigned.
    private static readonly int[] UnassignedEinPrefixes = [0, 7, 8, 9, 17, 18, 19, 28, 29, 49, 69, 70, 78, 79, 89, 96, 97];

    public static bool TryGetSsnParts(string? value, out int area, out int group, out int serial)
    {
        area = group = serial = 0;
        Span<char> d = stackalloc char[9];
        if (!TryGetDigits(value, d, dashesAt: [3, 6]))
            return false;

        area = Number(d[..3]);
        group = Number(d.Slice(3, 2));
        serial = Number(d[5..]);
        return true;
    }

    // ITIN: area 900–999 and group 50–65, 70–88, 90–92 or 94–99.
    public static bool IsItin(string? value) =>
        TryGetSsnParts(value, out int area, out int group, out _)
        && area >= 900
        && group is (>= 50 and <= 65) or (>= 70 and <= 88) or (>= 90 and <= 92) or (>= 94 and <= 99);

    public static bool IsEin(string? value)
    {
        Span<char> d = stackalloc char[9];
        return TryGetDigits(value, d, dashesAt: [2]) && Array.IndexOf(UnassignedEinPrefixes, Number(d[..2])) < 0;
    }

    // Accepts 9 digits, or 9 digits with dashes exactly at the given positions of the formatted value.
    private static bool TryGetDigits(string? value, Span<char> destination, ReadOnlySpan<int> dashesAt)
    {
        if (value is null)
            return false;

        bool formatted = value.Length == 9 + dashesAt.Length;
        if (!formatted && value.Length != 9)
            return false;

        int count = 0;
        for (int i = 0; i < value.Length; i++)
        {
            char c = value[i];
            if (formatted && dashesAt.Contains(i))
            {
                if (c != '-')
                    return false;

                continue;
            }

            if (c is < '0' or > '9')
                return false;

            destination[count++] = c;
        }

        return count == 9;
    }

    private static int Number(ReadOnlySpan<char> digits)
    {
        int result = 0;
        foreach (char c in digits)
            result = result * 10 + (c - '0');

        return result;
    }
}
