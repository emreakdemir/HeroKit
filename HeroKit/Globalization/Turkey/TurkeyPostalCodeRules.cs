namespace HeroKit.Globalization.Turkey;

// Five digits; the first two are a province code (01–81).
internal sealed class TurkeyPostalCodeRules : IIdentifierRules
{
    public static TurkeyPostalCodeRules Instance { get; } = new();

    public bool IsValid(string? value)
    {
        if (value is not { Length: 5 } || value.AsSpan().ContainsAnyExceptInRange('0', '9'))
            return false;

        int province = (value[0] - '0') * 10 + (value[1] - '0');
        return province is >= 1 and <= 81;
    }
}
