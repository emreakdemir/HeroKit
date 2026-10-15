using System.ComponentModel.DataAnnotations;
using HeroKit.Turkish;

namespace HeroKit.Validation;

/// <summary>
/// Validates a Turkish postal code with <see cref="Turkish.TurkishLocationExtensions.IsValidPostalCodeTr"/>.
/// </summary>
/// <remarks>
/// Null and empty values are valid; combine with <see cref="RequiredAttribute"/> to require a value. Non-string values are invalid.
/// </remarks>
/// <example>
/// <code>
/// public sealed class Customer
/// {
///     [Required, PostalCodeTr]
///     public string? Value { get; set; }
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class PostalCodeTrAttribute : ValidationAttribute
{
    /// <summary>
    /// Initializes the attribute with the default error message: "The {0} field must be a valid Turkish postal code."
    /// </summary>
    public PostalCodeTrAttribute()
        : base("The {0} field must be a valid Turkish postal code.")
    {
    }

    /// <inheritdoc />
    public override bool IsValid(object? value) => value switch
    {
        null => true,
        string { Length: 0 } => true,
        string text => text.IsValidPostalCodeTr(),
        _ => false,
    };
}
