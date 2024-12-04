namespace AdventOfCode.Days;

public class Day02 : BaseDay
{
    private readonly List<List<int>> _input;

    public Day02()
    {
        _input = File.ReadAllLines(InputFilePath).Select(x => x.Split(' ').Select(int.Parse).ToList()).ToList();
    }

    public override ValueTask<string> Solve_1()
    {
        var safeReportCount = 0;

        foreach (var report in _input)
        {
            var levelDifferences = GetReportDifferences(report);

            if (ReportIsSafe(levelDifferences))
            {
                safeReportCount++;
            }
        }

        return new(safeReportCount.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var safeReportCount = 0;

        foreach (var report in _input)
        {
            var levelDifferences = GetReportDifferences(report);

            if (ReportIsSafe(levelDifferences))
            {
                safeReportCount++;
            }
            else
            {
                for (int i = 0; i < report.Count; i++)
                {
                    var modifiedReport = report.Take(i).Concat(report.Skip(i + 1)).ToList();
                    levelDifferences = GetReportDifferences(modifiedReport);

                    if (ReportIsSafe(levelDifferences))
                    {
                        safeReportCount++;
                        break;
                    }
                }
            }
        }

        return new(safeReportCount.ToString());
    }

    private static bool ReportIsSafe(List<int> levelDifferences)
    {
        return levelDifferences.All(x => x is >= 1 and <= 3) || levelDifferences.All(x => x is <= -1 and >= -3);
    }

    private static List<int> GetReportDifferences(List<int> report) => report
                .Select((x, y) => new { Value = x, Index = y })
                .Where(x => x.Index != 0 && (Math.Abs(x.Value - report[x.Index - 1]) >= 1 || Math.Abs(x.Value - report[x.Index - 1]) <= 3))
                .Select(x => x.Value - report[x.Index - 1])
                .ToList();
}

