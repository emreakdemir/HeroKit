using HeroKit.Arrays;

namespace HeroKit.Tests.Arrays;

using Xunit;

public class ArrayControlExtensionsTests
{
    [Theory]
    [InlineData(null, true)]
    [InlineData(new int[] { }, true)]
    [InlineData(new[] { 1 }, false)]
    [InlineData(new[] { 1, 2, 3 }, false)]
    [InlineData(new object[] { null, null }, false)]
    [InlineData(new object[] { null, "Test" }, false)]
    [InlineData(new string[] { "Test", "Test2" }, false)]
    public void IsNullOrEmptyTests<T>(T[] array, bool expected)
    {
        Assert.Equal(expected, array.IsNullOrEmpty());
    }

    [Theory]
    [InlineData(null, false)]
    [InlineData(new int[] { }, false)]
    [InlineData(new[] { 1 }, true)]
    [InlineData(new[] { 1, 2, 3 }, true)]
    [InlineData(new object[] { null, null }, true)]
    [InlineData(new object[] { null, "Test" }, true)]
    [InlineData(new string[] { "Test", "Test2" }, true)]
    public void IsNotNullOrEmptyTestsForInt<T>(T[] array, bool expected)
    {
        Assert.Equal(expected, array.IsNotNullOrEmpty());
    }
}