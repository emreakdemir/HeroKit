namespace HeroKit.Globalization.Turkey;

// Turkish IBANs: 26 characters; the 5th character (reserved) is always 0. Input is the normalized IBAN.
internal sealed class TurkeyIbanRules : IIdentifierRules
{
    public static TurkeyIbanRules Instance { get; } = new();

    public bool IsValid(string? value) => value is { Length: 26 } && value[4] == '0';
}
