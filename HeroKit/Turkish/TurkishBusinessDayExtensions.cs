namespace HeroKit.Turkish;

/// <summary>
/// Public holiday and business day helpers for Turkey.
/// </summary>
/// <remarks>
/// Business days are Monday to Friday, excluding full-day public holidays. Half-day holidays (arife afternoons and 28 October)
/// count as business days because the morning is worked. All methods throw <see cref="ArgumentOutOfRangeException"/> for years
/// outside <see cref="TurkishHolidays.FirstSupportedYear"/>–<see cref="TurkishHolidays.LastSupportedYear"/>.
/// </remarks>
public static class TurkishBusinessDayExtensions
{
    /// <summary>
    /// Checks whether a date is a full-day public holiday in Turkey.
    /// </summary>
    /// <param name="value">The date.</param>
    /// <returns>True for full-day holidays; false for half days and ordinary days.</returns>
    /// <example>
    /// <code>
    /// new DateOnly(2026, 10, 29).IsPublicHolidayTr(); // true
    /// new DateOnly(2026, 10, 28).IsPublicHolidayTr(); // false (half day)
    /// </code>
    /// </example>
    public static bool IsPublicHolidayTr(this DateOnly value) => TurkishHolidays.Find(value) is { IsHalfDay: false };

    /// <summary>
    /// Checks whether a date is a full-day public holiday in Turkey.
    /// </summary>
    /// <param name="value">The date. The time part is ignored.</param>
    /// <returns>True for full-day holidays; false for half days and ordinary days.</returns>
    public static bool IsPublicHolidayTr(this DateTime value) => DateOnly.FromDateTime(value).IsPublicHolidayTr();

    /// <summary>
    /// Checks whether a date is a half-day holiday in Turkey: the eve (arife) of Ramazan or Kurban Bayramı, or 28 October.
    /// </summary>
    /// <param name="value">The date.</param>
    /// <returns>True for half-day holidays; otherwise false.</returns>
    public static bool IsHalfDayHolidayTr(this DateOnly value) => TurkishHolidays.Find(value) is { IsHalfDay: true };

    /// <summary>
    /// Returns the public holiday on a date, including half days.
    /// </summary>
    /// <param name="value">The date.</param>
    /// <returns>The holiday, or null if the date is not a holiday.</returns>
    /// <example>
    /// <code>
    /// new DateOnly(2026, 3, 20).GetPublicHolidayTr()?.Name; // "Ramazan Bayramı 1. Gün"
    /// </code>
    /// </example>
    public static PublicHoliday? GetPublicHolidayTr(this DateOnly value) => TurkishHolidays.Find(value);

    /// <summary>
    /// Checks whether a date is a business day in Turkey: Monday to Friday and not a full-day public holiday.
    /// </summary>
    /// <param name="value">The date.</param>
    /// <returns>True for business days; otherwise false.</returns>
    public static bool IsBusinessDayTr(this DateOnly value) =>
        value.DayOfWeek is not (DayOfWeek.Saturday or DayOfWeek.Sunday) && !value.IsPublicHolidayTr();

    /// <summary>
    /// Checks whether a date is a business day in Turkey.
    /// </summary>
    /// <param name="value">The date. The time part is ignored.</param>
    /// <returns>True for business days; otherwise false.</returns>
    public static bool IsBusinessDayTr(this DateTime value) => DateOnly.FromDateTime(value).IsBusinessDayTr();

    /// <summary>
    /// Moves a date by a number of business days. Negative values move backward.
    /// </summary>
    /// <param name="value">The start date. It does not need to be a business day.</param>
    /// <param name="days">The number of business days to move.</param>
    /// <returns>The resulting business day, or <paramref name="value"/> when <paramref name="days"/> is zero.</returns>
    /// <example>
    /// <code>
    /// // Thursday 19 March 2026 (arife) + 1 business day skips Ramazan Bayramı and the weekend → Monday 23 March.
    /// new DateOnly(2026, 3, 19).AddBusinessDaysTr(1); // 2026-03-23
    /// </code>
    /// </example>
    public static DateOnly AddBusinessDaysTr(this DateOnly value, int days)
    {
        int step = Math.Sign(days);
        DateOnly current = value;

        for (int remaining = Math.Abs(days); remaining > 0;)
        {
            current = current.AddDays(step);

            if (current.IsBusinessDayTr())
                remaining--;
        }

        return current;
    }

    /// <summary>
    /// Moves a date by a number of business days. The time part is kept.
    /// </summary>
    /// <param name="value">The start date and time.</param>
    /// <param name="days">The number of business days to move. Negative values move backward.</param>
    /// <returns>The resulting date with the original time and <see cref="DateTimeKind"/>.</returns>
    public static DateTime AddBusinessDaysTr(this DateTime value, int days) =>
        DateOnly.FromDateTime(value).AddBusinessDaysTr(days).ToDateTime(TimeOnly.FromDateTime(value), value.Kind);

    /// <summary>
    /// Counts the business days after <paramref name="start"/> up to and including <paramref name="end"/>.
    /// </summary>
    /// <remarks>
    /// The count is the inverse of <see cref="AddBusinessDaysTr(DateOnly, int)"/>: when <paramref name="end"/> is a business day,
    /// <c>start.AddBusinessDaysTr(start.BusinessDaysUntilTr(end)) == end</c>. The result is negative when <paramref name="end"/> is earlier.
    /// </remarks>
    /// <param name="start">The start date (not counted).</param>
    /// <param name="end">The end date (counted if it is a business day).</param>
    /// <returns>The number of business days.</returns>
    /// <example>
    /// <code>
    /// new DateOnly(2026, 10, 26).BusinessDaysUntilTr(new DateOnly(2026, 10, 30)); // 3 (29 October is a holiday)
    /// </code>
    /// </example>
    public static int BusinessDaysUntilTr(this DateOnly start, DateOnly end)
    {
        if (end < start)
            return -end.BusinessDaysUntilTr(start);

        int count = 0;

        for (DateOnly day = start.AddDays(1); day <= end; day = day.AddDays(1))
        {
            if (day.IsBusinessDayTr())
                count++;
        }

        return count;
    }
}
