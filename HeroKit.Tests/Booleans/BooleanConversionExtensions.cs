namespace HeroKit.Tests.Booleans;

public class BooleanConversionExtensions
{
    [Theory]
    [InlineData(true, "Yes")]
    [InlineData(false, "No")]
    public void ToYesNo_BooleanValue_ReturnsExpectedString(bool value, string expected)
    {
        string result = value.ToYesNo();
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(true, "Yes")]
    [InlineData(false, "No")]
    [InlineData(null, "No")]
    public void ToYesNo_NullableBooleanValue_ReturnsExpectedString(bool? value, string expected)
    {
        string result = value.ToYesNo();
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(true, 1)]
    [InlineData(false, 0)]
    public void ToInt_BooleanValue_ReturnsExpectedIntValue(bool value, int expected)
    {
        int result = value.ToInt();
        Assert.Equal(expected, result);
    }
}