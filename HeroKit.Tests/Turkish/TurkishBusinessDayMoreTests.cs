using HeroKit.Turkish;

namespace HeroKit.Tests.Turkish;

public class TurkishBusinessDayMoreTests
{
    [Fact]
    public void IsPublicHolidayTr_DateTime_ShouldIgnoreTimePart()
    {
        Assert.True(new DateTime(2026, 10, 29, 15, 30, 0).IsPublicHolidayTr());
    }

    [Fact]
    public void IsPublicHolidayTr_DateTime_ShouldReturnFalse_ForOrdinaryDay()
    {
        Assert.False(new DateTime(2026, 10, 27, 9, 0, 0).IsPublicHolidayTr());
    }

    [Fact]
    public void IsHalfDayHolidayTr_ShouldReturnTrue_ForRepublicDayEve()
    {
        Assert.True(new DateOnly(2026, 10, 28).IsHalfDayHolidayTr());
    }

    [Fact]
    public void IsHalfDayHolidayTr_ShouldReturnFalse_ForFullHoliday()
    {
        Assert.False(new DateOnly(2026, 10, 29).IsHalfDayHolidayTr());
    }

    [Fact]
    public void GetPublicHolidayTr_ShouldReturnHoliday_OnHolidayDate()
    {
        Assert.NotNull(new DateOnly(2026, 10, 29).GetPublicHolidayTr());
    }

    [Fact]
    public void GetPublicHolidayTr_ShouldReturnNull_OnOrdinaryDay()
    {
        Assert.Null(new DateOnly(2026, 10, 27).GetPublicHolidayTr());
    }

    [Fact]
    public void IsBusinessDayTr_DateTime_ShouldReturnFalse_OnWeekend()
    {
        Assert.False(new DateTime(2026, 10, 31, 12, 0, 0).IsBusinessDayTr());
    }

    [Fact]
    public void IsBusinessDayTr_DateTime_ShouldReturnTrue_OnWeekday()
    {
        Assert.True(new DateTime(2026, 10, 27, 12, 0, 0).IsBusinessDayTr());
    }

    [Fact]
    public void AddBusinessDaysTr_ShouldSkipWeekendAndHoliday_WhenMovingForward()
    {
        DateOnly result = new DateOnly(2026, 10, 28).AddBusinessDaysTr(1);

        Assert.Equal(new DateOnly(2026, 10, 30), result);
    }

    [Fact]
    public void AddBusinessDaysTr_ShouldMoveBackward_WithNegativeDays()
    {
        DateOnly result = new DateOnly(2026, 10, 30).AddBusinessDaysTr(-1);

        Assert.Equal(new DateOnly(2026, 10, 28), result);
    }

    [Fact]
    public void AddBusinessDaysTr_ShouldReturnSameDate_ForZero()
    {
        Assert.Equal(new DateOnly(2026, 10, 31), new DateOnly(2026, 10, 31).AddBusinessDaysTr(0));
    }

    [Fact]
    public void AddBusinessDaysTr_DateTime_ShouldKeepTimeAndKind()
    {
        var start = new DateTime(2026, 10, 27, 14, 45, 10, DateTimeKind.Utc);

        DateTime result = start.AddBusinessDaysTr(1);

        Assert.Equal(new DateTime(2026, 10, 28, 14, 45, 10, DateTimeKind.Utc), result);
        Assert.Equal(DateTimeKind.Utc, result.Kind);
    }

    [Fact]
    public void BusinessDaysUntilTr_ShouldCountForward()
    {
        Assert.Equal(1, new DateOnly(2026, 10, 27).BusinessDaysUntilTr(new DateOnly(2026, 10, 28)));
    }

    [Fact]
    public void BusinessDaysUntilTr_ShouldNegate_WhenEndIsBeforeStart()
    {
        Assert.Equal(-1, new DateOnly(2026, 10, 28).BusinessDaysUntilTr(new DateOnly(2026, 10, 27)));
    }

    [Fact]
    public void BusinessDaysUntilTr_ShouldReturnZero_ForSameDay()
    {
        Assert.Equal(0, new DateOnly(2026, 10, 27).BusinessDaysUntilTr(new DateOnly(2026, 10, 27)));
    }
}
