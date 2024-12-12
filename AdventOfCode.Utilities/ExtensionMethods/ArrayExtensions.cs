namespace AdventOfCode.Utilities.ExtensionMethods;

public static class ArrayExtensions
{
    public static bool IsValidCoordinate<T>(this T[] array, int x)
    {
        return !(x < 0 || x > array.Length - 1);
    }
}