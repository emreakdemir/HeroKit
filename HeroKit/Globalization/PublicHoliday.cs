namespace HeroKit.Globalization;

/// <summary>
/// A public holiday.
/// </summary>
/// <param name="Date">The date of the holiday.</param>
/// <param name="Name">The name of the holiday in the country's language.</param>
/// <param name="IsHalfDay">True for afternoon-only holidays.</param>
public readonly record struct PublicHoliday(DateOnly Date, string Name, bool IsHalfDay);
