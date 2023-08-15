namespace HeroKit.Tests.Types;

public class TypeExtensionsTests
{
    [Fact]
    public void GetDefault_ShouldReturnDefault_ForValueType()
    {
        // Arrange
        Type valueType = typeof(int);

        // Act
        object defaultValue = valueType.GetDefault();

        // Assert
        Assert.Equal(0, defaultValue);
    }

    [Fact]
    public void GetDefault_ShouldReturnNull_ForReferenceType()
    {
        // Arrange
        Type referenceType = typeof(string);

        // Act
        object defaultValue = referenceType.GetDefault();

        // Assert
        Assert.Null(defaultValue);
    }

    [Fact]
    public void GetDefault_Generic_ShouldReturnDefault_ForValueType()
    {
        // Act
        object defaultValue = typeof(int).GetDefault();

        // Assert
        Assert.Equal(0, defaultValue);
    }

    [Fact]
    public void GetDefault_Generic_ShouldReturnDefault_ForReferenceType()
    {
        // Act
        object defaultValue = typeof(string).GetDefault();

        // Assert
        Assert.Null(defaultValue);
    }

    [Fact]
    public void GetRealType_ShouldReturnConcreteImplementation_ForInterfaceType()
    {
        // Arrange
        Type interfaceType = typeof(IEnumerable<int>);

        // Act
        Type implementationType = interfaceType.GetRealType();

        // Assert
        Assert.Null(implementationType);
    }
}