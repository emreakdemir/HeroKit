using HeroKit.Turkish;

namespace HeroKit.Tests.Turkish;

public class TurkishPhoneExtensionsTests
{
    [Theory]
    [InlineData("0532 123 45 67", "+905321234567")]
    [InlineData("+90 532 123 45 67", "+905321234567")]
    [InlineData("905321234567", "+905321234567")]
    [InlineData("00905321234567", "+905321234567")]
    [InlineData("(0212) 555 12 34", "+902125551234")]
    [InlineData("5321234567", "+905321234567")]
    [InlineData("0532-123-45-67", "+905321234567")]
    [InlineData("0532.123.45.67", "+905321234567")]
    [InlineData("0032 532 123 45 67", null)]
    [InlineData("532123456", null)]
    [InlineData("05321234", null)]
    [InlineData("53212345678", null)]
    [InlineData("0532123456789", null)]
    [InlineData("1321234567", "+901321234567")] // structural only: ToE164Tr does not check the first digit
    [InlineData("01321234567", "+901321234567")]
    [InlineData("00532123456", null)]
    [InlineData("+9001321234567", null)]
    [InlineData("0532 abc 45 67", null)]
    [InlineData("+44 532 123 45 67", null)]
    [InlineData("", null)]
    [InlineData(null, null)]
    public void ToE164Tr_ReturnsExpected(string? value, string? expected) => Assert.Equal(expected, value.ToE164Tr());

    [Theory]
    [InlineData("+90 532 123 45 67", "0532 123 45 67")]
    [InlineData("5321234567", "0532 123 45 67")]
    [InlineData("(0212) 555 12 34", "0212 555 12 34")]
    [InlineData("00905321234567", "0532 123 45 67")]
    [InlineData("12345", null)]
    [InlineData("", null)]
    [InlineData(null, null)]
    public void FormatPhoneTr_ReturnsExpected(string? value, string? expected) => Assert.Equal(expected, value.FormatPhoneTr());

    [Theory]
    [InlineData("0532 123 45 67", true)]
    [InlineData("+90 532 123 45 67", true)]
    [InlineData("905321234567", true)]
    [InlineData("00905321234567", true)]
    [InlineData("(0212) 555 12 34", true)]
    [InlineData("0312 555 12 34", true)]
    [InlineData("0412 555 12 34", true)]
    [InlineData("5321234567", true)]
    [InlineData("0612 555 12 34", false)]
    [InlineData("0812 555 12 34", false)]
    [InlineData("0132 123 45 67", false)]
    [InlineData("0032 532 123 45 67", false)]
    [InlineData("532123456", false)]
    [InlineData("53212345678", false)]
    [InlineData("0532 abc 45 67", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void IsValidPhoneTr_ReturnsExpected(string? value, bool expected) => Assert.Equal(expected, value.IsValidPhoneTr());

    [Theory]
    [InlineData("0532 123 45 67", true)]
    [InlineData("+90 532 123 45 67", true)]
    [InlineData("5321234567", true)]
    [InlineData("(0212) 555 12 34", false)]
    [InlineData("0312 555 12 34", false)]
    [InlineData("0132 123 45 67", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void IsValidMobileTr_ReturnsExpected(string? value, bool expected) => Assert.Equal(expected, value.IsValidMobileTr());
}
