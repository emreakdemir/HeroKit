using HeroKit.DateTimes;

namespace HeroKit.Tests.DateTimes;

public class DateTimeRangeExtensionsTests
{
    private const long LastTick = TimeSpan.TicksPerDay - 1;

    [Theory]
    [InlineData(2026, 1, 31)]
    [InlineData(2026, 2, 28)]
    [InlineData(2024, 2, 29)]
    [InlineData(2026, 4, 30)]
    [InlineData(2026, 12, 31)]
    public void EndOfMonth_ShouldReturnLastTickOfLastDay(int year, int month, int lastDay)
    {
        DateTime input = new(year, month, 10, 13, 0, 0);
        Assert.Equal(new DateTime(year, month, lastDay).AddTicks(LastTick), input.EndOfMonth());
        Assert.Equal(new DateOnly(year, month, lastDay), DateOnly.FromDateTime(input).EndOfMonth());
    }

    [Fact]
    public void DayYearAndMonthBoundaries_ShouldBeCorrect()
    {
        DateTime d = new(2026, 3, 7, 14, 5, 9);
        Assert.Equal(new DateTime(2026, 3, 7), d.StartOfDay());
        Assert.Equal(new DateTime(2026, 3, 7).AddTicks(LastTick), d.EndOfDay());
        Assert.Equal(new DateTime(2026, 3, 1), d.StartOfMonth());
        Assert.Equal(new DateTime(2026, 1, 1), d.StartOfYear());
        Assert.Equal(new DateTime(2026, 12, 31).AddTicks(LastTick), d.EndOfYear());
        Assert.Equal(new DateOnly(2026, 3, 1), new DateOnly(2026, 3, 7).StartOfMonth());
    }

    [Theory]
    // 2026-03-07 is a Saturday; 2026-03-02 is a Monday; 2026-03-01 is a Sunday.
    [InlineData(2026, 3, 7, DayOfWeek.Monday, 2026, 3, 2, 2026, 3, 8)]
    [InlineData(2026, 3, 2, DayOfWeek.Monday, 2026, 3, 2, 2026, 3, 8)]
    [InlineData(2026, 3, 8, DayOfWeek.Monday, 2026, 3, 2, 2026, 3, 8)]
    [InlineData(2026, 3, 7, DayOfWeek.Sunday, 2026, 3, 1, 2026, 3, 7)]
    [InlineData(2026, 3, 1, DayOfWeek.Sunday, 2026, 3, 1, 2026, 3, 7)]
    [InlineData(2026, 3, 4, DayOfWeek.Sunday, 2026, 3, 1, 2026, 3, 7)]
    public void WeekBoundaries_ShouldFollowFirstDayOfWeek(int y, int m, int d, DayOfWeek first, int sy, int sm, int sd, int ey, int em, int ed)
    {
        DateTime input = new(y, m, d, 9, 30, 0);
        Assert.Equal(new DateTime(sy, sm, sd), input.StartOfWeek(first));
        Assert.Equal(new DateTime(ey, em, ed).AddTicks(LastTick), input.EndOfWeek(first));

        DateOnly date = new(y, m, d);
        Assert.Equal(new DateOnly(sy, sm, sd), date.StartOfWeek(first));
        Assert.Equal(new DateOnly(ey, em, ed), date.EndOfWeek(first));
    }

    [Fact]
    public void WeekBoundaries_ShouldDefaultToMonday()
    {
        Assert.Equal(new DateTime(2026, 3, 2), new DateTime(2026, 3, 7).StartOfWeek());
        Assert.Equal(new DateOnly(2026, 3, 8), new DateOnly(2026, 3, 7).EndOfWeek());
    }

    [Theory]
    [InlineData(DateTimeKind.Utc)]
    [InlineData(DateTimeKind.Local)]
    [InlineData(DateTimeKind.Unspecified)]
    public void AllDateTimeResults_ShouldPreserveKind(DateTimeKind kind)
    {
        DateTime d = new(2026, 3, 7, 14, 5, 9, kind);
        Assert.Equal(kind, d.StartOfDay().Kind);
        Assert.Equal(kind, d.EndOfDay().Kind);
        Assert.Equal(kind, d.StartOfWeek().Kind);
        Assert.Equal(kind, d.EndOfWeek().Kind);
        Assert.Equal(kind, d.StartOfMonth().Kind);
        Assert.Equal(kind, d.EndOfMonth().Kind);
        Assert.Equal(kind, d.StartOfYear().Kind);
        Assert.Equal(kind, d.EndOfYear().Kind);
    }

    [Theory]
    [InlineData(2026, 3, 6, false)]
    [InlineData(2026, 3, 7, true)]
    [InlineData(2026, 3, 8, true)]
    [InlineData(2026, 3, 9, false)]
    public void IsWeekend_ShouldDetectSaturdayAndSunday(int y, int m, int d, bool expected)
    {
        Assert.Equal(expected, new DateTime(y, m, d).IsWeekend());
        Assert.Equal(expected, new DateOnly(y, m, d).IsWeekend());
    }

    [Theory]
    [InlineData(1, true)]
    [InlineData(5, true)]
    [InlineData(10, true)]
    [InlineData(0, false)]
    [InlineData(11, false)]
    public void IsBetween_ShouldBeInclusive(int day, bool expected)
    {
        Assert.Equal(expected, new DateTime(2026, 3, day == 0 ? 1 : day, 0, 0, 0).AddTicks(day == 0 ? -1 : 0).IsBetween(new DateTime(2026, 3, 1), new DateTime(2026, 3, 10)));
        if (day is >= 1 and <= 11)
            Assert.Equal(expected, new DateOnly(2026, 3, day).IsBetween(new DateOnly(2026, 3, 1), new DateOnly(2026, 3, 10)));
    }

    [Fact]
    public void IsBetween_StartAfterEnd_ShouldThrow()
    {
        Assert.Throws<ArgumentException>(() => new DateTime(2026, 3, 5).IsBetween(new DateTime(2026, 3, 10), new DateTime(2026, 3, 1)));
        Assert.Throws<ArgumentException>(() => new DateOnly(2026, 3, 5).IsBetween(new DateOnly(2026, 3, 10), new DateOnly(2026, 3, 1)));
    }

    [Theory]
    [InlineData(1990, 5, 20, 2026, 5, 20, 36)]
    [InlineData(1990, 5, 20, 2026, 5, 19, 35)]
    [InlineData(1990, 5, 20, 2026, 5, 21, 36)]
    [InlineData(1990, 5, 20, 1990, 5, 20, 0)]
    [InlineData(2000, 2, 29, 2026, 2, 28, 25)]
    [InlineData(2000, 2, 29, 2026, 3, 1, 26)]
    [InlineData(2000, 2, 29, 2028, 2, 28, 27)]
    [InlineData(2000, 2, 29, 2028, 2, 29, 28)]
    public void Age_ShouldCountCompletedYears(int by, int bm, int bd, int ay, int am, int ad, int expected)
    {
        Assert.Equal(expected, new DateTime(by, bm, bd).Age(new DateTime(ay, am, ad)));
        Assert.Equal(expected, new DateOnly(by, bm, bd).Age(new DateOnly(ay, am, ad)));
    }

    [Fact]
    public void Age_ReferenceBeforeBirth_ShouldThrow()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new DateTime(2000, 1, 2).Age(new DateTime(2000, 1, 1)));
        Assert.Throws<ArgumentOutOfRangeException>(() => new DateOnly(2000, 1, 2).Age(new DateOnly(2000, 1, 1)));
    }
}
