namespace HeroKit.Turkish;

/// <summary>
/// A public holiday in Turkey.
/// </summary>
/// <param name="Date">The date of the holiday.</param>
/// <param name="Name">The Turkish name of the holiday, for example "Cumhuriyet Bayramı".</param>
/// <param name="IsHalfDay">True for afternoon-only holidays: the eves (arife) of religious holidays and 28 October.</param>
public readonly record struct PublicHoliday(DateOnly Date, string Name, bool IsHalfDay);
