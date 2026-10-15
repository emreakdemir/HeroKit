using System.Reflection;
using HeroKit.Assemblies;

namespace HeroKit.Tests.Assemblies;

public class AssemblyExtensionsTests
{
    [Fact]
    public void GetAttribute_ShouldReturnAttributeWhenPresent()
    {
        Assembly assembly = typeof(HeroKit.Integers.IntegerExtensions).Assembly;

        Assert.NotNull(assembly.GetAttribute<AssemblyTitleAttribute>());
        Assert.Null(assembly.GetAttribute<AssemblyTrademarkAttribute>());
    }
}
