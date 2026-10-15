namespace HeroKit.Globalization.UnitedStates;

// The 11 federal holidays (5 U.S.C. 6103). They follow fixed rules, so every year from 1986 (first Martin Luther King Jr. Day)
// is supported; Juneteenth starts in 2021. A holiday on a Saturday is observed on the Friday before and a holiday on a Sunday
// on the Monday after; the observed day is listed as a separate "(observed)" entry. New Year's Day of the next year can be
// observed on December 31.
internal sealed class UnitedStatesHolidayCalendar : IHolidayCalendar
{
    public static UnitedStatesHolidayCalendar Instance { get; } = new();

    public int FirstSupportedYear => 1986;

    public int LastSupportedYear => 9998;

    public bool IsWeekend(DayOfWeek day) => day is DayOfWeek.Saturday or DayOfWeek.Sunday;

    public IReadOnlyList<PublicHoliday> GetHolidays(int year)
    {
        if (year < FirstSupportedYear || year > LastSupportedYear)
            throw new ArgumentOutOfRangeException(nameof(year), year, $"US federal holidays are available for {FirstSupportedYear}-{LastSupportedYear}.");

        var holidays = new List<PublicHoliday>();
        AddFixed(holidays, new DateOnly(year, 1, 1), "New Year's Day");
        holidays.Add(new(Nth(year, 1, DayOfWeek.Monday, 3), "Martin Luther King Jr. Day", false));
        holidays.Add(new(Nth(year, 2, DayOfWeek.Monday, 3), "Washington's Birthday", false));
        holidays.Add(new(Last(year, 5, DayOfWeek.Monday), "Memorial Day", false));
        if (year >= 2021)
            AddFixed(holidays, new DateOnly(year, 6, 19), "Juneteenth National Independence Day");
        AddFixed(holidays, new DateOnly(year, 7, 4), "Independence Day");
        holidays.Add(new(Nth(year, 9, DayOfWeek.Monday, 1), "Labor Day", false));
        holidays.Add(new(Nth(year, 10, DayOfWeek.Monday, 2), "Columbus Day", false));
        AddFixed(holidays, new DateOnly(year, 11, 11), "Veterans Day");
        holidays.Add(new(Nth(year, 11, DayOfWeek.Thursday, 4), "Thanksgiving Day", false));
        AddFixed(holidays, new DateOnly(year, 12, 25), "Christmas Day");

        // New Year's Day of the next year on a Saturday is observed on December 31 of this year.
        if (new DateOnly(year + 1, 1, 1).DayOfWeek == DayOfWeek.Saturday)
            holidays.Add(new(new DateOnly(year, 12, 31), "New Year's Day (observed)", false));

        // Observed days of New Year's Day that fall into the previous year are removed.
        holidays.RemoveAll(h => h.Date.Year != year);
        holidays.Sort((a, b) => a.Date.CompareTo(b.Date));
        return holidays;
    }

    private static void AddFixed(List<PublicHoliday> holidays, DateOnly date, string name)
    {
        holidays.Add(new(date, name, false));

        DateOnly observed = date.DayOfWeek switch
        {
            DayOfWeek.Saturday => date.AddDays(-1),
            DayOfWeek.Sunday => date.AddDays(1),
            _ => date,
        };

        if (observed != date)
            holidays.Add(new(observed, $"{name} (observed)", false));
    }

    private static DateOnly Nth(int year, int month, DayOfWeek day, int n)
    {
        var first = new DateOnly(year, month, 1);
        int offset = ((int)day - (int)first.DayOfWeek + 7) % 7;
        return first.AddDays(offset + (n - 1) * 7);
    }

    private static DateOnly Last(int year, int month, DayOfWeek day)
    {
        var last = new DateOnly(year, month, DateTime.DaysInMonth(year, month));
        int offset = ((int)last.DayOfWeek - (int)day + 7) % 7;
        return last.AddDays(-offset);
    }
}
