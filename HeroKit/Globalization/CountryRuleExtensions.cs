namespace HeroKit.Globalization;

/// <summary>
/// Validates and formats country-specific values with the rules of a <see cref="Country"/>.
/// </summary>
/// <remarks>
/// All methods throw <see cref="NotSupportedException"/> when the country has no rules for the value type.
/// </remarks>
public static class CountryRuleExtensions
{
    /// <summary>
    /// Checks whether a value is a valid phone number of a country.
    /// </summary>
    /// <param name="value">The phone number in any common notation.</param>
    /// <param name="country">The country, for example <see cref="Countries.Turkey"/>.</param>
    /// <returns>True if valid; otherwise false.</returns>
    /// <example>
    /// <code>
    /// "0532 123 45 67".IsValidPhone(Countries.Turkey); // true
    /// </code>
    /// </example>
    public static bool IsValidPhone(this string? value, Country country) => Phone(country).IsValid(value);

    /// <summary>
    /// Checks whether a value is a valid mobile phone number of a country.
    /// </summary>
    /// <param name="value">The phone number in any common notation.</param>
    /// <param name="country">The country.</param>
    /// <returns>True if valid and mobile; otherwise false.</returns>
    public static bool IsValidMobilePhone(this string? value, Country country) => Phone(country).IsMobile(value);

    /// <summary>
    /// Converts a phone number of a country to E.164.
    /// </summary>
    /// <param name="value">The phone number in any common notation.</param>
    /// <param name="country">The country.</param>
    /// <returns>The E.164 form, for example "+905321234567", or null if the value is not valid.</returns>
    public static string? ToE164(this string? value, Country country) => Phone(country).ToE164(value);

    /// <summary>
    /// Formats a phone number in the national notation of a country.
    /// </summary>
    /// <param name="value">The phone number in any common notation.</param>
    /// <param name="country">The country.</param>
    /// <returns>The formatted number, for example "0532 123 45 67", or null if the value is not valid.</returns>
    public static string? FormatPhone(this string? value, Country country) => Phone(country).Format(value);

    /// <summary>
    /// Checks whether a value is a valid vehicle plate of a country.
    /// </summary>
    /// <param name="value">The plate in any common notation.</param>
    /// <param name="country">The country.</param>
    /// <returns>True if valid; otherwise false.</returns>
    public static bool IsValidVehiclePlate(this string? value, Country country) => Plate(country).IsValid(value);

    /// <summary>
    /// Formats a vehicle plate in the national notation of a country.
    /// </summary>
    /// <param name="value">The plate in any common notation.</param>
    /// <param name="country">The country.</param>
    /// <returns>The formatted plate, for example "34 ABC 123", or null if the value is not valid.</returns>
    public static string? FormatVehiclePlate(this string? value, Country country) => Plate(country).Format(value);

    /// <summary>
    /// Checks whether a value is a valid postal code of a country.
    /// </summary>
    /// <param name="value">The postal code.</param>
    /// <param name="country">The country.</param>
    /// <returns>True if valid; otherwise false.</returns>
    public static bool IsValidPostalCode(this string? value, Country country) =>
        country.Require(country.PostalCodes, "postal code").IsValid(value);

    /// <summary>
    /// Checks whether a value is a valid national identity number of a country, for example a TCKN for Türkiye.
    /// </summary>
    /// <param name="value">The identity number.</param>
    /// <param name="country">The country.</param>
    /// <returns>True if valid; otherwise false.</returns>
    public static bool IsValidNationalId(this string? value, Country country) =>
        country.Require(country.NationalIds, "national identity number").IsValid(value);

    /// <summary>
    /// Checks whether a value is a valid tax identifier of a country, for example a VKN or TCKN for Türkiye.
    /// </summary>
    /// <param name="value">The tax identifier.</param>
    /// <param name="country">The country.</param>
    /// <returns>True if valid; otherwise false.</returns>
    public static bool IsValidTaxId(this string? value, Country country) =>
        country.Require(country.TaxIds, "tax identifier").IsValid(value);

    private static IPhoneNumberRules Phone(Country country)
    {
        ArgumentNullException.ThrowIfNull(country);
        return country.Require(country.PhoneNumbers, "phone number");
    }

    private static IVehiclePlateRules Plate(Country country)
    {
        ArgumentNullException.ThrowIfNull(country);
        return country.Require(country.VehiclePlates, "vehicle plate");
    }
}
