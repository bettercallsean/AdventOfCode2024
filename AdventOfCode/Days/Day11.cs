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
            foreach (var stone in stones)
            {
                if (stone.Key == 0)
                    AddStoneToModifiedList(1, stone.Value, modifications);
                else if (stone.Key.ToString().Length % 2 == 0)
                {
                    var stoneString = stone.Key.ToString();
                    var leftStone = int.Parse(stoneString[..(stoneString.Length / 2)]);
                    var rightStone = int.Parse(stoneString[(stoneString.Length / 2)..]);

                    AddStoneToModifiedList(leftStone, stone.Value, modifications);
                    AddStoneToModifiedList(rightStone, stone.Value, modifications);
                }
                else
                    AddStoneToModifiedList(stone.Key * 2024, stone.Value, modifications);

                stones.Remove(stone.Key);
            }

            foreach (var modification in modifications)
                stones[modification.Key] = modification.Value;
        }

        return stones.Where(x => x.Value > 0).ToDictionary(x => x.Key, x => x.Value);
    }

    private static void AddStoneToModifiedList(long key, long value, Dictionary<long, long> modifications)
    {
        if (!modifications.TryAdd(key, value))
            modifications[key] += value;
    }
}
