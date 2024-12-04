namespace AdventOfCode.Utilities;

public static class IEnumerableHelper
{
    private const string DebugTextFilePath = "debug.txt";

    public static void CreateArrayTextFile<T>(IEnumerable<IEnumerable<T>> list)
    {
        var temp = list.Select(x => x.ToList()).ToList();
        File.WriteAllText(DebugTextFilePath, string.Empty);

        for (int i = 0; i < temp.Count; i++)
        {
            for (int j = 0; j < temp[0].Count; j++)
            {
                File.AppendAllText(DebugTextFilePath, $"{temp[i][j]}");
            }

            File.AppendAllText(DebugTextFilePath, Environment.NewLine);
        }
    }
}
