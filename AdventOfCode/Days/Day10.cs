using AdventOfCode.Utilities.Helpers;

namespace AdventOfCode.Days;

public class Day10 : BaseDay
{
    private readonly int[][] _input;
    private readonly List<(int, int)> _startPoints;
    private readonly List<(int, int)> _endPoints;

    public Day10()
    {
        _input = File.ReadAllLines(InputFilePath)
            .Select(x => x.ToCharArray().Select(y => y - 48).ToArray())
            .ToArray();
        
        _startPoints = [];
        _endPoints = [];
        
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
        var trailCount = 0;

        foreach (var validPaths in _startPoints.Select(start => GetValidPaths(start, [])))
        {
            _endPoints.AddRange(validPaths);
            trailCount += validPaths.Distinct().Count();
        }
        
        return new(trailCount.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        return new(_endPoints.Count.ToString());
    }
    
    private List<(int, int)> GetValidPaths((int, int) start, List<(int, int)> validPaths)
    {
        var surroundingPaths = ArrayHelper.GetSurroundingCompassValues(start.Item1, start.Item2, _input);
        foreach (var surroundingValue in surroundingPaths)
        {
            if (_input[surroundingValue.Item1][surroundingValue.Item2] - _input[start.Item1][start.Item2] is not 1) continue;

            if (_input[surroundingValue.Item1][surroundingValue.Item2] == 9 && _input[start.Item1][start.Item2] != 9)
                validPaths.Add(surroundingValue);
            else
                validPaths = GetValidPaths(surroundingValue, validPaths);
        }
        
        return validPaths;
    }
}