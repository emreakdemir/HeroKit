namespace HeroKit.Turkish;

/// <summary>
/// Public holidays in Turkey, as defined by Law No. 2429, with religious holiday dates from the Diyanet calendar.
/// </summary>
/// <remarks>
/// Fixed-date holidays are calculated for any year. Ramazan and Kurban Bayramı follow the lunar calendar published by
/// Diyanet İşleri Başkanlığı, so they are kept in a table; years outside <see cref="FirstSupportedYear"/>–<see cref="LastSupportedYear"/>
/// throw instead of returning an incomplete list. Administrative bridge days ("idari izin") are not included.
/// </remarks>
public static class TurkishHolidays
{
    // First day of Ramazan Bayramı (3 days) and Kurban Bayramı (4 days) per year, from the Diyanet religious days calendar.
    private static readonly Dictionary<int, (DateOnly Ramazan, DateOnly Kurban)> ReligiousHolidays = new()
    {
        [2022] = (new DateOnly(2022, 5, 2), new DateOnly(2022, 7, 9)),
        [2023] = (new DateOnly(2023, 4, 21), new DateOnly(2023, 6, 28)),
        [2024] = (new DateOnly(2024, 4, 10), new DateOnly(2024, 6, 16)),
        [2025] = (new DateOnly(2025, 3, 30), new DateOnly(2025, 6, 6)),
        [2026] = (new DateOnly(2026, 3, 20), new DateOnly(2026, 5, 27)),
        [2027] = (new DateOnly(2027, 3, 9), new DateOnly(2027, 5, 16)),
    };

    private static readonly Dictionary<int, IReadOnlyList<PublicHoliday>> Cache = [];
    private static readonly object CacheLock = new();

    /// <summary>
    /// Gets the first year with complete holiday data.
    /// </summary>
    public static int FirstSupportedYear => ReligiousHolidays.Keys.Min();

    /// <summary>
    /// Gets the last year with complete holiday data.
    /// </summary>
    public static int LastSupportedYear => ReligiousHolidays.Keys.Max();

    /// <summary>
    /// Checks whether complete holiday data is available for a year.
    /// </summary>
    /// <param name="year">The year.</param>
    /// <returns>True if religious holiday dates are known for the year; otherwise false.</returns>
    public static bool IsSupported(int year) => ReligiousHolidays.ContainsKey(year);

    /// <summary>
    /// Returns all public holidays of a year, including half days, sorted by date.
    /// </summary>
    /// <param name="year">The year.</param>
    /// <returns>The holidays of the year.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Religious holiday dates are not available for <paramref name="year"/>.</exception>
    /// <example>
    /// <code>
    /// foreach (PublicHoliday holiday in TurkishHolidays.GetHolidays(2026))
    ///     Console.WriteLine($"{holiday.Date} {holiday.Name}");
    /// </code>
    /// </example>
    public static IReadOnlyList<PublicHoliday> GetHolidays(int year)
    {
        if (!ReligiousHolidays.TryGetValue(year, out (DateOnly Ramazan, DateOnly Kurban) religious))
            throw new ArgumentOutOfRangeException(nameof(year), year, $"Turkish religious holiday dates are available for {FirstSupportedYear}–{LastSupportedYear}.");

        lock (CacheLock)
        {
            if (Cache.TryGetValue(year, out IReadOnlyList<PublicHoliday>? cached))
                return cached;

            var holidays = new List<PublicHoliday>
            {
                new(new DateOnly(year, 1, 1), "Yılbaşı", false),
                new(new DateOnly(year, 4, 23), "Ulusal Egemenlik ve Çocuk Bayramı", false),
                new(new DateOnly(year, 5, 1), "Emek ve Dayanışma Günü", false),
                new(new DateOnly(year, 5, 19), "Atatürk'ü Anma, Gençlik ve Spor Bayramı", false),
                new(new DateOnly(year, 7, 15), "Demokrasi ve Millî Birlik Günü", false),
                new(new DateOnly(year, 8, 30), "Zafer Bayramı", false),
                new(new DateOnly(year, 10, 28), "Cumhuriyet Bayramı Arifesi", true),
                new(new DateOnly(year, 10, 29), "Cumhuriyet Bayramı", false),
            };

            AddReligious(holidays, religious.Ramazan, "Ramazan Bayramı", days: 3);
            AddReligious(holidays, religious.Kurban, "Kurban Bayramı", days: 4);

            // A religious holiday can fall on a fixed holiday; the full-day entry wins for the same date.
            IReadOnlyList<PublicHoliday> result = holidays
                .GroupBy(h => h.Date)
                .Select(g => g.OrderBy(h => h.IsHalfDay).First())
                .OrderBy(h => h.Date)
                .ToArray();

            Cache[year] = result;
            return result;
        }
    }

    private static void AddReligious(List<PublicHoliday> holidays, DateOnly firstDay, string name, int days)
    {
        holidays.Add(new PublicHoliday(firstDay.AddDays(-1), $"{name} Arifesi", true));

        for (int day = 0; day < days; day++)
            holidays.Add(new PublicHoliday(firstDay.AddDays(day), $"{name} {day + 1}. Gün", false));
    }

    internal static PublicHoliday? Find(DateOnly date)
    {
        foreach (PublicHoliday holiday in GetHolidays(date.Year))
        {
            if (holiday.Date == date)
                return holiday;
        }

        return null;
    }
}
