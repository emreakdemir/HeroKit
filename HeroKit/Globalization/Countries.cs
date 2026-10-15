using System.Collections.Concurrent;
using HeroKit.Globalization.Turkey;
using HeroKit.Globalization.UnitedStates;

namespace HeroKit.Globalization;

/// <summary>
/// Predefined countries and a registry that resolves countries by ISO code, for example in validation attributes.
/// </summary>
public static class Countries
{
    private static readonly ConcurrentDictionary<string, Country> Registry = new(StringComparer.OrdinalIgnoreCase);

    static Countries()
    {
        Register(Turkey);
        Register(UnitedStates);
    }

    /// <summary>
    /// Gets Türkiye (TR): phone numbers, plates, postal codes, TCKN, VKN, public holidays, provinces and the Turkish lira.
    /// </summary>
    public static Country Turkey { get; } = TurkeyCountry.Create();

    /// <summary>
    /// Gets the United States (US): phone numbers, ZIP codes, SSN, tax IDs (SSN, ITIN, EIN), federal holidays, states and the US dollar.
    /// Vehicle plates differ by state and are not supported.
    /// </summary>
    public static Country UnitedStates { get; } = UnitedStatesCountry.Create();

    /// <summary>
    /// Gets all registered countries.
    /// </summary>
    public static IReadOnlyCollection<Country> All => [.. Registry.Values];

    /// <summary>
    /// Registers a country so it can be resolved by its ISO code. A country with the same code is replaced.
    /// </summary>
    /// <param name="country">The country.</param>
    public static void Register(Country country)
    {
        ArgumentNullException.ThrowIfNull(country);
        Registry[country.Code] = country;
    }

    /// <summary>
    /// Returns the registered country with an ISO 3166-1 alpha-2 code.
    /// </summary>
    /// <param name="code">The code, for example "TR". Case is ignored.</param>
    /// <returns>The country.</returns>
    /// <exception cref="KeyNotFoundException">No country is registered with the code.</exception>
    public static Country Get(string code) =>
        TryGet(code, out Country? country) ? country : throw new KeyNotFoundException($"No country is registered with the code '{code}'.");

    /// <summary>
    /// Tries to return the registered country with an ISO 3166-1 alpha-2 code.
    /// </summary>
    /// <param name="code">The code, for example "TR". Case is ignored.</param>
    /// <param name="country">The country when the method returns true.</param>
    /// <returns>True if a country is registered with the code.</returns>
    public static bool TryGet(string? code, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out Country? country)
    {
        country = null;
        return code is not null && Registry.TryGetValue(code, out country);
    }
}
