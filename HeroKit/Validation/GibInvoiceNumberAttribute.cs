using System.ComponentModel.DataAnnotations;
using HeroKit.Globalization.Turkey;

namespace HeroKit.Validation;

/// <summary>
/// Validates a Turkish e-Fatura or e-Arşiv invoice number (GİB format) with <see cref="Globalization.Turkey.EInvoiceExtensions.IsValidGibInvoiceNumber"/>.
/// </summary>
/// <remarks>
/// Null and empty values are valid; combine with <see cref="RequiredAttribute"/> to require a value. Non-string values are invalid.
/// </remarks>
/// <example>
/// <code>
/// public sealed class Customer
/// {
///     [Required, GibInvoiceNumber]
///     public string? Value { get; set; }
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class GibInvoiceNumberAttribute : ValidationAttribute
{
    /// <summary>
    /// Initializes the attribute with the default error message: "The {0} field must be a valid e-invoice number (GİB format)."
    /// </summary>
    public GibInvoiceNumberAttribute()
        : base("The {0} field must be a valid e-invoice number (GİB format).")
    {
    }

    /// <inheritdoc />
    public override bool IsValid(object? value) => value switch
    {
        null => true,
        string { Length: 0 } => true,
        string text => text.IsValidGibInvoiceNumber(),
        _ => false,
    };
}
