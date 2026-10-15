using FluentValidation;
using HeroKit.Banking;
using HeroKit.Turkish;

namespace HeroKit.FluentValidation;

/// <summary>
/// FluentValidation rules for HeroKit validators.
/// </summary>
/// <remarks>
/// Null and empty values pass, so optional properties stay optional. Combine with <c>NotEmpty()</c> to require a value.
/// Messages use the FluentValidation placeholder <c>{PropertyName}</c> and can be replaced with <c>WithMessage</c>.
/// </remarks>
public static class HeroKitRuleBuilderExtensions
{
    /// <summary>
    /// Validates that the value is a valid Turkish identity number (TCKN).
    /// </summary>
    /// <typeparam name="T">The validated object type.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <returns>The rule builder options, for chaining.</returns>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Value).NotEmpty().Tckn();
    /// </code>
    /// </example>
    public static IRuleBuilderOptions<T, string?> Tckn<T>(this IRuleBuilder<T, string?> ruleBuilder) =>
        ruleBuilder.Must(value => string.IsNullOrEmpty(value) || value.IsValidTckn())
                   .WithMessage("'{PropertyName}' must be a valid Turkish identity number (TCKN).");

    /// <summary>
    /// Validates that the value is a valid Turkish tax number (VKN).
    /// </summary>
    /// <typeparam name="T">The validated object type.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <returns>The rule builder options, for chaining.</returns>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Value).NotEmpty().Vkn();
    /// </code>
    /// </example>
    public static IRuleBuilderOptions<T, string?> Vkn<T>(this IRuleBuilder<T, string?> ruleBuilder) =>
        ruleBuilder.Must(value => string.IsNullOrEmpty(value) || value.IsValidVkn())
                   .WithMessage("'{PropertyName}' must be a valid Turkish tax number (VKN).");

    /// <summary>
    /// Validates that the value is a valid Turkish tax identifier (VKN or TCKN).
    /// </summary>
    /// <typeparam name="T">The validated object type.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <returns>The rule builder options, for chaining.</returns>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Value).NotEmpty().TaxIdTr();
    /// </code>
    /// </example>
    public static IRuleBuilderOptions<T, string?> TaxIdTr<T>(this IRuleBuilder<T, string?> ruleBuilder) =>
        ruleBuilder.Must(value => string.IsNullOrEmpty(value) || value.IsValidTaxIdTr())
                   .WithMessage("'{PropertyName}' must be a valid Turkish tax identifier (VKN or TCKN).");

    /// <summary>
    /// Validates that the value is a valid IBAN.
    /// </summary>
    /// <typeparam name="T">The validated object type.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <returns>The rule builder options, for chaining.</returns>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Value).NotEmpty().Iban();
    /// </code>
    /// </example>
    public static IRuleBuilderOptions<T, string?> Iban<T>(this IRuleBuilder<T, string?> ruleBuilder) =>
        ruleBuilder.Must(value => string.IsNullOrEmpty(value) || value.IsValidIban())
                   .WithMessage("'{PropertyName}' must be a valid IBAN.");

    /// <summary>
    /// Validates that the value is a valid Turkish postal code.
    /// </summary>
    /// <typeparam name="T">The validated object type.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <returns>The rule builder options, for chaining.</returns>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Value).NotEmpty().PostalCodeTr();
    /// </code>
    /// </example>
    public static IRuleBuilderOptions<T, string?> PostalCodeTr<T>(this IRuleBuilder<T, string?> ruleBuilder) =>
        ruleBuilder.Must(value => string.IsNullOrEmpty(value) || value.IsValidPostalCodeTr())
                   .WithMessage("'{PropertyName}' must be a valid Turkish postal code.");

    /// <summary>
    /// Validates that the value is a valid Turkish vehicle plate.
    /// </summary>
    /// <typeparam name="T">The validated object type.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <returns>The rule builder options, for chaining.</returns>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Value).NotEmpty().PlateTr();
    /// </code>
    /// </example>
    public static IRuleBuilderOptions<T, string?> PlateTr<T>(this IRuleBuilder<T, string?> ruleBuilder) =>
        ruleBuilder.Must(value => string.IsNullOrEmpty(value) || value.IsValidPlateTr())
                   .WithMessage("'{PropertyName}' must be a valid Turkish vehicle plate.");

    /// <summary>
    /// Validates that the value is a valid card number.
    /// </summary>
    /// <typeparam name="T">The validated object type.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <returns>The rule builder options, for chaining.</returns>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Value).NotEmpty().CardNumber();
    /// </code>
    /// </example>
    public static IRuleBuilderOptions<T, string?> CardNumber<T>(this IRuleBuilder<T, string?> ruleBuilder) =>
        ruleBuilder.Must(value => string.IsNullOrEmpty(value) || value.IsValidCardNumber())
                   .WithMessage("'{PropertyName}' must be a valid card number.");

    /// <summary>
    /// Validates that the value is a valid Turkish phone number, or a mobile number when <paramref name="mobileOnly"/> is true.
    /// </summary>
    /// <typeparam name="T">The validated object type.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <param name="mobileOnly">When true, only mobile numbers (starting with 5) pass.</param>
    /// <returns>The rule builder options, for chaining.</returns>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Mobile).TurkishPhone(mobileOnly: true);
    /// </code>
    /// </example>
    public static IRuleBuilderOptions<T, string?> TurkishPhone<T>(this IRuleBuilder<T, string?> ruleBuilder, bool mobileOnly = false) =>
        ruleBuilder.Must(value => string.IsNullOrEmpty(value) || (mobileOnly ? value.IsValidMobileTr() : value.IsValidPhoneTr()))
                   .WithMessage(mobileOnly ? "'{PropertyName}' must be a valid Turkish mobile number." : "'{PropertyName}' must be a valid Turkish phone number.");
}
