namespace HeroKit.Globalization;

/// <summary>
/// A country and its rules. Predefined countries are in <see cref="Countries"/>.
/// </summary>
/// <remarks>
/// Every rule is optional. Methods that need a rule the country does not have throw <see cref="NotSupportedException"/>.
/// To support another country, implement the rule interfaces, create an instance and register it with <see cref="Countries.Register"/>.
/// </remarks>
/// <param name="code">The ISO 3166-1 alpha-2 code, for example "TR".</param>
/// <param name="name">The English name, for example "Türkiye".</param>
/// <param name="language">The primary language, used for region names.</param>
public sealed class Country(string code, string name, Language language)
{
    /// <summary>Gets the ISO 3166-1 alpha-2 code.</summary>
    public string Code { get; } = code;

    /// <summary>Gets the English name.</summary>
    public string Name { get; } = name;

    /// <summary>Gets the primary language.</summary>
    public Language Language { get; } = language;

    /// <summary>Gets the national currency, or null.</summary>
    public Currency? Currency { get; init; }

    /// <summary>Gets the phone number rules, or null.</summary>
    public IPhoneNumberRules? PhoneNumbers { get; init; }

    /// <summary>Gets the vehicle plate rules, or null.</summary>
    public IVehiclePlateRules? VehiclePlates { get; init; }

    /// <summary>Gets the postal code rules, or null.</summary>
    public IIdentifierRules? PostalCodes { get; init; }

    /// <summary>Gets the national identity number rules, or null.</summary>
    public IIdentifierRules? NationalIds { get; init; }

    /// <summary>Gets the tax identifier rules, or null.</summary>
    public IIdentifierRules? TaxIds { get; init; }

    /// <summary>Gets the public holiday calendar, or null.</summary>
    public IHolidayCalendar? Holidays { get; init; }

    /// <summary>
    /// Gets the national IBAN rules (length and fixed positions), or null. IBAN validation applies them to IBANs with this country code.
    /// </summary>
    public IIdentifierRules? IbanRules { get; init; }

    /// <summary>Gets the first-level administrative regions, for example the 81 Turkish provinces.</summary>
    public IReadOnlyList<Region> Regions { get; init; } = [];

    /// <summary>
    /// Finds a region by its code.
    /// </summary>
    /// <param name="code">The region code.</param>
    /// <returns>The region, or null.</returns>
    public Region? FindRegion(int code)
    {
        foreach (Region region in Regions)
        {
            if (region.Code == code)
                return region;
        }

        return null;
    }

    /// <summary>
    /// Finds a region by its code ("34", "06"), its abbreviation ("CA") or its name, ignoring case and diacritics ("istanbul", "Sanliurfa").
    /// </summary>
    /// <param name="codeOrName">The code or the name.</param>
    /// <returns>The region, or null.</returns>
    /// <example>
    /// <code>
    /// Countries.Turkey.FindRegion("34")?.Name;        // "İstanbul"
    /// Countries.Turkey.FindRegion("sanliurfa")?.Code; // 63
    /// Countries.UnitedStates.FindRegion("ca")?.Name;   // "California"
    /// </code>
    /// </example>
    public Region? FindRegion(string? codeOrName)
    {
        if (string.IsNullOrWhiteSpace(codeOrName))
            return null;

        string trimmed = codeOrName.Trim();

        if (int.TryParse(trimmed, System.Globalization.NumberStyles.None, System.Globalization.CultureInfo.InvariantCulture, out int code))
            return FindRegion(code);

        foreach (Region region in Regions)
        {
            if (region.Abbreviation is not null && string.Equals(region.Abbreviation, trimmed, StringComparison.OrdinalIgnoreCase))
                return region;
        }

        string key = TextFolding.Fold(trimmed, Language.TextRules);

        foreach (Region region in Regions)
        {
            if (TextFolding.Fold(region.Name, Language.TextRules) == key)
                return region;
        }

        return null;
    }

    /// <inheritdoc />
    public override string ToString() => Name;

    internal T Require<T>(T? rules, string feature) where T : class =>
        rules ?? throw new NotSupportedException($"{Name} has no {feature} rules.");
}
