using System.ComponentModel.DataAnnotations;
using HeroKit.Globalization;

namespace HeroKit.Validation;

/// <summary>
/// Base class for attributes that validate a string with the rules of a country registered in <see cref="Countries"/>.
/// </summary>
/// <remarks>
/// The country is resolved by its ISO code when a value is validated, so countries registered after the attribute
/// was created are found. Null and empty values are valid; combine with <see cref="RequiredAttribute"/> to require a value.
/// Non-string values are invalid.
/// </remarks>
public abstract class CountryValidationAttribute : ValidationAttribute
{
    /// <summary>
    /// Initializes the attribute.
    /// </summary>
    /// <param name="countryCode">The ISO 3166-1 alpha-2 code, for example "TR".</param>
    /// <param name="errorMessage">The default error message; "{0}" is the field name.</param>
    protected CountryValidationAttribute(string countryCode, string errorMessage)
        : base(errorMessage)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(countryCode);
        CountryCode = countryCode;
    }

    /// <summary>
    /// Gets the ISO 3166-1 alpha-2 code of the country.
    /// </summary>
    public string CountryCode { get; }

    /// <inheritdoc />
    public override bool IsValid(object? value) => value switch
    {
        null => true,
        string { Length: 0 } => true,
        string text => IsValid(text, Countries.Get(CountryCode)),
        _ => false,
    };

    /// <summary>
    /// Validates a non-empty string with the rules of the country.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="country">The resolved country.</param>
    /// <returns>True if valid; otherwise false.</returns>
    protected abstract bool IsValid(string value, Country country);
}

/// <summary>
/// Validates a phone number of a country, for example <c>[PhoneNumber("TR")]</c>.
/// </summary>
/// <param name="countryCode">The ISO 3166-1 alpha-2 code.</param>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class PhoneNumberAttribute(string countryCode)
    : CountryValidationAttribute(countryCode, "The {0} field must be a valid phone number.")
{
    /// <summary>
    /// Gets or sets whether only mobile numbers are accepted.
    /// </summary>
    public bool MobileOnly { get; set; }

    /// <inheritdoc />
    protected override bool IsValid(string value, Country country) =>
        MobileOnly ? value.IsValidMobilePhone(country) : value.IsValidPhone(country);
}

/// <summary>
/// Validates a vehicle plate of a country, for example <c>[VehiclePlate("TR")]</c>.
/// </summary>
/// <param name="countryCode">The ISO 3166-1 alpha-2 code.</param>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class VehiclePlateAttribute(string countryCode)
    : CountryValidationAttribute(countryCode, "The {0} field must be a valid vehicle plate.")
{
    /// <inheritdoc />
    protected override bool IsValid(string value, Country country) => value.IsValidVehiclePlate(country);
}

/// <summary>
/// Validates a postal code of a country, for example <c>[PostalCode("TR")]</c>.
/// </summary>
/// <param name="countryCode">The ISO 3166-1 alpha-2 code.</param>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class PostalCodeAttribute(string countryCode)
    : CountryValidationAttribute(countryCode, "The {0} field must be a valid postal code.")
{
    /// <inheritdoc />
    protected override bool IsValid(string value, Country country) => value.IsValidPostalCode(country);
}

/// <summary>
/// Validates a national identity number of a country, for example <c>[NationalId("TR")]</c> for a TCKN.
/// </summary>
/// <param name="countryCode">The ISO 3166-1 alpha-2 code.</param>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class NationalIdAttribute(string countryCode)
    : CountryValidationAttribute(countryCode, "The {0} field must be a valid national identity number.")
{
    /// <inheritdoc />
    protected override bool IsValid(string value, Country country) => value.IsValidNationalId(country);
}

/// <summary>
/// Validates a tax identifier of a country, for example <c>[TaxId("TR")]</c> for a VKN or TCKN.
/// </summary>
/// <param name="countryCode">The ISO 3166-1 alpha-2 code.</param>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class TaxIdAttribute(string countryCode)
    : CountryValidationAttribute(countryCode, "The {0} field must be a valid tax identifier.")
{
    /// <inheritdoc />
    protected override bool IsValid(string value, Country country) => value.IsValidTaxId(country);
}
