using System.ComponentModel.DataAnnotations;
using HeroKit.Globalization.Turkey;

namespace HeroKit.Validation;

/// <summary>
/// Validates an e-invoice ETTN (UUID) with <see cref="Globalization.Turkey.EInvoiceExtensions.IsValidEttn"/>.
/// </summary>
/// <remarks>
/// Null and empty values are valid; combine with <see cref="RequiredAttribute"/> to require a value. Non-string values are invalid.
/// </remarks>
/// <example>
/// <code>
/// public sealed class Customer
/// {
///     [Required, Ettn]
///     public string? Value { get; set; }
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class EttnAttribute : ValidationAttribute
{
    /// <summary>
    /// Initializes the attribute with the default error message: "The {0} field must be a valid ETTN (UUID)."
    /// </summary>
    public EttnAttribute()
        : base("The {0} field must be a valid ETTN (UUID).")
    {
    }

    /// <inheritdoc />
    public override bool IsValid(object? value) => value switch
    {
        null => true,
        string { Length: 0 } => true,
        string text => text.IsValidEttn(),
        _ => false,
    };
}
