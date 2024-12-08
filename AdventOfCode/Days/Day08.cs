using AdventOfCode.Utilities.Helpers;

namespace AdventOfCode.Days;

public class Day08 : BaseDay
{
    private readonly char[][] _input;
    private readonly Dictionary<char, List<(int, int)>> _antennaTypes;

    public Day08()
    {
        _input = File.ReadAllLines(InputFilePath).Select(x => x.ToCharArray()).ToArray();
        _antennaTypes = new Dictionary<char, List<(int, int)>>();
        
        for (var i = 0; i < _input.Length; i++)
        {
            for (var j = 0; j < _input[0].Length; j++)
            {
                if(_input[i][j] == '.') continue;
                
                if(_antennaTypes.TryGetValue(_input[i][j], out var value))
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
                    var euclideanDistance = (antenna.Item1 - adjacentAntenna.Item1, antenna.Item2 - adjacentAntenna.Item2);

                    var validAntinodes = new List<(int, int)>
                        {
                            (antenna.Item1 + euclideanDistance.Item1, antenna.Item2 + euclideanDistance.Item2),
                            (adjacentAntenna.Item1 - euclideanDistance.Item1, adjacentAntenna.Item2 - euclideanDistance.Item2)
                        }
                        .Where(x => ArrayHelper.IsValidCoordinate(x.Item1, x.Item2, _input));

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
                    var euclideanDistance = (antenna.Item1 - adjacentAntenna.Item1, antenna.Item2 - adjacentAntenna.Item2);

                    antinodes.Add(antenna);
                    antinodes.Add(adjacentAntenna);
                    
                    var validAntinodes = Enumerable.Range(1, _input[0].Length).SelectMany(x => new List<(int, int)>
                    {
                        (antenna.Item1 + euclideanDistance.Item1 * x, antenna.Item2 + euclideanDistance.Item2 * x),
                        (adjacentAntenna.Item1 - euclideanDistance.Item1 * x, adjacentAntenna.Item2 - euclideanDistance.Item2 * x)
                    })
                    .Where(x => ArrayHelper.IsValidCoordinate(x.Item1, x.Item2, _input));

                    foreach (var antinode in validAntinodes)
                        antinodes.Add(antinode); 
                }
                
                antennaIndex++;
            }
        }
        
        return new(antinodes.Count.ToString());
    }
}