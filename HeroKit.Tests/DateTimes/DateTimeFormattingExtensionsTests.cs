using System.Globalization;
using HeroKit.DateTimes;

namespace HeroKit.Tests.DateTimes;

public class DateTimeFormattingExtensionsTests
{
    private static readonly DateTime Sample = new(2026, 3, 7, 14, 5, 9);

    [Fact]
    public void ToTrString_ShouldUseTurkishCulture()
    {
#pragma warning disable CS0618 // obsolete shortcut stays covered until removal
        Assert.Equal("07 Mar 2026, Cumartesi", Sample.ToTrString());
#pragma warning restore CS0618
    }

    [Fact]
    public void ToCultureString_ShouldUseGivenCulture()
    {
        Assert.Equal("07 Mar 2026, Saturday", Sample.ToCultureString(new CultureInfo("en-US")));
    }

    [Fact]
    public void AsDirectoryName_ShouldUseIsoDate()
    {
        Assert.Equal("2026-03-07", Sample.AsDirectoryName());
    }

    [Theory]
    [InlineData("", "2026-03-07-14-05-09")]
    [InlineData("Monthly Report", "monthly-report-2026-03-07-14-05-09")]
    public void AsFileName_ShouldAppendTimestamp(string baseName, string expected)
    {
        Assert.Equal(expected, Sample.AsFileName(baseName));
    }
}
