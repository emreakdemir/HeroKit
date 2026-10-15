using HeroKit.Properties;

namespace HeroKit.Tests.Properties;

public class PropertyInfoExtensionsTests
{
    private sealed class Sample
    {
        public int Number { get; set; }
        public int? OptionalNumber { get; set; }
        public string Text { get; set; }
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
    public void SetValue_ShouldConvertAndAssign()
    {
        var sample = new Sample();

        // Called statically: PropertyInfo.SetValue(object, object) hides the extension in instance syntax.
        PropertyInfoExtensions.SetValue(typeof(Sample).GetProperty(nameof(Sample.Number))!, sample, "42");
        PropertyInfoExtensions.SetValue(typeof(Sample).GetProperty(nameof(Sample.Text))!, sample, "hello");

        Assert.Equal(42, sample.Number);
        Assert.Equal("hello", sample.Text);
    }
}
