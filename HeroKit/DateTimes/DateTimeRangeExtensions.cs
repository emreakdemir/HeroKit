namespace HeroKit.DateTimes;

/// <summary>
/// Provides range, boundary and age helpers for DateTime and DateOnly values.
/// </summary>
public static class DateTimeRangeExtensions
{
    /// <summary>
    /// Returns the start of the day (00:00:00) and keeps the DateTimeKind.
    /// </summary>
    /// <param name="value">The date and time.</param>
    /// <returns>The first tick of the day.</returns>
    /// <example><code>
    /// new DateTime(2026, 3, 7, 14, 5, 9).StartOfDay(); // 2026-03-07 00:00:00
    /// </code></example>
    public static DateTime StartOfDay(this DateTime value) => value.Date;

    /// <summary>
    /// Returns the last tick of the day and keeps the DateTimeKind.
    /// </summary>
    /// <param name="value">The date and time.</param>
    /// <returns>The last tick of the day.</returns>
    /// <example><code>
    /// new DateTime(2026, 3, 7, 14, 5, 9).EndOfDay(); // 2026-03-07 23:59:59.9999999
    /// </code></example>
    public static DateTime EndOfDay(this DateTime value) => value.Date.AddTicks(TimeSpan.TicksPerDay - 1);

    /// <summary>
    /// Returns the start of the week (Monday, 00:00:00) and keeps the DateTimeKind.
    /// </summary>
    /// <param name="value">The date and time.</param>
    /// <returns>The first tick of the week.</returns>
    /// <example><code>
    /// new DateTime(2026, 3, 7).StartOfWeek(); // 2026-03-02 (Monday)
    /// </code></example>
    public static DateTime StartOfWeek(this DateTime value) => value.StartOfWeek(DayOfWeek.Monday);

    /// <summary>
    /// Returns the first tick of the week for the given first day of the week and keeps the DateTimeKind.
    /// </summary>
    /// <param name="value">The date and time.</param>
    /// <param name="firstDayOfWeek">The day that starts the week.</param>
    /// <returns>The first tick of the week.</returns>
    /// <example><code>
    /// new DateTime(2026, 3, 7).StartOfWeek(DayOfWeek.Sunday);
    /// </code></example>
    public static DateTime StartOfWeek(this DateTime value, DayOfWeek firstDayOfWeek) =>
        value.Date.AddDays(-DaysSinceWeekStart(value.DayOfWeek, firstDayOfWeek));

    /// <summary>
    /// Returns the last tick of the week (Sunday) and keeps the DateTimeKind.
    /// </summary>
    /// <param name="value">The date and time.</param>
    /// <returns>The last tick of the week.</returns>
    /// <example><code>
    /// new DateTime(2026, 3, 7).EndOfWeek(); // 2026-03-08 23:59:59.9999999 (Sunday)
    /// </code></example>
    public static DateTime EndOfWeek(this DateTime value) => value.EndOfWeek(DayOfWeek.Monday);

    /// <summary>
    /// Returns the last tick of the week for the given first day of the week and keeps the DateTimeKind.
    /// </summary>
    /// <param name="value">The date and time.</param>
    /// <param name="firstDayOfWeek">The day that starts the week.</param>
    /// <returns>The last tick of the week.</returns>
    /// <example><code>
    /// new DateTime(2026, 3, 7).EndOfWeek(DayOfWeek.Sunday);
    /// </code></example>
    public static DateTime EndOfWeek(this DateTime value, DayOfWeek firstDayOfWeek) =>
        value.StartOfWeek(firstDayOfWeek).AddTicks(7 * TimeSpan.TicksPerDay - 1);

    /// <summary>
    /// Returns the start of the month and keeps the DateTimeKind.
    /// </summary>
    /// <param name="value">The date and time.</param>
    /// <returns>The first tick of the month.</returns>
    /// <example><code>
    /// new DateTime(2026, 3, 7).StartOfMonth(); // 2026-03-01 00:00:00
    /// </code></example>
    public static DateTime StartOfMonth(this DateTime value) => new(value.Year, value.Month, 1, 0, 0, 0, value.Kind);

    /// <summary>
    /// Returns the last tick of the month and keeps the DateTimeKind.
    /// </summary>
    /// <param name="value">The date and time.</param>
    /// <returns>The last tick of the month.</returns>
    /// <example><code>
    /// new DateTime(2024, 2, 10).EndOfMonth(); // 2024-02-29 23:59:59.9999999
    /// </code></example>
    public static DateTime EndOfMonth(this DateTime value) =>
        new DateTime(value.Year, value.Month, DateTime.DaysInMonth(value.Year, value.Month), 0, 0, 0, value.Kind).AddTicks(TimeSpan.TicksPerDay - 1);

