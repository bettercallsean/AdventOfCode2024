using AdventOfCode.Utilities.Helpers;

namespace AdventOfCode.Days;

public class Day10 : BaseDay
{
    private readonly int[][] _input;
    private readonly List<(int, int)> _startPoints;

    public Day10()
    {
        _input = File.ReadAllLines(InputFilePath)
            .Select(x => x.ToCharArray().Select(x => x - 48).ToArray())
            .ToArray();
        
        _startPoints = [];
        for (var i = 0; i < _input.Length; i++)
        {
            for (var j = 0; j < _input[0].Length; j++)
            {
                if(_input[i][j] == 0)
                    _startPoints.Add((i, j));
            }
        }
    }
    
    public override ValueTask<string> Solve_1()
    {
        var trailCount = _startPoints.Sum(start => GetValidPaths(start, [], []).Distinct().Count());
        
        return new(trailCount.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var trailCount = _startPoints.Sum(startPoint => GetValidPaths(startPoint, [], []).Count);

        return new(trailCount.ToString());
    }
    
    private List<(int, int)> GetValidPaths((int, int) start, HashSet<(int, int)> path, List<(int, int)> validPaths)
    {
        path = path.Select(x => x).ToHashSet();
        path.Add(start);
        
        var surroundingPaths = ArrayHelper.GetSurroundingCompassValues(start.Item1, start.Item2, _input);
        foreach (var surroundingValue in surroundingPaths.Where(x => !path.Contains(x)))
        {
            if (_input[surroundingValue.Item1][surroundingValue.Item2] - _input[start.Item1][start.Item2] is not 1) continue;

            if (_input[surroundingValue.Item1][surroundingValue.Item2] == 9 && _input[start.Item1][start.Item2] != 9)
                validPaths.Add(surroundingValue);
            else
                validPaths = GetValidPaths(surroundingValue, path, validPaths);
        }
        
        return validPaths;
    }
}