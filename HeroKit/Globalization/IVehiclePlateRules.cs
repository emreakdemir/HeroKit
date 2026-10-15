namespace HeroKit.Globalization;

/// <summary>
/// Vehicle registration plate rules of a country.
/// </summary>
public interface IVehiclePlateRules
{
    /// <summary>
    /// Checks whether a value is a valid plate of the country.
    /// </summary>
    /// <param name="value">The plate in any common notation.</param>
    /// <returns>True if valid; otherwise false.</returns>
    bool IsValid(string? value);

    /// <summary>
    /// Formats a plate in the national notation of the country.
    /// </summary>
    /// <param name="value">The plate in any common notation.</param>
    /// <returns>The formatted plate, or null if the value is not valid.</returns>
    string? Format(string? value);
}
