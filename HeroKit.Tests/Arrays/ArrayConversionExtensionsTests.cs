using HeroKit.Arrays;

namespace HeroKit.Tests.Arrays;

public class ArrayConversionExtensionsTests
{
    [Theory]
    [InlineData(new byte[] { 251, 255 }, "-_8")]
    [InlineData(new byte[] { 1, 2, 3, 4, 5 }, "AQIDBAU")]
    [InlineData(new byte[] { 72, 101, 114, 111 }, "SGVybw")]
    [InlineData(new byte[0], "")]
    public void ToBase64Url_ShouldUseUrlSafeAlphabetWithoutPadding(byte[] value, string expected)
    {
        Assert.Equal(expected, value.ToBase64Url());
    }

    [Fact]
    public void ToBase64Url_ShouldReturnEmptyForNull()
    {
        byte[]? value = null;

        Assert.Equal(string.Empty, value.ToBase64Url());
    }
}
