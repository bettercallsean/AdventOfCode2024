namespace AdventOfCode2024.Utilities.Tests.Extensions;

[TestClass]
public class ArrayExtensionsTest
{
    [TestMethod]
    [DataRow(0)]
    [DataRow(1)]
    [DataRow(2)]
    public void IsValidCoordinate1DArray_WhenPassedValidCoordinate_ReturnsTrue(int x)
    {
        // Arrange
        var array = new[] { 1, 2, 3 };

        // Assert
        Assert.IsTrue(array.IsValidCoordinate(x));
    }

    [TestMethod]
    [DataRow(-1)]
    [DataRow(4)]
    public void IsValidCoordinate1DArray_WhenPassedInvalidCoordinate_ReturnsFalse(int x)
    {
        // Arrange
        var array = new[] { 1, 2, 3 };

        // Assert
        Assert.IsFalse(array.IsValidCoordinate(x));
    }
}