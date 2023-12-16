namespace HeroKit.Tests.Arrays;

using System;
using System.Collections.Generic;
using System.Web;
using HeroKit.Arrays;

public class ArrayConversionExtensionsTests
{
    private static int MapFunction(object value) => (int)value * 10;

    [Fact]
    public void ToListShouldTurnArrayToListByApplyingMapFunction()
    {
        // Arrange
        int[] array = { 1, 2, 3, 4, 5 };

        // Act
        List<int> result = array.ToList(MapFunction);

        // Assert
        Assert.Equal(new List<int> { 10, 20, 30, 40, 50 }, result);
    }

    [Fact]
    public void ToListShouldReturnEmptyListWhenInputArrayIsNullOrEmpty()
    {
        // Arrange
        int[] array = Array.Empty<int>();

        // Act
        List<int> result = array.ToList(MapFunction);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void ToListShouldReturnEmptyListWhenMapFunctionIsNull()
    {
        // Arrange
        int[] array = { 1, 2, 3, 4, 5 };

        // Act
        List<int> result = array.ToList(mapFunction: null);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void UrlTokenEncodeShouldEncodeArrayToString()
    {
        // Arrange
        byte[] byteArray = { 1, 2, 3, 4, 5 };

        // Act
        string result = byteArray.UrlTokenEncode();

        // Assert
        Assert.Equal(HttpUtility.UrlEncode(byteArray), result);
    }

    [Fact]
    public void UrlTokenEncodeShouldReturnEmptyStringWhenNullOrEmptyInput()
    {
        // Arrange
        byte[] array = Array.Empty<byte>();

        // Act
        string result = array.UrlTokenEncode();

        // Assert
        Assert.Equal(string.Empty, result);
    }
}