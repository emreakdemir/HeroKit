using HeroKit.Globalization;
using HeroKit.Globalization.Turkey;

namespace HeroKit.Tests.Turkish;

public class TurkeyIdentityExtensionsTests
{
    [Theory]
    [InlineData("10000000146", true)]
    [InlineData("10000000147", false)] // wrong 11th digit
    [InlineData("10000000156", false)] // wrong 10th digit
    [InlineData("00000000146", false)] // starts with 0
    [InlineData("1000000014", false)]  // 10 digits
    [InlineData("100000001460", false)] // 12 digits
    [InlineData("1000000014a", false)]
    [InlineData("10000 00146", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void IsValidTckn_ShouldValidateChecksum(string? value, bool expected)
    {
        Assert.Equal(expected, value.IsValidTckn());
    }

    [Theory]
    [InlineData("4840847211", true)]
    [InlineData("1234567890", true)]
    [InlineData("9999999994", true)]
    [InlineData("1000000000", true)]
    [InlineData("4840847212", false)]
    [InlineData("1234567891", false)]
    [InlineData("484084721", false)]
    [InlineData("48408472110", false)]
    [InlineData("484084721a", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void IsValidVkn_ShouldValidateChecksum(string? value, bool expected)
    {
        Assert.Equal(expected, value.IsValidVkn());
    }

    [Theory]
    [InlineData("4840847211", true)]
    [InlineData("10000000146", true)]
    [InlineData("4840847212", false)]
    [InlineData("10000000147", false)]
    [InlineData("123", false)]
    [InlineData(null, false)]
    public void IsValidTaxIdTr_ShouldAcceptVknOrTckn(string? value, bool expected)
    {
        Assert.Equal(expected, value.IsValidTaxId(Countries.Turkey));
    }

    [Theory]
    [InlineData("10000000146", true)]
    [InlineData("10000000147", false)]
    [InlineData("4840847211", false)]
    [InlineData("01000000146", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void IsValidNationalId_ShouldValidateTckn(string? value, bool expected)
    {
        Assert.Equal(expected, value.IsValidNationalId(Countries.Turkey));
    }

    [Fact]
    public void IsValidTckn_ShouldAcceptEveryNumberBuiltWithTheAlgorithm()
    {
        var random = new Random(42);

        for (int n = 0; n < 1000; n++)
        {
            int[] digits = new int[11];
            digits[0] = random.Next(1, 10);
            for (int i = 1; i < 9; i++)
                digits[i] = random.Next(0, 10);

            int odd = digits[0] + digits[2] + digits[4] + digits[6] + digits[8];
            int even = digits[1] + digits[3] + digits[5] + digits[7];
            digits[9] = ((odd * 7 - even) % 10 + 10) % 10;
            digits[10] = (odd + even + digits[9]) % 10;

            string tckn = string.Concat(digits);
            Assert.True(tckn.IsValidTckn(), tckn);

            string broken = tckn[..10] + (char)('0' + (digits[10] + 1) % 10);
            Assert.False(broken.IsValidTckn(), broken);
        }
    }
}
