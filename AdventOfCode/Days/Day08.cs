namespace AdventOfCode.Days;

public class Day08 : BaseDay
{
    private readonly char[][] _input;
    private readonly Dictionary<char, List<(int, int)>> _antennaTypes;

    public Day08()
    {
        _input = File.ReadAllLines(InputFilePath).Select(x => x.ToCharArray()).ToArray();
        _antennaTypes = [];

        for (var i = 0; i < _input.Length; i++)
        {
            for (var j = 0; j < _input[0].Length; j++)
            {
                if (_input[i][j] == '.') continue;

                if (_antennaTypes.TryGetValue(_input[i][j], out var value))
                    value.Add((i, j));
                else
                    _antennaTypes.Add(_input[i][j], [(i, j)]);
            }
        }
    }

    public override ValueTask<string> Solve_1()
    {
        var antinodes = new HashSet<(int, int)>();

        foreach (var antennaType in _antennaTypes)
        {
            var antennaIndex = 0;

            foreach (var antenna in antennaType.Value)
            {
                for (var i = antennaIndex + 1; i < antennaType.Value.Count; i++)
                {
                    var adjacentAntenna = antennaType.Value[i];

                    var validAntinodes = GetValidAntinodeLocations(1, 1, antenna, adjacentAntenna);

                    foreach (var antinode in validAntinodes)
                        antinodes.Add(antinode);
                }

                antennaIndex++;
            }
        }

        return new(antinodes.Count.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var antinodes = new HashSet<(int, int)>();

        foreach (var antennaType in _antennaTypes)
        {
            var antennaIndex = 0;

            foreach (var antenna in antennaType.Value)
            {
                for (var i = antennaIndex + 1; i < antennaType.Value.Count; i++)
                {
                    var adjacentAntenna = antennaType.Value[i];

                    var validAntinodes = GetValidAntinodeLocations(0, _input.Length, antenna, adjacentAntenna);

                    foreach (var antinode in validAntinodes)
                        antinodes.Add(antinode);
                }

                antennaIndex++;
            }
        }

        return new(antinodes.Count.ToString());
    }

    private List<(int, int)> GetValidAntinodeLocations(int start, int end, (int, int) antenna1, (int, int) antenna2)
    {
        var distance = (antenna1.Item1 - antenna2.Item1, antenna1.Item2 - antenna2.Item2);

        return Enumerable.Range(start, end)
            .SelectMany(x => new List<(int, int)>
            {
                (antenna1.Item1 + distance.Item1 * x, antenna1.Item2 + distance.Item2 * x),
                (antenna2.Item1 - distance.Item1 * x, antenna2.Item2 - distance.Item2 * x)
            })
            .Where(x => _input.IsValidCoordinate(x.Item1, x.Item2))
            .ToList();
    }
}