using AdventOfCode.Utilities.Helpers;

namespace AdventOfCode2024.Utilities.Tests;

[TestClass]
public sealed class ArrayHelperTests
{
    [TestMethod]
    [DataRow(0, 0)]
    [DataRow(1, 1)]
    [DataRow(2, 2)]
    public void IsValidCoordinate_WhenPassedValidCoordinate_ReturnsTrue(int x, int y)
    {
        // Arrange
        var array = new int[][] { [1, 2, 3], [4, 5, 6], [7, 8, 9] };

        // Assert
        Assert.IsTrue(ArrayHelper.IsValidCoordinate(x, y, array));
    }

    [TestMethod]
    [DataRow(0, 4)]
    [DataRow(-1, 1)]
    [DataRow(4, 2)]
    public void IsValidCoordinate_WhenPassedInvalidCoordinate_ReturnsTrue(int x, int y)
    {
        // Arrange
        var array = new int[][] { [1, 2, 3], [4, 5, 6], [7, 8, 9] };

        // Assert
        Assert.IsFalse(ArrayHelper.IsValidCoordinate(-1, 1, array));
    }

    [TestMethod]
    [DataRow(0)]
    [DataRow(1)]
    [DataRow(2)]
    public void IsValidCoordinate1DArray_WhenPassedValidCoordinate_ReturnsTrue(int x)
    {
        // Arrange
        var array = new[] { 1, 2, 3 };

        // Assert
        Assert.IsTrue(ArrayHelper.IsValidCoordinate(x, array));
    }

    [TestMethod]
    [DataRow(-1)]
    [DataRow(4)]
    public void IsValidCoordinate1DArray_WhenPassedInvalidCoordinate_ReturnsFalse(int x)
    {
        // Arrange
        var array = new[] { 1, 2, 3 };

        // Assert
        Assert.IsFalse(ArrayHelper.IsValidCoordinate(x, array));
    }
    
    [TestMethod]
    [DataRow(0, new[] {1, 4, 7})]
    [DataRow(1, new[] {2, 5, 8})]
    [DataRow(2, new[] {3, 6, 9})]
    public void GetVerticalSlice_WhenPassedValidYValues_ReturnsVerticalSlice(int x, int[] expectedArray)
    {
        // Arrange
        var array = new int[][] { [1, 2, 3], [4, 5, 6], [7, 8, 9] };

        // Act
        var slice = ArrayHelper.GetVerticalSlice(x, 0, 2, array);
        
        // Assert
        CollectionAssert.AreEqual(expectedArray, slice);
    }
}
