namespace HeroKit.Tests.Booleans;

public class BooleanControlExtensionsTests
{
    [Fact]
    public void IsTrue_NullableTrue_ReturnsTrue()
    {
        bool? value = true;
        bool result = value.IsTrue();
        Assert.True(result);
    }

    [Fact]
    public void IsTrue_NullableFalse_ReturnsFalse()
    {
        bool? value = false;
        bool result = value.IsTrue();
        Assert.False(result);
    }

    [Fact]
    public void IsTrue_NullValue_ReturnsFalse()
    {
        bool? value = null;
        bool result = value.IsTrue();
        Assert.False(result);
    }

    [Fact]
    public void IsFalse_NullableTrue_ReturnsFalse()
    {
        bool? value = true;
        bool result = value.IsFalse();
        Assert.False(result);
    }

    [Fact]
    public void IsFalse_NullableFalse_ReturnsTrue()
    {
        bool? value = false;
        bool result = value.IsFalse();
        Assert.True(result);
    }

    [Fact]
    public void IsFalse_NullValue_ReturnsTrue()
    {
        bool? value = null;
        bool result = value.IsFalse();
        Assert.True(result);
    }

    [Fact]
    public void AllTrue_AllTrueValues_ReturnsTrue()
    {
        IEnumerable<bool> values = new[] { true, true, true };
        bool result = values.AllTrue();
        Assert.True(result);
    }

    [Fact]
    public void AllTrue_MixedValues_ReturnsFalse()
    {
        IEnumerable<bool> values = new[] { true, false, true };
        bool result = values.AllTrue();
        Assert.False(result);
    }

    [Fact]
    public void AnyTrue_AnyTrueValue_ReturnsTrue()
    {
        IEnumerable<bool> values = new[] { false, false, true };
        bool result = values.AnyTrue();
        Assert.True(result);
    }

    [Fact]
    public void AnyTrue_AllFalseValues_ReturnsFalse()
    {
        IEnumerable<bool> values = new[] { false, false, false };
        bool result = values.AnyTrue();
        Assert.False(result);
    }

    [Fact]
    public void AllFalse_AllFalseValues_ReturnsTrue()
    {
        IEnumerable<bool> values = new[] { false, false, false };
        bool result = values.AllFalse();
        Assert.True(result);
    }

    [Fact]
    public void AllFalse_MixedValues_ReturnsFalse()
    {
        IEnumerable<bool> values = new[] { false, true, false };
        bool result = values.AllFalse();
        Assert.False(result);
    }

    [Fact]
    public void AnyFalse_AnyFalseValue_ReturnsTrue()
    {
        IEnumerable<bool> values = new[] { true, true, false };
        bool result = values.AnyFalse();
        Assert.True(result);
    }

    [Fact]
    public void AnyFalse_AllTrueValues_ReturnsFalse()
    {
        IEnumerable<bool> values = new[] { true, true, true };
        bool result = values.AnyFalse();
        Assert.False(result);
    }
}