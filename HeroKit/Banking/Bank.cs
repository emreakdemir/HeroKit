namespace HeroKit.Banking;

/// <summary>
/// A bank identified by its national bank code.
/// </summary>
/// <param name="Code">The national bank code as it appears in the IBAN, for example "00062".</param>
/// <param name="Name">The registered name, for example "Türkiye Garanti Bankası A.Ş.".</param>
public sealed record Bank(string Code, string Name);
