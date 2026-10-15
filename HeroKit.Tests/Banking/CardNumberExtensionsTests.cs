using HeroKit.Banking;

namespace HeroKit.Tests.Banking;

public class CardNumberExtensionsTests
{
    private static string TroyNumber()
    {
        string partial = "979200000000001";
        int sum = 0;
        bool doubleIt = true;

        for (int i = partial.Length - 1; i >= 0; i--)
        {
            int digit = partial[i] - '0';
            if (doubleIt)
            {
                digit *= 2;
                if (digit > 9)
                    digit -= 9;
            }

            sum += digit;
            doubleIt = !doubleIt;
        }

        return partial + (10 - sum % 10) % 10;
    }

    [Fact]
    public void TroyNumber_IsLuhnValidAndTroy()
    {
        string troy = TroyNumber();
        Assert.Equal(16, troy.Length);
        Assert.True(troy.IsValidCardNumber());
        Assert.Equal(CardBrand.Troy, troy.GetCardBrand());
    }

    [Theory]
    [InlineData("4111111111111111", true)]
    [InlineData("4111 1111 1111 1111", true)]
    [InlineData("4111-1111-1111-1111", true)]
    [InlineData("5555555555554444", true)]
    [InlineData("2223003122003222", true)]
    [InlineData("378282246310005", true)]
    [InlineData("6011111111111117", true)]
    [InlineData("3530111333300000", true)]
    [InlineData("4111111111111112", false)]
    [InlineData("411111111111", false)]
    [InlineData("4111111111", false)]
    [InlineData("41111111111111111111", false)]
    [InlineData("4111a11111111111", false)]
    [InlineData("4111.1111.1111.1111", false)]
    [InlineData("", false)]
    [InlineData("   ", false)]
    [InlineData(null, false)]
    public void IsValidCardNumber(string? value, bool expected) =>
        Assert.Equal(expected, value.IsValidCardNumber());

    [Theory]
    [InlineData("4111111111111111", CardBrand.Visa)]
    [InlineData("4111 1111 1111 1111", CardBrand.Visa)]
    [InlineData("4222222222222", CardBrand.Visa)]
    [InlineData("4111111111111111111", CardBrand.Visa)]
    [InlineData("411111111111111", CardBrand.Unknown)]
    [InlineData("5555555555554444", CardBrand.Mastercard)]
    [InlineData("5105105105105100", CardBrand.Mastercard)]
    [InlineData("2223003122003222", CardBrand.Mastercard)]
    [InlineData("2720999999999999", CardBrand.Mastercard)]
    [InlineData("2221000000000000", CardBrand.Mastercard)]
    [InlineData("2220999999999999", CardBrand.Unknown)]
    [InlineData("2721000000000000", CardBrand.Unknown)]
    [InlineData("378282246310005", CardBrand.AmericanExpress)]
    [InlineData("371449635398431", CardBrand.AmericanExpress)]
    [InlineData("3782822463100050", CardBrand.Unknown)]
    [InlineData("6011111111111117", CardBrand.Discover)]
    [InlineData("6440000000000000", CardBrand.Discover)]
    [InlineData("6500000000000000", CardBrand.Discover)]
    [InlineData("6011111111111117000", CardBrand.Discover)]
    [InlineData("3530111333300000", CardBrand.Jcb)]
    [InlineData("3528000000000000", CardBrand.Jcb)]
    [InlineData("3589000000000000", CardBrand.Jcb)]
    [InlineData("3590000000000000", CardBrand.Unknown)]
    [InlineData("9792000000000000", CardBrand.Troy)]
    [InlineData("97920000000000", CardBrand.Unknown)]
    [InlineData("1234567890123456", CardBrand.Unknown)]
    [InlineData("4111a11111111111", CardBrand.Unknown)]
    [InlineData("", CardBrand.Unknown)]
    [InlineData(null, CardBrand.Unknown)]
    public void GetCardBrand(string? value, CardBrand expected) =>
        Assert.Equal(expected, value.GetCardBrand());
}
