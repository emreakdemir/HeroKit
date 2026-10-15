using HeroKit.Comparers;

namespace HeroKit.Tests.Comparers;

public class NameEqualityComparerTests
{
    [Theory]
    [InlineData("Hero", "hero", true)]
    [InlineData("HERO", "hero", true)]
    [InlineData("Hero", "Kit", false)]
    [InlineData(null, null, true)]
    [InlineData("Hero", null, false)]
    public void Equals_IgnoresCase(string source, string control, bool expected)
    {
        Assert.Equal(expected, NameEqualityComparer.Default.Equals(source, control));
    }

    [Fact]
    public void GetHashCode_IsConsistentWithEquals()
    {
        var comparer = NameEqualityComparer.Default;
        Assert.Equal(comparer.GetHashCode("Hero"), comparer.GetHashCode("HERO"));
        Assert.Equal(0, comparer.GetHashCode(null));
    }

    [Fact]
    public void Dictionary_FindsKeyRegardlessOfCase()
    {
        var dictionary = new Dictionary<string, int>(NameEqualityComparer.Default) { ["Hero"] = 1 };
        Assert.True(dictionary.ContainsKey("HERO"));
        Assert.Equal(1, dictionary["hero"]);
    }
}
