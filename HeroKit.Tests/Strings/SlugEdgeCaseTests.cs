namespace HeroKit.Tests.Strings;

public class SlugEdgeCaseTests
{
    [Theory]
    [InlineData("  ---  ", "")]
    [InlineData("- Hello -", "hello")]
    [InlineData("--Çağdaş--Şehir--", "cagdas-sehir")]
    [InlineData("!!!", "")]
    [InlineData("a - b", "a-b")]
    public void ToSlug_ShouldNotStartOrEndWithDash(string value, string expected)
    {
        Assert.Equal(expected, value.ToSlug());
    }
}
