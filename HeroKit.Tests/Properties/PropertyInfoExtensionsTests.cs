using HeroKit.Properties;

namespace HeroKit.Tests.Properties;

public class PropertyInfoExtensionsTests
{
    private sealed class Sample
    {
        public int Number { get; set; }
        public int? OptionalNumber { get; set; }
        public string? Text { get; set; }
    }

    [Fact]
    public void IsNullable_ShouldDetectNullableValueTypes()
    {
        Assert.False(typeof(Sample).GetProperty(nameof(Sample.Number))!.IsNullable());
        Assert.True(typeof(Sample).GetProperty(nameof(Sample.OptionalNumber))!.IsNullable());
    }

    [Fact]
    public void CanAssignValue_ShouldCheckType()
    {
        var number = typeof(Sample).GetProperty(nameof(Sample.Number))!;

        Assert.True(number.CanAssignValue(5));
        Assert.False(number.CanAssignValue("5"));
        Assert.False(number.CanAssignValue(null));
    }

    [Fact]
    public void TrySetConvertedValue_ShouldConvertAndAssign()
    {
        var sample = new Sample();

        Assert.True(typeof(Sample).GetProperty(nameof(Sample.Number))!.TrySetConvertedValue(sample, "42"));
        Assert.True(typeof(Sample).GetProperty(nameof(Sample.Text))!.TrySetConvertedValue(sample, "hello"));

        Assert.Equal(42, sample.Number);
        Assert.Equal("hello", sample.Text);
    }

    [Fact]
    public void TrySetConvertedValue_ShouldReturnFalseWhenConversionFails()
    {
        var sample = new Sample { Number = 7 };

        Assert.False(typeof(Sample).GetProperty(nameof(Sample.Number))!.TrySetConvertedValue(sample, "not a number"));
        Assert.Equal(7, sample.Number);
    }

    [Fact]
    public void SetValue_ShouldStillWorkWhenCalledStatically()
    {
        var sample = new Sample();

#pragma warning disable CS0618
        PropertyInfoExtensions.SetValue(typeof(Sample).GetProperty(nameof(Sample.Number))!, sample, "42");
#pragma warning restore CS0618

        Assert.Equal(42, sample.Number);
    }
}
