using System.ComponentModel.DataAnnotations;
using HeroKit.Globalization.Turkey;

namespace HeroKit.Validation;

/// <summary>
/// Validates a Turkish tax number (Vergi Kimlik No) with <see cref="Globalization.Turkey.TurkeyIdentityExtensions.IsValidVkn"/>.
/// </summary>
/// <remarks>
/// Null and empty values are valid; combine with <see cref="RequiredAttribute"/> to require a value. Non-string values are invalid.
/// </remarks>
/// <example>
/// <code>
/// public sealed class Customer
/// {
///     [Required, Vkn]
///     public string? Value { get; set; }
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class VknAttribute : ValidationAttribute
{
    /// <summary>
    /// Initializes the attribute with the default error message: "The {0} field must be a valid Turkish tax number (VKN)."
    /// </summary>
    public VknAttribute()
        : base("The {0} field must be a valid Turkish tax number (VKN).")
    {
    }

    /// <inheritdoc />
    public override bool IsValid(object? value) => value switch
    {
        null => true,
        string { Length: 0 } => true,
        string text => text.IsValidVkn(),
        _ => false,
    };
}