    /// <summary>
    /// Returns the start of the year and keeps the DateTimeKind.
    /// </summary>
    /// <param name="value">The date and time.</param>
    /// <returns>The first tick of the year.</returns>
    /// <example><code>
    /// new DateTime(2026, 3, 7).StartOfYear(); // 2026-01-01 00:00:00
    /// </code></example>
    public static DateTime StartOfYear(this DateTime value) => new(value.Year, 1, 1, 0, 0, 0, value.Kind);

    /// <summary>
    /// Returns the last tick of the year and keeps the DateTimeKind.
    /// </summary>
    /// <param name="value">The date and time.</param>
    /// <returns>The last tick of the year.</returns>
    /// <example><code>
    /// new DateTime(2026, 3, 7).EndOfYear(); // 2026-12-31 23:59:59.9999999
    /// </code></example>
    public static DateTime EndOfYear(this DateTime value) =>
        new DateTime(value.Year, 12, 31, 0, 0, 0, value.Kind).AddTicks(TimeSpan.TicksPerDay - 1);

    /// <summary>
    /// Determines whether the date falls on a Saturday or Sunday.
    /// </summary>
    /// <param name="value">The date and time.</param>
    /// <returns><see langword="true"/> for Saturday and Sunday; otherwise <see langword="false"/>.</returns>
    /// <example><code>
    /// new DateTime(2026, 3, 7).IsWeekend(); // true (Saturday)
    /// </code></example>
    public static bool IsWeekend(this DateTime value) => value.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;

    /// <summary>
    /// Determines whether the value lies between start and end, both ends included.
    /// </summary>
    /// <param name="value">The date and time to test.</param>
    /// <param name="start">The lower bound.</param>
    /// <param name="end">The upper bound.</param>
    /// <returns><see langword="true"/> if start &lt;= value &lt;= end.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="start"/> is later than <paramref name="end"/>.</exception>
    /// <example><code>
    /// new DateTime(2026, 3, 7).IsBetween(new DateTime(2026, 3, 1), new DateTime(2026, 3, 7)); // true
    /// </code></example>
    public static bool IsBetween(this DateTime value, DateTime start, DateTime end)
    {
        if (start > end)
            throw new ArgumentException("Start must not be later than end.", nameof(start));

        return value >= start && value <= end;
    }

    /// <summary>
    /// Calculates the completed years between a birth date and a reference date.
    /// A Feb 29 birthday counts as completed on Mar 1 in non-leap years.
    /// </summary>
    /// <param name="birthDate">The birth date.</param>
    /// <param name="asOf">The reference date.</param>
    /// <returns>The number of completed years.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="asOf"/> is earlier than <paramref name="birthDate"/>.</exception>
    /// <example><code>
    /// new DateTime(1990, 5, 20).Age(new DateTime(2026, 5, 19)); // 35
    /// </code></example>
    public static int Age(this DateTime birthDate, DateTime asOf)
    {
        if (asOf < birthDate)
            throw new ArgumentOutOfRangeException(nameof(asOf), asOf, "Reference date must not be earlier than the birth date.");

        int years = asOf.Year - birthDate.Year;
        if (asOf < Anniversary(birthDate, years))
            years--;

        return years;
    }

    /// <summary>
    /// Returns the start of the week (first day of the week) for a date.
    /// </summary>
    /// <param name="value">The date.</param>
    /// <returns>The first day of the week (Monday start).</returns>
    /// <example><code>
    /// new DateOnly(2026, 3, 7).StartOfWeek(); // 2026-03-02
    /// </code></example>
    public static DateOnly StartOfWeek(this DateOnly value) => value.StartOfWeek(DayOfWeek.Monday);

    /// <summary>
    /// Returns the first day of the week for a date.
    /// </summary>
    /// <param name="value">The date.</param>
    /// <param name="firstDayOfWeek">The day that starts the week.</param>
    /// <returns>The first day of the week.</returns>
    /// <example><code>
    /// new DateOnly(2026, 3, 7).StartOfWeek(DayOfWeek.Sunday);
    /// </code></example>
    public static DateOnly StartOfWeek(this DateOnly value, DayOfWeek firstDayOfWeek) =>
        value.AddDays(-DaysSinceWeekStart(value.DayOfWeek, firstDayOfWeek));

    /// <summary>
    /// Returns the end of the week (last day of the week) for a date.
    /// </summary>
    /// <param name="value">The date.</param>
    /// <returns>The last day of the week (Monday start).</returns>
    /// <example><code>
    /// new DateOnly(2026, 3, 7).EndOfWeek(); // 2026-03-08
    /// </code></example>
    public static DateOnly EndOfWeek(this DateOnly value) => value.EndOfWeek(DayOfWeek.Monday);

