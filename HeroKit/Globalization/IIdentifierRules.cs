namespace HeroKit.Globalization;

/// <summary>
/// Validation rules of an identifier, such as a postal code, a national identity number or a tax number.
/// </summary>
public interface IIdentifierRules
{
    /// <summary>
    /// Checks whether a value is a valid identifier.
    /// </summary>
    /// <param name="value">The identifier.</param>
    /// <returns>True if valid; otherwise false.</returns>
    bool IsValid(string? value);
}
