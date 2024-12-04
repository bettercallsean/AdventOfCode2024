namespace AdventOfCode.Utilities.Helpers;
public class StringHelper
{
    public static bool IsValidSubstring(int x, int y, string input)
    {
        return !(x < 0 || y < 0 || x > input.Length - 1 || y > input.Length - 1);
    }
}