    /// <summary>
    /// Returns the last day of the week for a date.
    /// </summary>
    /// <param name="value">The date.</param>
    /// <param name="firstDayOfWeek">The day that starts the week.</param>
    /// <returns>The last day of the week.</returns>
    /// <example><code>
    /// new DateOnly(2026, 3, 7).EndOfWeek(DayOfWeek.Sunday);
    /// </code></example>
    public static DateOnly EndOfWeek(this DateOnly value, DayOfWeek firstDayOfWeek) =>
        value.AddDays(6 - DaysSinceWeekStart(value.DayOfWeek, firstDayOfWeek));

    /// <summary>
    /// Returns the first day of the month.
    /// </summary>
    /// <param name="value">The date.</param>
    /// <returns>The first day of the month.</returns>
    /// <example><code>
    /// new DateOnly(2026, 3, 7).StartOfMonth(); // 2026-03-01
    /// </code></example>
    public static DateOnly StartOfMonth(this DateOnly value) => new(value.Year, value.Month, 1);

    /// <summary>
    /// Returns the last day of the month.
    /// </summary>
    /// <param name="value">The date.</param>
    /// <returns>The last day of the month.</returns>
    /// <example><code>
    /// new DateOnly(2024, 2, 10).EndOfMonth(); // 2024-02-29
    /// </code></example>
    public static DateOnly EndOfMonth(this DateOnly value) => new(value.Year, value.Month, DateTime.DaysInMonth(value.Year, value.Month));

    /// <summary>
    /// Determines whether the date falls on a Saturday or Sunday.
    /// </summary>
    /// <param name="value">The date.</param>
    /// <returns><see langword="true"/> for Saturday and Sunday; otherwise <see langword="false"/>.</returns>
    /// <example><code>
    /// new DateOnly(2026, 3, 7).IsWeekend(); // true
    /// </code></example>
    public static bool IsWeekend(this DateOnly value) => value.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;

    /// <summary>
    /// Determines whether the date lies between start and end, both ends included.
    /// </summary>
    /// <param name="value">The date to test.</param>
    /// <param name="start">The lower bound.</param>
    /// <param name="end">The upper bound.</param>
    /// <returns><see langword="true"/> if start &lt;= value &lt;= end.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="start"/> is later than <paramref name="end"/>.</exception>
    /// <example><code>
    /// new DateOnly(2026, 3, 7).IsBetween(new DateOnly(2026, 3, 1), new DateOnly(2026, 3, 7)); // true
    /// </code></example>
    public static bool IsBetween(this DateOnly value, DateOnly start, DateOnly end)
    {
        if (start > end)
            throw new ArgumentException("Start must not be later than end.", nameof(start));

        return value >= start && value <= end;
    }

    /// <summary>
    /// Calculates the completed years between a birth date and a reference date.
    /// A Feb 29 birthday counts as completed on Mar 1 in non-leap years.
    /// </summary>
    /// <param name="birthDate">The birth date.</param>
    /// <param name="asOf">The reference date.</param>
    /// <returns>The number of completed years.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="asOf"/> is earlier than <paramref name="birthDate"/>.</exception>
    /// <example><code>
    /// new DateOnly(2000, 2, 29).Age(new DateOnly(2026, 3, 1)); // 26
    /// </code></example>
    public static int Age(this DateOnly birthDate, DateOnly asOf)
    {
        if (asOf < birthDate)
            throw new ArgumentOutOfRangeException(nameof(asOf), asOf, "Reference date must not be earlier than the birth date.");

        int years = asOf.Year - birthDate.Year;
        if (asOf < Anniversary(birthDate, years))
            years--;

        return years;
    }

    private static int DaysSinceWeekStart(DayOfWeek day, DayOfWeek firstDayOfWeek) => ((int)day - (int)firstDayOfWeek + 7) % 7;

    private static DateTime Anniversary(DateTime birthDate, int years)
    {
        int year = birthDate.Year + years;
        if (birthDate.Month == 2 && birthDate.Day == 29 && !DateTime.IsLeapYear(year))
            return new DateTime(year, 3, 1, 0, 0, 0, birthDate.Kind).Add(birthDate.TimeOfDay);

        return birthDate.AddYears(years);
    }

    private static DateOnly Anniversary(DateOnly birthDate, int years)
    {
        int year = birthDate.Year + years;
        if (birthDate.Month == 2 && birthDate.Day == 29 && !DateTime.IsLeapYear(year))
            return new DateOnly(year, 3, 1);

        return birthDate.AddYears(years);
    }
}
