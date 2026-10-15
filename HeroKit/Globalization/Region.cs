namespace HeroKit.Globalization;

/// <summary>
/// A first-level administrative region of a country, such as a Turkish province.
/// </summary>
/// <param name="Code">The official region code, for example the Turkish plate code 34 for İstanbul or the FIPS code 6 for California.</param>
/// <param name="Name">The name of the region in the country's language.</param>
public readonly record struct Region(int Code, string Name)
{
    /// <summary>
    /// Gets the short letter code, for example "CA" for California, or null.
    /// </summary>
    public string? Abbreviation { get; init; }
}
