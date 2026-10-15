namespace HeroKit.Globalization;

/// <summary>
/// Public holidays and weekend days of a country.
/// </summary>
public interface IHolidayCalendar
{
    /// <summary>
    /// Gets the first year with complete holiday data.
    /// </summary>
    int FirstSupportedYear { get; }

    /// <summary>
    /// Gets the last year with complete holiday data.
    /// </summary>
    int LastSupportedYear { get; }

    /// <summary>
    /// Checks whether a day of the week is a weekend day.
    /// </summary>
    /// <param name="day">The day of the week.</param>
    /// <returns>True for weekend days.</returns>
    bool IsWeekend(DayOfWeek day);

    /// <summary>
    /// Returns all public holidays of a year, including half days, sorted by date.
    /// </summary>
    /// <param name="year">The year.</param>
    /// <returns>The holidays of the year.</returns>
    /// <exception cref="ArgumentOutOfRangeException">The year is outside the supported range.</exception>
    IReadOnlyList<PublicHoliday> GetHolidays(int year);
}
