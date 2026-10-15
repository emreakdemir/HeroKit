namespace HeroKit.Globalization.Turkey;

// National identity number: TCKN.
internal sealed class TurkeyNationalIdRules : IIdentifierRules
{
    public static TurkeyNationalIdRules Instance { get; } = new();

    public bool IsValid(string? value) => value.IsValidTckn();
}

// Tax identifier: VKN (10 digits, companies) or TCKN (11 digits, individuals).
internal sealed class TurkeyTaxIdRules : IIdentifierRules
{
    public static TurkeyTaxIdRules Instance { get; } = new();

    public bool IsValid(string? value) => value?.Length switch
    {
        10 => value.IsValidVkn(),
        11 => value.IsValidTckn(),
        _ => false,
    };
}
