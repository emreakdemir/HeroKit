using HeroKit.Enumerables;

namespace HeroKit.Tests.Enumerables;

public class EnumerableExtensionsTests
{
    private static IEnumerable<int> Lazy(int count, Action onElement)
    {
        for (int i = 0; i < count; i++)
        {
            onElement();
            yield return i;
        }
    }

    [Fact]
    public void IsNullOrEmpty_ShouldHandleNullEmptyAndFilled()
    {
        IEnumerable<int>? nullSequence = null;

        Assert.True(nullSequence.IsNullOrEmpty());
        Assert.True(new List<int>().IsNullOrEmpty());
        Assert.True(Enumerable.Empty<int>().IsNullOrEmpty());
        Assert.False(new HashSet<int> { 1 }.IsNullOrEmpty());
        Assert.True(new HashSet<int> { 1 }.IsNotNullOrEmpty());
    }

    [Fact]
    public void IsNullOrEmpty_ShouldReadOnlyTheFirstElementOfALazySequence()
    {
        int produced = 0;

        Assert.False(Lazy(1000, () => produced++).IsNullOrEmpty());
        Assert.Equal(1, produced);
    }

    [Fact]
    public void IsNotNullOrEmpty_ShouldNarrowNullState()
    {
        IEnumerable<int>? values = [1, 2];

        if (values.IsNotNullOrEmpty())
            Assert.Equal(2, values.Count());
    }

    [Theory]
    [InlineData(true, new[] { 2, 4 })]
    [InlineData(false, new[] { 1, 2, 3, 4 })]
    public void WhereIf_ShouldFilterOnlyWhenConditionIsTrue(bool condition, int[] expected)
    {
        int[] source = [1, 2, 3, 4];

        Assert.Equal(expected, source.WhereIf(condition, x => x % 2 == 0));
        Assert.Equal(expected, source.AsQueryable().WhereIf(condition, x => x % 2 == 0));
    }

    [Theory]
    [InlineData(1, 3, new[] { 1, 2, 3 })]
    [InlineData(2, 3, new[] { 4, 5, 6 })]
    [InlineData(4, 3, new[] { 10 })]
    [InlineData(5, 3, new int[0])]
    [InlineData(int.MaxValue, int.MaxValue, new int[0])]
    public void Page_ShouldReturnTheRequestedPage(int pageNumber, int pageSize, int[] expected)
    {
        IEnumerable<int> source = Enumerable.Range(1, 10);

        Assert.Equal(expected, source.Page(pageNumber, pageSize));
        Assert.Equal(expected, source.AsQueryable().Page(pageNumber, pageSize));
    }

    [Theory]
    [InlineData(0, 10)]
    [InlineData(1, 0)]
    [InlineData(-1, 10)]
    public void Page_ShouldRejectInvalidArguments(int pageNumber, int pageSize)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Enumerable.Range(1, 10).Page(pageNumber, pageSize));
    }

    [Fact]
    public void ToReadOnlyList_ShouldCopyAndBlockMutation()
    {
        var source = new List<int> { 1, 2 };

        IReadOnlyList<int> result = source.ToReadOnlyList();
        source.Add(3);

        Assert.Equal([1, 2], result);
        Assert.False(result is int[]);
        Assert.Throws<NotSupportedException>(() => ((IList<int>)result).Add(4));
    }
}
