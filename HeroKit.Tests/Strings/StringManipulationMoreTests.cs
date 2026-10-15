namespace HeroKit.Tests.Strings;

public class StringManipulationMoreTests
{
    [Theory]
    [InlineData("ığüşöç", "igusoc")]
    [InlineData("İĞÜŞÖÇ", "IGUSOC")]
    [InlineData("plain ascii", "plain ascii")]
    [InlineData("", "")]
    [InlineData(null, "")]
    public void ClearTurkishChars_ShouldReplaceTurkishLetters(string? input, string expected)
    {
        Assert.Equal(expected, input.RemoveDiacritics());
    }

    [Fact]
    public void Format_ShouldInsertArguments()
    {
        Assert.Equal("Hello Emre, 3", "Hello {0}, {1}".Format("Emre", 3));
    }

    [Theory]
    [InlineData("HelloWorld", "hello-world")]
    [InlineData("helloWorldAgain", "hello-world-again")]
    [InlineData("lower", "lower")]
    [InlineData("", "")]
    public void ToDashCase_ShouldInsertDashBeforeUpperCase(string input, string expected)
    {
        Assert.Equal(expected, input.ToDashCase());
    }

    [Theory]
    [InlineData("a   b", "a-b")]
    [InlineData("a - - b", "a-b")]
    [InlineData("Hello, World!", "hello-world")]
    public void ToSlug_ShouldCollapseSeparatorsAndStripSymbols(string input, string expected)
    {
        Assert.Equal(expected, input.ToSlug());
    }

    [Fact]
    public void ToSlug_ShouldLimitLengthTo200()
    {
        Assert.Equal(200, new string('a', 500).ToSlug().Length);
    }
}
