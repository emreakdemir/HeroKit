using System.ComponentModel.DataAnnotations;
using HeroKit.Barcodes;

namespace HeroKit.Validation;

/// <summary>
/// Validates a GTIN barcode number (EAN-8, UPC-A, EAN-13, GTIN-14) with <see cref="Barcodes.BarcodeExtensions.IsValidGtin"/>.
/// </summary>
/// <remarks>
/// Null and empty values are valid; combine with <see cref="RequiredAttribute"/> to require a value. Non-string values are invalid.
/// </remarks>
/// <example>
/// <code>
/// public sealed class Customer
/// {
///     [Required, Gtin]
///     public string? Value { get; set; }
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class GtinAttribute : ValidationAttribute
{
    /// <summary>
    /// Initializes the attribute with the default error message: "The {0} field must be a valid barcode number (GTIN)."
    /// </summary>
    public GtinAttribute()
        : base("The {0} field must be a valid barcode number (GTIN).")
    {
    }

    /// <inheritdoc />
    public override bool IsValid(object? value) => value switch
    {
        null => true,
        string { Length: 0 } => true,
        string text => text.IsValidGtin(),
        _ => false,
    };
}
