using HeroKit.Guids;

namespace HeroKit.Tests.Guids;

public class GuidExtensionsTests
{
    [Fact]
    public void IsEmpty_ShouldDetectEmptyGuid()
    {
        Assert.True(Guid.Empty.IsEmpty());
        Assert.False(Guid.NewGuid().IsEmpty());
        Assert.True(Guid.NewGuid().IsNotEmpty());
    }

    [Fact]
    public void IsNullOrEmpty_ShouldHandleNullableGuid()
    {
        Guid? nullGuid = null;
        Guid? emptyGuid = Guid.Empty;
        Guid? value = Guid.NewGuid();

        Assert.True(nullGuid.IsNullOrEmpty());
        Assert.True(emptyGuid.IsNullOrEmpty());
        Assert.False(value.IsNullOrEmpty());
        Assert.True(value.IsNotNullOrEmpty());
    }
}
