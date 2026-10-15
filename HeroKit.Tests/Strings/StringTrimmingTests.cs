namespace HeroKit.Tests.Strings;

public class StringTrimmingTests
{
    [Theory]
    [InlineData("HeroKit extensions", 10, "…", "HeroKit e…")]
    [InlineData("HeroKit", 10, "…", "HeroKit")]
    [InlineData("HeroKit", 7, "…", "HeroKit")]
    [InlineData("HeroKit extensions", 10, "...", "HeroKit...")]
    [InlineData("HeroKit", 3, "", "Her")]
    [InlineData("abc", 1, "…", "…")]
    [InlineData(null, 5, "…", null)]
    public void Truncate_ShouldRespectMaxLength(string? value, int maxLength, string ellipsis, string? expected)
    {
        string? result = value.Truncate(maxLength, ellipsis);

        Assert.Equal(expected, result);
        Assert.True(result is null || result.Length <= maxLength);
    }

    [Fact]
    public void Truncate_ShouldNotSplitSurrogatePairs()
    {
        // "ab😀cd": the emoji occupies indexes 2 and 3.
        string result = "ab😀cd".Truncate(4);

        Assert.Equal("ab…", result);
        Assert.False(char.IsHighSurrogate(result[^2]));
    }

    [Fact]
    public void Truncate_ShouldRejectMaxLengthShorterThanEllipsis()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => "abc".Truncate(2, "..."));
    }

    [Theory]
    [InlineData("5321234567", 4, '*', "******4567")]
    [InlineData("0532 123 45 67", 2, '*', "**** *** ** 67")]
    [InlineData("TR33 0006 1005", 4, '#', "#### #### 1005")]
    [InlineData("abc", 4, '*', "abc")]
    [InlineData("abc", 0, '*', "***")]
    [InlineData("", 4, '*', "")]
    [InlineData(null, 4, '*', null)]
    public void Mask_ShouldHideAllButLastCharacters(string? value, int visibleCount, char maskChar, string? expected)
    {
        Assert.Equal(expected, value.Mask(visibleCount, maskChar));
    }

    [Theory]
    [InlineData("HeroKit", 4, "Hero", "oKit")]
    [InlineData("Hero", 10, "Hero", "Hero")]
    [InlineData("Hero", 0, "", "")]
    [InlineData(null, 3, null, null)]
    public void LeftAndRight_ShouldBeSafeForShortStrings(string? value, int length, string? left, string? right)
    {
        Assert.Equal(left, value.Left(length));
        Assert.Equal(right, value.Right(length));
    }

    [Fact]
    public void LeftAndRight_ShouldRejectNegativeLength()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => "a".Left(-1));
        Assert.Throws<ArgumentOutOfRangeException>(() => "a".Right(-1));
    }

    [Theory]
    [InlineData("TR33 0006\t1005", "TR3300061005")]
    [InlineData(" a\r\nb c ", "abc")]
    [InlineData("abc", "abc")]
    [InlineData("", "")]
    [InlineData(null, null)]
    public void RemoveWhitespace_ShouldRemoveAllWhitespace(string? value, string? expected)
    {
        Assert.Equal(expected, value.RemoveWhitespace());
    }

    [Fact]
    public void RemoveWhitespace_ShouldHandleLongStrings()
    {
        string value = string.Join(' ', Enumerable.Repeat("abc", 200));

        Assert.Equal(600, value.RemoveWhitespace().Length);
    }
}
