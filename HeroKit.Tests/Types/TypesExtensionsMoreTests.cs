using System.Reflection;

namespace HeroKit.Tests.Types;

public struct DefaultableStruct
{
    public int Value;
}

public static class MoreSampleExtensions
{
    public static string Shout(this string value) => value.ToUpperInvariant();

    public static string Whisper(this string value) => value.ToLowerInvariant();

    public static int Twice(this int value) => value * 2;
}

public class TypesExtensionsMoreTests
{
    private static readonly Assembly TestAssembly = typeof(MoreSampleExtensions).Assembly;

    [Fact]
    public void GetDefault_ShouldReturnNull_ForVoid()
    {
        Assert.Null(typeof(void).GetDefault());
    }

    [Fact]
    public void GetDefault_ShouldReturnNull_ForInterface()
    {
        Assert.Null(typeof(IDisposable).GetDefault());
    }

    [Fact]
    public void GetDefault_ShouldReturnDefaultInstance_ForCustomStruct()
    {
        object? result = typeof(DefaultableStruct).GetDefault();

        Assert.Equal(default(DefaultableStruct), result);
    }

    [Fact]
    public void GetDefault_ShouldReturnZero_ForEnum()
    {
        Assert.Equal(DayOfWeek.Sunday, typeof(DayOfWeek).GetDefault());
    }

    [Fact]
    public void GetDefault_ShouldReturnNull_ForNullableValueType()
    {
        Assert.Null(typeof(int?).GetDefault());
    }

    [Fact]
    public void GetDefault_ShouldThrow_ForOpenGenericValueType()
    {
        ArgumentException exception = Assert.Throws<ArgumentException>(() => typeof(ValueTuple<>).GetDefault());

        Assert.Contains("generic parameters", exception.Message);
    }

    [Fact]
    public void GetDefault_ShouldThrow_ForNonPublicStruct()
    {
        ArgumentException exception = Assert.Throws<ArgumentException>(() => typeof(HiddenStruct).GetDefault());

        Assert.Contains("not a publicly-visible type", exception.Message);
    }

    [Fact]
    public void GetDefault_ShouldWrapActivatorFailure_ForByRefLikeStruct()
    {
        ArgumentException exception = Assert.Throws<ArgumentException>(() => typeof(Span<int>).GetDefault());

        Assert.NotNull(exception.InnerException);
    }

    [Fact]
    public void GetRealType_ShouldReturnNull_ForInterfaceWithoutImplementation()
    {
        Assert.Null(typeof(IUnimplemented).GetRealType());
    }

    [Fact]
    public void GetRealType_ShouldSkipAbstractTypes()
    {
        Type? result = typeof(PublicAbstractBase).GetRealType();

        Assert.Equal(typeof(PublicConcrete), result);
    }

    [Fact]
    public void GetExtensionMethods_ShouldReturnMethodsForExactType()
    {
        string[] names = typeof(string).GetExtensionMethods(TestAssembly)
                                       .Where(m => m.DeclaringType == typeof(MoreSampleExtensions))
                                       .Select(m => m.Name)
                                       .Order()
                                       .ToArray();

        Assert.Equal(["Shout", "Whisper"], names);
    }

    [Fact]
    public void GetExtensionMethods_ShouldReturnEmpty_ForUnrelatedType()
    {
        Assert.DoesNotContain(typeof(Guid).GetExtensionMethods(TestAssembly), m => m.DeclaringType == typeof(MoreSampleExtensions));
    }

    [Fact]
    public void GetExtensionMethod_ShouldFindMethodByName()
    {
        MethodInfo? method = typeof(int).GetExtensionMethod(TestAssembly, nameof(MoreSampleExtensions.Twice));

        Assert.NotNull(method);
        Assert.Equal(nameof(MoreSampleExtensions.Twice), method.Name);
    }

    [Fact]
    public void GetExtensionMethod_ShouldReturnNull_WhenNameIsUnknown()
    {
        Assert.Null(typeof(int).GetExtensionMethod(TestAssembly, "Missing"));
    }

    [Fact]
    public void GetExtensionMethod_Assemblies_ShouldReturnFirstMatch()
    {
        MethodInfo? method = typeof(string).GetExtensionMethod([typeof(string).Assembly, TestAssembly], nameof(MoreSampleExtensions.Shout));

        Assert.NotNull(method);
        Assert.Equal(typeof(MoreSampleExtensions), method.DeclaringType);
    }

    [Fact]
    public void GetExtensionMethod_Assemblies_ShouldReturnNull_WhenNoAssemblyHasMethod()
    {
        Assert.Null(typeof(string).GetExtensionMethod([TestAssembly], "Missing"));
    }

    [Fact]
    public void GetExtensionMethod_Assemblies_ShouldReturnNull_ForEmptyCollection()
    {
        Assert.Null(typeof(string).GetExtensionMethod(Array.Empty<Assembly>(), nameof(MoreSampleExtensions.Shout)));
    }

    [Fact]
    public void GetExtensionMethod_WithTypes_ShouldReturnSingleCandidate_WithoutComparingTypes()
    {
        MethodInfo? method = typeof(string).GetExtensionMethod(TestAssembly, nameof(MoreSampleExtensions.Shout), []);

        Assert.NotNull(method);
        Assert.Equal(nameof(MoreSampleExtensions.Shout), method.Name);
    }

    [Fact]
    public void GetExtensionMethod_WithTypes_ShouldReturnNull_WhenNameIsUnknown()
    {
        Assert.Null(typeof(string).GetExtensionMethod(TestAssembly, "Missing", []));
    }
}

internal struct HiddenStruct;

public interface IUnimplemented;

public abstract class PublicAbstractBase;

public sealed class PublicConcrete : PublicAbstractBase;
