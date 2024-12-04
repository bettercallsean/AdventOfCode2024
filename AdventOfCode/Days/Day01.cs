namespace AdventOfCode.Days;

public class Day01 : BaseDay
{
    private readonly List<int> _leftInput;
    private readonly List<int> _rightInput;

    public Day01()
    {
        var input = File.ReadAllLines(InputFilePath);
        _leftInput = [];
        _rightInput = [];

        foreach (var location in input)
        {
            var locations = location.Split("   ");

            _leftInput.Add(int.Parse(locations[0]));
            _rightInput.Add(int.Parse(locations[1]));
        }

        _leftInput.Sort();
        _rightInput.Sort();
    }

    public override ValueTask<string> Solve_1() => new(_leftInput.Select((t, i) => Math.Abs(t - _rightInput[i])).Sum().ToString());

    public override ValueTask<string> Solve_2()
    {
        var rightInputOccurrences = _rightInput.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());

        return new(_leftInput.Sum(locationCount => locationCount * rightInputOccurrences.GetValueOrDefault(locationCount, 0)).ToString());
    }
}
