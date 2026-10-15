namespace HeroKit.Turkish;

/// <summary>
/// Provides helpers for Turkish provinces, vehicle plates and postal codes.
/// </summary>
/// <remarks>
/// The checks are format checks. They confirm that a value is well formed, not that it is issued.
/// </remarks>
public static class TurkishLocationExtensions
{
    private const int MaxProvinceCode = 81;

    private static readonly Dictionary<string, int> CodesByName = BuildCodesByName();

    /// <summary>
    /// Gets the province name for a plate code.
    /// </summary>
    /// <param name="code">The province code from 1 to 81.</param>
    /// <returns>The province name, or null if the code is outside 1 to 81.</returns>
    /// <example>
    /// <code>
    /// 34.ToProvinceName(); // "İstanbul"
    /// 82.ToProvinceName(); // null
    /// </code>
    /// </example>
    public static string? ToProvinceName(this int code) =>
        code is >= 1 and <= MaxProvinceCode ? TurkishProvinces.All[code - 1].Name : null;

    /// <summary>
    /// Gets the province name for a plate code given as text.
    /// </summary>
    /// <param name="code">The code as digits, for example "34", "06" or "6". Surrounding white space is ignored.</param>
    /// <returns>The province name, or null if the text is not a code from 1 to 81.</returns>
    /// <example>
    /// <code>
    /// "06".ToProvinceName(); // "Ankara"
    /// "abc".ToProvinceName(); // null
    /// </code>
    /// </example>
    public static string? ToProvinceName(this string? code)
    {
        if (code is null)
            return null;

        string trimmed = code.Trim();
        if (trimmed.Length is 0 or > 2 || !IsAllDigits(trimmed))
            return null;

        int number = 0;
        foreach (char c in trimmed)
            number = number * 10 + (c - '0');

        return number.ToProvinceName();
    }

    /// <summary>
    /// Gets the plate code for a province name.
    /// </summary>
    /// <param name="name">The province name. Case and Turkish letters are ignored. Surrounding white space is ignored.</param>
    /// <returns>The code from 1 to 81, or null if no province matches.</returns>
    /// <example>
    /// <code>
    /// "İSTANBUL".ToProvinceCode(); // 34
    /// "sanliurfa".ToProvinceCode(); // 63
    /// </code>
    /// </example>
    public static int? ToProvinceCode(this string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return null;

        return CodesByName.TryGetValue(Normalize(name.Trim()), out int code) ? code : null;
    }

    /// <summary>
    /// Checks whether a string is a valid Turkish civilian vehicle plate.
    /// </summary>
    /// <param name="value">The plate. Spaces, dashes and lower case are accepted.</param>
    /// <returns>
    /// True if the plate has a province code from 01 to 81, then one to three letters (A-Z without Q, W, X),
    /// then a digit group that does not start with 0; otherwise false.
    /// The allowed shapes are 1 letter and 4 digits, 2 letters and 3 or 4 digits, 3 letters and 2 or 3 digits.
    /// </returns>
    /// <example>
    /// <code>
    /// "34 ABC 123".IsValidPlateTr(); // true
    /// "34 Q 1234".IsValidPlateTr(); // false
    /// </code>
    /// </example>
    public static bool IsValidPlateTr(this string? value) => FormatPlateTr(value) is not null;

    /// <summary>
    /// Formats a Turkish civilian vehicle plate with single spaces.
    /// </summary>
    /// <param name="value">The plate. Spaces, dashes and lower case are accepted.</param>
    /// <returns>The plate in the form "34 ABC 123", or null if the plate is not valid.</returns>
    /// <example>
    /// <code>
    /// "34abc123".FormatPlateTr(); // "34 ABC 123"
    /// "35-AB-123".FormatPlateTr(); // "35 AB 123"
    /// </code>
    /// </example>
    public static string? FormatPlateTr(this string? value)
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

    /// <summary>
    /// Checks whether a string is a valid Turkish postal code.
    /// </summary>
    /// <param name="value">The postal code to check.</param>
    /// <returns>True if the value has exactly 5 digits and the first two are 01 to 81; otherwise false.</returns>
    /// <example>
    /// <code>
    /// "34710".IsValidPostalCodeTr(); // true
    /// "82000".IsValidPostalCodeTr(); // false
    /// </code>
    /// </example>
    public static bool IsValidPostalCodeTr(this string? value)
    {
        if (value is not { Length: 5 } || !IsAllDigits(value))
            return false;

        int province = (value[0] - '0') * 10 + (value[1] - '0');
        return province is >= 1 and <= MaxProvinceCode;
    }

    private static bool IsDigit(char c) => c is >= '0' and <= '9';

    private static bool IsAllDigits(string value)
    {
        foreach (char c in value)
        {
            if (!IsDigit(c))
                return false;
        }

        return true;
    }

    private static bool IsPlateLetter(char c) => c is >= 'A' and <= 'Z' and not ('Q' or 'W' or 'X');

    private static Dictionary<string, int> BuildCodesByName()
    {
        Dictionary<string, int> map = new(TurkishProvinces.All.Count);
        foreach (Province province in TurkishProvinces.All)
            map[Normalize(province.Name)] = province.Code;

        return map;
    }

    // Lower-cases with Turkish rules (I and İ both become i) and drops Turkish diacritics.
    // This does not depend on the current culture.
    private static string Normalize(string value) =>
        string.Create(value.Length, value, static (span, source) =>
        {
            for (int i = 0; i < source.Length; i++)
            {
                char c = source[i];
                span[i] = c switch
                {
                    'İ' or 'I' or 'ı' => 'i',
                    'Ç' or 'ç' => 'c',
                    'Ğ' or 'ğ' => 'g',
                    'Ö' or 'ö' => 'o',
                    'Ş' or 'ş' => 's',
                    'Ü' or 'ü' => 'u',
                    >= 'A' and <= 'Z' => (char)(c + 32),
                    _ => char.ToLowerInvariant(c),
                };
            }
        });
}
