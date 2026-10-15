namespace HeroKit.Globalization;

/// <summary>
/// Phone number rules of a country.
/// </summary>
public interface IPhoneNumberRules
{
    /// <summary>
    /// Checks whether a value is a valid phone number of the country.
    /// </summary>
    /// <param name="value">The phone number in any common notation.</param>
    /// <returns>True if valid; otherwise false.</returns>
    bool IsValid(string? value);

    /// <summary>
    /// Checks whether a value is a valid mobile phone number of the country.
    /// </summary>
    /// <param name="value">The phone number in any common notation.</param>
    /// <returns>True if valid and mobile; otherwise false.</returns>
    bool IsMobile(string? value);

    /// <summary>
    /// Converts a phone number to E.164, for example "+905321234567".
    /// </summary>
    /// <param name="value">The phone number in any common notation.</param>
    /// <returns>The E.164 form, or null if the value is not valid.</returns>
    string? ToE164(string? value);

    /// <summary>
    /// Formats a phone number in the national notation of the country.
    /// </summary>
    /// <param name="value">The phone number in any common notation.</param>
    /// <returns>The formatted number, or null if the value is not valid.</returns>
    string? Format(string? value);
}
