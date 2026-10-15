using HeroKit.Globalization;
using HeroKit.Globalization.Turkey;

namespace HeroKit.Tests.Turkish;

public class EInvoiceTests
{
    [Theory]
    [InlineData("ABC2026000000001", true)]
    [InlineData("GIB2023123456789", true)]
    [InlineData("A1B2026999999999", true)]
    [InlineData("ABC2026000000000", false)]  // sequence starts at 1
    [InlineData("abc2026000000001", false)]  // series must be upper case
    [InlineData("ÇAB2026000000001", false)]  // Turkish letters are not allowed
    [InlineData("ABC1999000000001", false)]  // year out of range
    [InlineData("ABC202600000001", false)]   // 15 characters
    [InlineData("ABC20260000000011", false)] // 17 characters
    [InlineData("ABC2026 00000001", false)]
    [InlineData("ABC20260000000A1", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void IsValidGibInvoiceNumber_ShouldFollowTheGibFormat(string? value, bool expected)
    {
        Assert.Equal(expected, value.IsValidGibInvoiceNumber());
    }

    [Fact]
    public void TryParse_ShouldSplitTheParts()
    {
        Assert.True(GibInvoiceNumber.TryParse("ABC2026000000042", out GibInvoiceNumber number));
        Assert.Equal(new GibInvoiceNumber("ABC", 2026, 42), number);
        Assert.Equal("ABC2026000000042", number.ToString());
    }

    [Fact]
    public void Create_And_Next_ShouldBuildSequentialNumbers()
    {
        GibInvoiceNumber first = GibInvoiceNumber.Create("ABC", 2026, 1);

        Assert.Equal("ABC2026000000001", first.ToString());
        Assert.Equal("ABC2026000000002", first.Next().ToString());
        Assert.Throws<InvalidOperationException>(() => GibInvoiceNumber.Create("ABC", 2026, GibInvoiceNumber.MaxSequence).Next());
    }

    [Theory]
    [InlineData("AB", 2026, 1)]
    [InlineData("abc", 2026, 1)]
    [InlineData("ABC", 1999, 1)]
    [InlineData("ABC", 2026, 0)]
    public void Create_ShouldRejectInvalidParts(string series, int year, int sequence)
    {
        Assert.ThrowsAny<ArgumentException>(() => GibInvoiceNumber.Create(series, year, sequence));
    }

    [Theory]
    [InlineData("51d3bb18-9650-4c8e-825d-5a7f845be630", true)]
    [InlineData("51D3BB18-9650-4C8E-825D-5A7F845BE630", true)]
    [InlineData("00000000-0000-0000-0000-000000000000", false)]
    [InlineData("51d3bb1896504c8e825d5a7f845be630", false)]  // hyphens are required
    [InlineData("{51d3bb18-9650-4c8e-825d-5a7f845be630}", false)]
    [InlineData("51d3bb18-9650-4c8e-825d-5a7f845be63", false)]
    [InlineData(null, false)]
    public void IsValidEttn_ShouldAcceptHyphenatedUuids(string? value, bool expected)
    {
        Assert.Equal(expected, value.IsValidEttn());
    }
}

