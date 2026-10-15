using System.ComponentModel.DataAnnotations;
using HeroKit.Banking;

namespace HeroKit.Validation;

/// <summary>
/// Validates a payment card number (length and Luhn) with <see cref="Banking.CardNumberExtensions.IsValidCardNumber"/>.
/// </summary>
/// <remarks>
/// Null and empty values are valid; combine with <see cref="RequiredAttribute"/> to require a value. Non-string values are invalid.
/// </remarks>
/// <example>
/// <code>
/// public sealed class Customer
/// {
///     [Required, CardNumber]
///     public string? Value { get; set; }
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class CardNumberAttribute : ValidationAttribute
{
    /// <summary>
    /// Initializes the attribute with the default error message: "The {0} field must be a valid card number."
    /// </summary>
    public CardNumberAttribute()
        : base("The {0} field must be a valid card number.")
    {
    }

    /// <inheritdoc />
    public override bool IsValid(object? value) => value switch
    {
        null => true,
        string { Length: 0 } => true,
        string text => text.IsValidCardNumber(),
        _ => false,
    };
}
