using System.ComponentModel.DataAnnotations;
using HeroKit.Banking;

namespace HeroKit.Validation;

/// <summary>
/// Validates an IBAN (ISO 13616) with <see cref="Banking.IbanExtensions.IsValidIban"/>.
/// </summary>
/// <remarks>
/// Null and empty values are valid; combine with <see cref="RequiredAttribute"/> to require a value. Non-string values are invalid.
/// </remarks>
/// <example>
/// <code>
/// public sealed class Customer
/// {
///     [Required, Iban]
///     public string? Value { get; set; }
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class IbanAttribute : ValidationAttribute
{
    /// <summary>
    /// Initializes the attribute with the default error message: "The {0} field must be a valid IBAN."
    /// </summary>
    public IbanAttribute()
        : base("The {0} field must be a valid IBAN.")
    {
    }

    /// <inheritdoc />
    public override bool IsValid(object? value) => value switch
    {
        null => true,
        string { Length: 0 } => true,
        string text => text.IsValidIban(),
        _ => false,
    };
}
