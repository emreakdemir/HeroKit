using HeroKit.Banking;

namespace HeroKit.Globalization;

/// <summary>
/// The banks of a country and how to find them from an IBAN.
/// </summary>
public interface IBankDirectory
{
    /// <summary>
    /// Gets all known banks.
    /// </summary>
    IReadOnlyList<Bank> All { get; }

    /// <summary>
    /// Finds the bank of an IBAN of this country.
    /// </summary>
    /// <param name="iban">A valid IBAN in electronic format: upper case, no spaces.</param>
    /// <returns>The bank, or null if the bank code is unknown.</returns>
    Bank? FindByIban(string iban);
}
