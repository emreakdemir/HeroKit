namespace HeroKit.Tests.Types;

public class TypeControlExtensionsMoreTests
{
    private interface IMarker<T>;

    private class Base<T>;

    private sealed class Derived : Base<int>, IMarker<string>;

    [Fact]
    public void IsNullable_ShouldThrow_ForNullType()
    {
        Type type = null!;

        Assert.Throws<ArgumentNullException>(() => type.IsNullable());
    }

    [Fact]
    public void IsGenericAssignableFrom_ShouldThrow_ForNullTarget()
    {
        Type toType = null!;

        Assert.Throws<ArgumentNullException>(() => toType.IsGenericAssignableFrom(typeof(Derived), out _));
    }

    [Fact]
    public void IsGenericAssignableFrom_ShouldThrow_ForNullSource()
    {
        Assert.Throws<ArgumentNullException>(() => typeof(Base<>).IsGenericAssignableFrom(null!, out _));
    }

    [Theory]
    [InlineData(typeof(int))]
    [InlineData(typeof(Base<int>))]
    public void IsGenericAssignableFrom_ShouldReturnFalse_WhenTargetIsNotGenericDefinition(Type toType)
    {
        bool result = toType.IsGenericAssignableFrom(typeof(Derived), out Type[]? arguments);

        Assert.False(result);
        Assert.Null(arguments);
    }

    [Fact]
    public void IsGenericAssignableFrom_ShouldReturnFalse_WhenSourceIsGenericDefinition()
    {
        bool result = typeof(Base<>).IsGenericAssignableFrom(typeof(Base<>), out Type[]? arguments);

        Assert.False(result);
        Assert.Null(arguments);
    }

    [Fact]
    public void IsGenericAssignableFrom_ShouldReturnArguments_ForBaseClass()
    {
        bool result = typeof(Base<>).IsGenericAssignableFrom(typeof(Derived), out Type[]? arguments);

        Assert.True(result);
        Assert.Equal([typeof(int)], arguments);
    }

    [Fact]
    public void IsGenericAssignableFrom_ShouldReturnArguments_ForInterface()
    {
        bool result = typeof(IMarker<>).IsGenericAssignableFrom(typeof(Derived), out Type[]? arguments);

        Assert.True(result);
        Assert.Equal([typeof(string)], arguments);
    }

    [Fact]
    public void IsGenericAssignableFrom_ShouldReturnFalse_WhenBaseClassIsNotInHierarchy()
    {
        bool result = typeof(List<>).IsGenericAssignableFrom(typeof(Derived), out Type[]? arguments);

        Assert.False(result);
        Assert.Null(arguments);
    }

    [Fact]
    public void IsGenericAssignableFrom_ShouldReturnFalse_WhenInterfaceIsNotImplemented()
    {
        bool result = typeof(IEnumerable<>).IsGenericAssignableFrom(typeof(Derived), out Type[]? arguments);

        Assert.False(result);
        Assert.Null(arguments);
    }

    [Theory]
    [InlineData(typeof(int?), typeof(int))]
    [InlineData(typeof(DateTime?), typeof(DateTime))]
    [InlineData(typeof(int), typeof(int))]
    [InlineData(typeof(string), typeof(string))]
    [InlineData(typeof(List<int>), typeof(List<int>))]
    public void GetUnderlyingType_ShouldUnwrapOnlyNullable(Type input, Type expected)
    {
        Assert.Equal(expected, input.GetUnderlyingType());
    }

    [Theory]
    [InlineData(0, true)]
    [InlineData(5, false)]
    public void IsDefaultValue_ShouldCompareToDefault_ForValueType(int value, bool expected)
    {
        Assert.Equal(expected, value.IsDefaultValue());
    }

    [Fact]
    public void IsDefaultValue_ShouldReturnTrue_ForNullReference()
    {
        string? value = null;

        Assert.True(value.IsDefaultValue());
    }

    [Fact]
    public void IsDefaultValue_ShouldReturnFalse_ForNonNullReference()
    {
        Assert.False("text".IsDefaultValue());
    }
}
