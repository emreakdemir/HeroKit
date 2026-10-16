using FluentValidation;
using HeroKit.Banking;
using HeroKit.Barcodes;
using HeroKit.Globalization;
using HeroKit.Globalization.Turkey;

namespace HeroKit.FluentValidation;

/// <summary>
/// FluentValidation rules for HeroKit validators.
/// </summary>
/// <remarks>
/// Null and empty values pass, so optional properties stay optional. Combine with <c>NotEmpty()</c> to require a value.
/// Messages use the FluentValidation placeholder <c>{PropertyName}</c> and can be replaced with <c>WithMessage</c>.
/// Country rules take a <see cref="Country"/>, for example <see cref="Countries.Turkey"/>.
/// </remarks>
public static class HeroKitRuleBuilderExtensions
{
    /// <summary>
    /// Validates that the value is a valid Turkish identity number (TCKN).
    /// </summary>
    /// <typeparam name="T">The validated object type.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <returns>The rule builder options, for chaining.</returns>
    public static IRuleBuilderOptions<T, string?> Tckn<T>(this IRuleBuilder<T, string?> ruleBuilder) =>
        ruleBuilder.Must(value => string.IsNullOrEmpty(value) || value.IsValidTckn())
                   .WithMessage("'{PropertyName}' must be a valid Turkish identity number (TCKN).");

    /// <summary>
    /// Validates that the value is a valid Turkish tax number (VKN).
    /// </summary>
    /// <typeparam name="T">The validated object type.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <returns>The rule builder options, for chaining.</returns>
    public static IRuleBuilderOptions<T, string?> Vkn<T>(this IRuleBuilder<T, string?> ruleBuilder) =>
        ruleBuilder.Must(value => string.IsNullOrEmpty(value) || value.IsValidVkn())
                   .WithMessage("'{PropertyName}' must be a valid Turkish tax number (VKN).");

    /// <summary>
    /// Validates that the value is a valid IBAN.
    /// </summary>
    /// <typeparam name="T">The validated object type.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <returns>The rule builder options, for chaining.</returns>
    public static IRuleBuilderOptions<T, string?> Iban<T>(this IRuleBuilder<T, string?> ruleBuilder) =>
        ruleBuilder.Must(value => string.IsNullOrEmpty(value) || value.IsValidIban())
                   .WithMessage("'{PropertyName}' must be a valid IBAN.");

    /// <summary>
    /// Validates that the value is a valid card number.
    /// </summary>
    /// <typeparam name="T">The validated object type.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <returns>The rule builder options, for chaining.</returns>
    public static IRuleBuilderOptions<T, string?> CardNumber<T>(this IRuleBuilder<T, string?> ruleBuilder) =>
        ruleBuilder.Must(value => string.IsNullOrEmpty(value) || value.IsValidCardNumber())
                   .WithMessage("'{PropertyName}' must be a valid card number.");

    /// <summary>
    /// Validates that the value is a valid e-invoice number (GİB format).
    /// </summary>
    /// <typeparam name="T">The validated object type.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <returns>The rule builder options, for chaining.</returns>
    public static IRuleBuilderOptions<T, string?> GibInvoiceNumber<T>(this IRuleBuilder<T, string?> ruleBuilder) =>
        ruleBuilder.Must(value => string.IsNullOrEmpty(value) || value.IsValidGibInvoiceNumber())
                   .WithMessage("'{PropertyName}' must be a valid e-invoice number (GİB format).");

    /// <summary>
    /// Validates that the value is a valid ETTN (UUID).
    /// </summary>
    /// <typeparam name="T">The validated object type.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <returns>The rule builder options, for chaining.</returns>
    public static IRuleBuilderOptions<T, string?> Ettn<T>(this IRuleBuilder<T, string?> ruleBuilder) =>
        ruleBuilder.Must(value => string.IsNullOrEmpty(value) || value.IsValidEttn())
                   .WithMessage("'{PropertyName}' must be a valid ETTN (UUID).");

    /// <summary>
    /// Validates that the value is a valid MERSİS number.
    /// </summary>
    /// <typeparam name="T">The validated object type.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <returns>The rule builder options, for chaining.</returns>
    public static IRuleBuilderOptions<T, string?> Mersis<T>(this IRuleBuilder<T, string?> ruleBuilder) =>
        ruleBuilder.Must(value => string.IsNullOrEmpty(value) || value.IsValidMersis())
                   .WithMessage("'{PropertyName}' must be a valid MERSİS number.");

    /// <summary>
    /// Validates that the value is a KEP (Kayıtlı Elektronik Posta) address.
    /// </summary>
    /// <typeparam name="T">The validated object type.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <returns>The rule builder options, for chaining.</returns>
    public static IRuleBuilderOptions<T, string?> KepAddress<T>(this IRuleBuilder<T, string?> ruleBuilder) =>
        ruleBuilder.Must(value => string.IsNullOrEmpty(value) || value.IsValidKepAddress())
                   .WithMessage("'{PropertyName}' must be a valid KEP address.");

