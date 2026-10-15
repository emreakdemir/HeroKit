using System.ComponentModel.DataAnnotations;
using HeroKit.Turkish;

namespace HeroKit.Validation;

/// <summary>
/// Validates a Turkish identity number (T.C. Kimlik No) with <see cref="Turkish.TurkishIdentityExtensions.IsValidTckn"/>.
/// </summary>
/// <remarks>
/// Null and empty values are valid; combine with <see cref="RequiredAttribute"/> to require a value. Non-string values are invalid.
/// </remarks>
/// <example>
/// <code>
/// public sealed class Customer
/// {
///     [Required, Tckn]
///     public string? Value { get; set; }
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class TcknAttribute : ValidationAttribute
{
    /// <summary>
    /// Initializes the attribute with the default error message: "The {0} field must be a valid Turkish identity number (TCKN)."
    /// </summary>
    public TcknAttribute()
        : base("The {0} field must be a valid Turkish identity number (TCKN).")
    {
    }

    /// <inheritdoc />
    public override bool IsValid(object? value) => value switch
    {
        null => true,
        string { Length: 0 } => true,
        string text => text.IsValidTckn(),
        _ => false,
    };
}
