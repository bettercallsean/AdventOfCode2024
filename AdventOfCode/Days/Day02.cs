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
            var mainDirection = 0;
            for (var i = 0; i < report.Count - 1; i++)
            {
                var direction = report[i] - report[i + 1] > 0 ? 1 : -1;
                
                if (mainDirection == 0)
                    mainDirection = direction;
                
                if(!SafeLevel(report[i], report[i + 1], direction, mainDirection))
                    break;
                
                if (i != report.Count - 2) continue;

                safeReportCount++;
                break;
            }
        }
        
        return new(safeReportCount.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var safeReportCount = 0;
        foreach (var report in _input)
        {
            var foo = report
                // .Select((x, y) => y == 0 ? 0 : x - report[y - 1])
                // .Select((x, y) => )
                .Where((x, y) =>  y != 0 && (Math.Abs(x - report[y - 1]) >= 1 || Math.Abs(x - report[y - 1]) <= 3))
                .ToList();

            var bar = foo.All(x => x is >= 1 and <= 3 or >= -3 and <= -1);
            
            var test = 0;
            
            var mainDirection = 0;
            var problemDampened = false;
            
            for (var i = 1; i < report.Count; i++)
            {
                var direction = report[i] - report[i - 1] > 0 ? 1 : -1;
                
                if (mainDirection == 0)
                    mainDirection = direction;

                if (!SafeLevel(report[i], report[i - 1], direction, mainDirection))
                {
                    if (problemDampened)
                        break;
                
                    if (i == 1)
                    {
                        report.RemoveAt(i);
                        problemDampened = true;
                        mainDirection = 0;
                        i--;
                        continue;
                    }
                    
                    if (i == report.Count - 1)
                    {
                        safeReportCount++;
                        break;
                    }
                    
                    var tempDirection = report[i + 1] - report[i - 1] > 0 ? 1 : -1;
                    if (SafeLevel(report[i + 1], report[i - 1], tempDirection, mainDirection))
                    {
                        report.RemoveAt(i);
                        i--;
                        problemDampened = true;
                    }
                    else if (SafeLevel(report[i], report[i + 1], tempDirection, tempDirection))
                    {
                        problemDampened = true;
                        mainDirection = tempDirection;
                    }
                    else
                        break;
                }
                
                if (i == report.Count - 1) 
                    safeReportCount++;
            }
        }
        
        return new(safeReportCount.ToString());
    }

    private static bool SafeLevel(int a, int b, int direction, int mainDirection)
    {
        return Math.Abs(a - b) <= 3 && Math.Abs(a - b) >= 1 && mainDirection == direction;
    }
}