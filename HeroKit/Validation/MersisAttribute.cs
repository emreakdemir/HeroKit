using System.ComponentModel.DataAnnotations;
using HeroKit.Globalization.Turkey;

namespace HeroKit.Validation;

/// <summary>
/// Validates a MERSİS number with <see cref="Globalization.Turkey.TurkeyIdentityExtensions.IsValidMersis"/>.
/// </summary>
/// <remarks>
/// Null and empty values are valid; combine with <see cref="RequiredAttribute"/> to require a value. Non-string values are invalid.
/// </remarks>
/// <example>
/// <code>
/// public sealed class Customer
/// {
///     [Required, Mersis]
///     public string? Value { get; set; }
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class MersisAttribute : ValidationAttribute
{
    /// <summary>
    /// Initializes the attribute with the default error message: "The {0} field must be a valid MERSİS number."
    /// </summary>
    public MersisAttribute()
        : base("The {0} field must be a valid MERSİS number.")
    {
    }

    /// <inheritdoc />
    public override bool IsValid(object? value) => value switch
    {
        null => true,
        string { Length: 0 } => true,
        string text => text.IsValidMersis(),
        _ => false,
    };
}
