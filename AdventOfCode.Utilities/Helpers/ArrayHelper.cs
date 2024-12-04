namespace AdventOfCode.Utilities.Helpers;

public static class ArrayHelper
{
    private const string DebugTextFilePath = "debug.txt";

    public static bool IsValidCoordinate<T>(int x, int y, T[][] array)
    {
        return !(x < 0 || y < 0 || x > array.Length - 1 || y > array[0].Length - 1);
    }

    public static bool IsValidCoordinate<T>(int x, T[] array)
    {
        return !(x < 0 || x > array.Length - 1);
    }

    public static T[] GetVerticalSlice<T>(int x, int y, int endY, T[][] array)
    {
        var slice = new List<T>();
        var absoluteLength = Math.Abs(y - endY);
        for (int i = 0; i <= absoluteLength; i++)
        {
            slice.Add(array[y + (endY > y ? 1 : -1) * i][x]);
        }

        return [.. slice];
    }

    public static T[] GetDiaganolSlice<T>(int x, int y, int endX, int endY, T[][] array)
    {
        var slice = new List<T>();
        var absoluteLength = Math.Abs(y - endY);


        for (int i = 0; i <= absoluteLength; i++)
        {
            slice.Add(array[y + (endY > y ? 1 : -1) * i][x + (endX > x ? 1 : -1) * i]);
        }

        return [.. slice];
    }

    public static void ArrayPrinter<T>(T[][] array)
    {
        foreach (var row in array)
        {
            foreach (var item in row)
            {
                Console.Write(item);
            }

            Console.WriteLine();
        }

        Console.WriteLine();
    }

    public static void ArrayPrinter<T>(T[,] array)
    {
        var row = array.GetLength(0);
        var col = array.GetLength(1);

        for (var i = 0; i < row; i++)
        {
            for (var j = 0; j < col; j++)
            {
                Console.Write(array[i, j]);
            }

            Console.WriteLine();
        }

        Console.WriteLine();
    }

    public static bool ArraysAreTheSame<T>(T[][] arr1, T[][] arr2) where T : IComparable<T>
    {
        var row = arr1.Length;
        var col = arr1[0].Length;

        for (var i = 0; i < row; i++)
        {
            for (var j = 0; j < col; j++)
            {
                if (arr1[i][j].CompareTo(arr2[i][j]) != 0)
                    return false;
            }
        }

        return true;
    }

    public static IEnumerable<(int, int)> GetSurroundingValues<T>(int x, int y, T[][] array)
    {
        var surroundingCoordinates = new List<(int X, int Y)>
        {
            (x - 1, y - 1), (x - 1, y), (x - 1, y + 1), (x, y - 1), (x, y + 1), (x + 1, y - 1), (x + 1, y), (x + 1, y + 1)
        };

        return surroundingCoordinates.Where(coordinate => IsValidCoordinate(coordinate.X, coordinate.Y, array)).Select(coordinate => (coordinate.X, coordinate.Y));
    }

    public static IEnumerable<(int, int)> GetSurroundingCompassValues<T>(int x, int y, T[][] array)
    {
        var surroundingCoordinates = new List<(int X, int Y)>
        {
            (x - 1, y), (x, y - 1), (x , y + 1), (x + 1, y)
        };

        return surroundingCoordinates.Where(coordinate => IsValidCoordinate(coordinate.X, coordinate.Y, array)).Select(coordinate => (coordinate.X, coordinate.Y));
    }

    public static void CreateArrayTextFile<T>(T[][] array)
    {
        File.WriteAllText(DebugTextFilePath, string.Empty);

        for (int i = 0; i < array.Length; i++)
        {
            for (int j = 0; j < array[0].Length; j++)
            {
                File.AppendAllText(DebugTextFilePath, $"{array[i][j]}");
            }

            File.AppendAllText(DebugTextFilePath, Environment.NewLine);
        }
    }
}