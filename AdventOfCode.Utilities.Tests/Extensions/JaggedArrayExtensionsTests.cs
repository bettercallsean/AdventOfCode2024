namespace AdventOfCode2024.Utilities.Tests.Extensions;

[TestClass]
public sealed class JaggedArrayExtensionsTests
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
        Assert.IsTrue(array.IsValidCoordinate(x, y));
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
        Assert.IsFalse(array.IsValidCoordinate(-1, 1));
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
        var slice = array.GetVerticalSlice(x, 0, 2);
        
        // Assert
        CollectionAssert.AreEqual(expectedArray, slice);
    }
}
