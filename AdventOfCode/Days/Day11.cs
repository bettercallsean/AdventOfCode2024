using System.Text.Json;

namespace AdventOfCode.Days;

public class Day11 : BaseDay
{
    private readonly List<long> _input;

    public Day11()
    {
        _input = File.ReadAllText(InputFilePath).Split(' ').Select(long.Parse).ToList();
    }

    public override ValueTask<string> Solve_1()
    {
        var stones = GetStonesAfterIteration(25);

        return new(stones.Values.Sum().ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var stones = GetStonesAfterIteration(75);

        return new(stones.Values.Sum().ToString());
    }

    private Dictionary<long, long> GetStonesAfterIteration(int iteration)
    {
        var stones = _input.ToDictionary(x => x, x => _input.LongCount(y => y == x));
        stones.TryAdd(1, 0);

        for (var i = 0; i < iteration; i++)
        {
            var modifications = new Dictionary<long, long> { { 1, 0 } };
            foreach (var stone in stones.Where(stone => stone.Value != 0))
            {
                if (stone.Key == 0)
                    AddModifiedValue(1, stone.Value, modifications);
                else if (stone.Key.ToString().Length % 2 == 0)
                {
                    var stoneString = stone.Key.ToString();
                    var leftStone = int.Parse(stoneString[..(stoneString.Length / 2)]);
                    var rightStone = int.Parse(stoneString[(stoneString.Length / 2)..]);

                    AddModifiedValue(leftStone, stone.Value, modifications);
                    AddModifiedValue(rightStone, stone.Value, modifications);
                }
                else
                    AddModifiedValue(stone.Key * 2024, stone.Value, modifications);

                stones[stone.Key] = 0;
            }

            foreach (var modification in modifications)
                stones[modification.Key] = modification.Value;

            modifications.Clear();
        }

        return stones.Where(x => x.Value > 0).ToDictionary(x => x.Key, x => x.Value);
    }

    private static void AddModifiedValue(long key, long value, Dictionary<long, long> modifications)
    {
        if (!modifications.TryAdd(key, value))
            modifications[key] += value;
    }
}
