using System.ComponentModel.DataAnnotations;
using HeroKit.Turkish;

namespace HeroKit.Validation;

/// <summary>
/// Validates a Turkish phone number with <see cref="TurkishPhoneExtensions.IsValidPhoneTr"/>,
/// or only mobile numbers with <see cref="TurkishPhoneExtensions.IsValidMobileTr"/> when <see cref="MobileOnly"/> is true.
/// </summary>
/// <remarks>
/// Null and empty values are valid; combine with <see cref="RequiredAttribute"/> to require a value. Non-string values are invalid.
/// </remarks>
/// <example>
/// <code>
/// [TurkishPhone(MobileOnly = true)]
/// public string? Mobile { get; set; }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class TurkishPhoneAttribute : ValidationAttribute
{
    /// <summary>
    /// Initializes the attribute with the default error message: "The {0} field must be a valid Turkish phone number."
    /// </summary>
    public TurkishPhoneAttribute()
        : base("The {0} field must be a valid Turkish phone number.")
    {
    }

    /// <summary>
    /// Gets or sets whether only mobile numbers (starting with 5) are accepted.
    /// </summary>
    public bool MobileOnly { get; set; }

    /// <inheritdoc />
    public override bool IsValid(object? value) => value switch
    {
        null => true,
        string { Length: 0 } => true,
        string text => MobileOnly ? text.IsValidMobileTr() : text.IsValidPhoneTr(),
        _ => false,
    };
}
