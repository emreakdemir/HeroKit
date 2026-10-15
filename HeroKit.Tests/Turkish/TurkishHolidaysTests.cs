using HeroKit.Turkish;

namespace HeroKit.Tests.Turkish;

public class TurkishHolidaysTests
{
    private static DateOnly D(int y, int m, int d) => new(y, m, d);

    [Fact]
    public void GetHolidays_2026_ShouldMatchTheOfficialCalendar()
    {
        IReadOnlyList<PublicHoliday> holidays = TurkishHolidays.GetHolidays(2026);

        // 8 fixed entries (28 October is a half day) + Ramazan (arife + 3) + Kurban (arife + 4).
        Assert.Equal(17, holidays.Count);
        Assert.Equal(holidays.OrderBy(h => h.Date), holidays);
        Assert.Contains(new PublicHoliday(D(2026, 3, 19), "Ramazan Bayramı Arifesi", true), holidays);
        Assert.Contains(new PublicHoliday(D(2026, 3, 20), "Ramazan Bayramı 1. Gün", false), holidays);
        Assert.Contains(new PublicHoliday(D(2026, 5, 26), "Kurban Bayramı Arifesi", true), holidays);
        Assert.Contains(new PublicHoliday(D(2026, 5, 30), "Kurban Bayramı 4. Gün", false), holidays);
        Assert.Contains(new PublicHoliday(D(2026, 10, 28), "Cumhuriyet Bayramı Arifesi", true), holidays);
    }

    [Theory]
    [InlineData(2022, 5, 2, 7, 9)]
    [InlineData(2023, 4, 21, 6, 28)]
    [InlineData(2024, 4, 10, 6, 16)]
    [InlineData(2025, 3, 30, 6, 6)]
    [InlineData(2026, 3, 20, 5, 27)]
    [InlineData(2027, 3, 9, 5, 16)]
    public void ReligiousHolidays_ShouldStartOnDiyanetDates(int year, int rMonth, int rDay, int kMonth, int kDay)
    {
        Assert.Equal("Ramazan Bayramı 1. Gün", D(year, rMonth, rDay).GetPublicHolidayTr()?.Name);
        Assert.Equal("Kurban Bayramı 1. Gün", D(year, kMonth, kDay).GetPublicHolidayTr()?.Name);
    }

    [Fact]
    public void OverlappingHolidays_ShouldKeepTheFullDay()
    {
        // 2027: Kurban Bayramı 4th day is 19 May, which is also a fixed holiday.
        IReadOnlyList<PublicHoliday> holidays = TurkishHolidays.GetHolidays(2027);

        Assert.Single(holidays, h => h.Date == D(2027, 5, 19));
        Assert.False(holidays.Single(h => h.Date == D(2027, 5, 19)).IsHalfDay);
    }

    [Fact]
    public void UnsupportedYears_ShouldThrow()
    {
        Assert.False(TurkishHolidays.IsSupported(2021));
        Assert.Throws<ArgumentOutOfRangeException>(() => TurkishHolidays.GetHolidays(2040));
        Assert.Throws<ArgumentOutOfRangeException>(() => D(2040, 1, 2).IsBusinessDayTr());
        Assert.Equal(2022, TurkishHolidays.FirstSupportedYear);
        Assert.True(TurkishHolidays.LastSupportedYear >= 2027);
    }

    [Theory]
    [InlineData(2026, 10, 29, true, false, false)]   // Cumhuriyet Bayramı
    [InlineData(2026, 10, 28, false, true, true)]    // half day, Wednesday
    [InlineData(2026, 3, 19, false, true, true)]     // Ramazan arifesi, Thursday
    [InlineData(2026, 3, 20, true, false, false)]    // Ramazan 1. gün, Friday
    [InlineData(2026, 10, 31, false, false, false)]  // Saturday
    [InlineData(2026, 10, 27, false, false, true)]   // ordinary Tuesday
    public void Classification_ShouldSeparateFullAndHalfDays(int y, int m, int d, bool holiday, bool halfDay, bool businessDay)
    {
        DateOnly date = D(y, m, d);

        Assert.Equal(holiday, date.IsPublicHolidayTr());
        Assert.Equal(halfDay, date.IsHalfDayHolidayTr());
        Assert.Equal(businessDay, date.IsBusinessDayTr());
        Assert.Equal(businessDay, date.ToDateTime(new TimeOnly(15, 0)).IsBusinessDayTr());
    }

    [Theory]
    [InlineData(2026, 3, 19, 1, 2026, 3, 23)]    // skips Ramazan Bayramı and the weekend
    [InlineData(2026, 5, 22, 3, 2026, 6, 1)]     // Friday before Kurban Bayramı: 25, 26 (arife, half day), then 1 June
    [InlineData(2026, 10, 30, -2, 2026, 10, 27)] // backward over 29 October: 28, 27
    [InlineData(2026, 10, 31, 0, 2026, 10, 31)]
    public void AddBusinessDaysTr_ShouldSkipWeekendsAndHolidays(int y, int m, int d, int days, int ey, int em, int ed)
    {
        Assert.Equal(D(ey, em, ed), D(y, m, d).AddBusinessDaysTr(days));
    }

    [Fact]
    public void AddBusinessDaysTr_DateTime_ShouldKeepTimeAndKind()
    {
        DateTime start = new(2026, 10, 28, 9, 30, 0, DateTimeKind.Utc);

        DateTime result = start.AddBusinessDaysTr(1);

        Assert.Equal(new DateTime(2026, 10, 30, 9, 30, 0, DateTimeKind.Utc), result);
        Assert.Equal(DateTimeKind.Utc, result.Kind);
    }

    [Fact]
    public void BusinessDaysUntilTr_ShouldBeTheInverseOfAddBusinessDays()
    {
        DateOnly start = D(2026, 10, 26);

        Assert.Equal(3, start.BusinessDaysUntilTr(D(2026, 10, 30)));
        Assert.Equal(-3, D(2026, 10, 30).BusinessDaysUntilTr(start));
        Assert.Equal(0, start.BusinessDaysUntilTr(start));

        for (int n = 1; n <= 40; n++)
            Assert.Equal(n, start.BusinessDaysUntilTr(start.AddBusinessDaysTr(n)));
    }

    [Fact]
    public void SupportedYears_ShouldCoverTheCurrentYear()
    {
        // Maintenance alarm: add the next year's Diyanet dates to TurkishHolidays before this fails.
        Assert.True(TurkishHolidays.IsSupported(DateTime.UtcNow.Year), $"Add Diyanet religious holiday dates for {DateTime.UtcNow.Year}.");
    }
}
