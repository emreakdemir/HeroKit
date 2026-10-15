using System.ComponentModel.DataAnnotations;
using HeroKit.Turkish;

namespace HeroKit.Validation;

/// <summary>
/// Validates a Turkish tax identifier: a VKN or a TCKN, with <see cref="Turkish.TurkishIdentityExtensions.IsValidTaxIdTr"/>.
/// </summary>
/// <remarks>
/// Null and empty values are valid; combine with <see cref="RequiredAttribute"/> to require a value. Non-string values are invalid.
/// </remarks>
/// <example>
/// <code>
/// public sealed class Customer
/// {
///     [Required, TaxIdTr]
///     public string? Value { get; set; }
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class TaxIdTrAttribute : ValidationAttribute
{
    /// <summary>
    /// Initializes the attribute with the default error message: "The {0} field must be a valid Turkish tax identifier (VKN or TCKN)."
    /// </summary>
    public TaxIdTrAttribute()
        : base("The {0} field must be a valid Turkish tax identifier (VKN or TCKN).")
    {
    }

    /// <inheritdoc />
    public override bool IsValid(object? value) => value switch
    {
        null => true,
        string { Length: 0 } => true,
        string text => text.IsValidTaxIdTr(),
        _ => false,
    };
}