    /// <summary>
    /// Validates that the value is a Turkish chip identity card serial number.
    /// </summary>
    /// <typeparam name="T">The validated object type.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <returns>The rule builder options, for chaining.</returns>
    public static IRuleBuilderOptions<T, string?> IdCardSerial<T>(this IRuleBuilder<T, string?> ruleBuilder) =>
        ruleBuilder.Must(value => string.IsNullOrEmpty(value) || value.IsValidIdCardSerial())
                   .WithMessage("'{PropertyName}' must be a valid identity card serial number.");

    /// <summary>
    /// Validates that the value is a valid GTIN barcode number (EAN-8, UPC-A, EAN-13, GTIN-14).
    /// </summary>
    /// <typeparam name="T">The validated object type.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <returns>The rule builder options, for chaining.</returns>
    public static IRuleBuilderOptions<T, string?> Gtin<T>(this IRuleBuilder<T, string?> ruleBuilder) =>
        ruleBuilder.Must(value => string.IsNullOrEmpty(value) || value.IsValidGtin())
                   .WithMessage("'{PropertyName}' must be a valid barcode number (GTIN).");

    /// <summary>
    /// Validates that the value is a valid vehicle plate of a country.
    /// </summary>
    /// <typeparam name="T">The validated object type.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <param name="country">The country, for example <see cref="Countries.Turkey"/>.</param>
    /// <returns>The rule builder options, for chaining.</returns>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Value).VehiclePlate(Countries.Turkey);
    /// </code>
    /// </example>
    public static IRuleBuilderOptions<T, string?> VehiclePlate<T>(this IRuleBuilder<T, string?> ruleBuilder, Country country)
    {
        ArgumentNullException.ThrowIfNull(country);
        return ruleBuilder.Must(value => string.IsNullOrEmpty(value) || value.IsValidVehiclePlate(country))
                          .WithMessage("'{PropertyName}' must be a valid vehicle plate.");
    }

    /// <summary>
    /// Validates that the value is a valid postal code of a country.
    /// </summary>
    /// <typeparam name="T">The validated object type.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <param name="country">The country, for example <see cref="Countries.Turkey"/>.</param>
    /// <returns>The rule builder options, for chaining.</returns>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Value).PostalCode(Countries.Turkey);
    /// </code>
    /// </example>
    public static IRuleBuilderOptions<T, string?> PostalCode<T>(this IRuleBuilder<T, string?> ruleBuilder, Country country)
    {
        ArgumentNullException.ThrowIfNull(country);
        return ruleBuilder.Must(value => string.IsNullOrEmpty(value) || value.IsValidPostalCode(country))
                          .WithMessage("'{PropertyName}' must be a valid postal code.");
    }

    /// <summary>
    /// Validates that the value is a valid national identity number of a country.
    /// </summary>
    /// <typeparam name="T">The validated object type.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <param name="country">The country, for example <see cref="Countries.Turkey"/>.</param>
    /// <returns>The rule builder options, for chaining.</returns>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Value).NationalId(Countries.Turkey);
    /// </code>
    /// </example>
    public static IRuleBuilderOptions<T, string?> NationalId<T>(this IRuleBuilder<T, string?> ruleBuilder, Country country)
    {
        ArgumentNullException.ThrowIfNull(country);
        return ruleBuilder.Must(value => string.IsNullOrEmpty(value) || value.IsValidNationalId(country))
                          .WithMessage("'{PropertyName}' must be a valid national identity number.");
    }

    /// <summary>
    /// Validates that the value is a valid tax identifier of a country.
    /// </summary>
    /// <typeparam name="T">The validated object type.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <param name="country">The country, for example <see cref="Countries.Turkey"/>.</param>
    /// <returns>The rule builder options, for chaining.</returns>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Value).TaxId(Countries.Turkey);
    /// </code>
    /// </example>
    public static IRuleBuilderOptions<T, string?> TaxId<T>(this IRuleBuilder<T, string?> ruleBuilder, Country country)
    {
        ArgumentNullException.ThrowIfNull(country);
        return ruleBuilder.Must(value => string.IsNullOrEmpty(value) || value.IsValidTaxId(country))
                          .WithMessage("'{PropertyName}' must be a valid tax identifier.");
    }

    /// <summary>
    /// Validates that the value is a valid phone number of a country, or a mobile number when <paramref name="mobileOnly"/> is true.
    /// </summary>
    /// <typeparam name="T">The validated object type.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <param name="country">The country, for example <see cref="Countries.Turkey"/>.</param>
    /// <param name="mobileOnly">When true, only mobile numbers pass.</param>
    /// <returns>The rule builder options, for chaining.</returns>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Mobile).PhoneNumber(Countries.Turkey, mobileOnly: true);
    /// </code>
    /// </example>
    public static IRuleBuilderOptions<T, string?> PhoneNumber<T>(this IRuleBuilder<T, string?> ruleBuilder, Country country, bool mobileOnly = false)
    {
        ArgumentNullException.ThrowIfNull(country);
        return ruleBuilder.Must(value => string.IsNullOrEmpty(value) || (mobileOnly ? value.IsValidMobilePhone(country) : value.IsValidPhone(country)))
                          .WithMessage(mobileOnly ? "'{PropertyName}' must be a valid mobile phone number." : "'{PropertyName}' must be a valid phone number.");
    }
}
