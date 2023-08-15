namespace HeroKit.Tests.Strings;

public class StringManipulationExtensionsTests
{
    [Theory]
    [InlineData(null, "")]
    [InlineData("", "")]
    [InlineData("Héllo", "hello")]
    [InlineData("Türkiye İstanbul", "turkiye-istanbul")]
    public void ToSlug_ShouldConvertStringToSlug(string input, string expectedOutput)
    {
        // Arrange

        // Act
        string result = input.ToSlug();

        // Assert
        Assert.Equal(expectedOutput, result);
    }
    
    [Theory]
    [InlineData("Café", "Cafe")]
    [InlineData("über", "uber")]
    [InlineData("résümé", "resume")]
    [InlineData("áéíóú", "aeiou")]
    [InlineData("こんにちは", "こんにちは")]
    [InlineData("", "")]
    [InlineData("12345", "12345")]
    public void RemoveAccent_ShouldRemoveAccents(string input, string expectedOutput)
    {
        // Arrange

        // Act
        string result = input.RemoveAccent();

        // Assert
        Assert.Equal(expectedOutput, result);
    }

    [Theory]
    [InlineData(null, "")]
    [InlineData("", "")]
    [InlineData("ığüşöçİĞÜŞÖÇ", "igusocIGUSOC")]
    public void ClearTurkishChars_ShouldReplaceTurkishCharsWithNonAccentedEquivalents(string input, string expectedOutput)
    {
        // Arrange

        // Act
        string result = input.ClearTurkishChars();

        // Assert
        Assert.Equal(expectedOutput, result);
    }
}