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
        var stones = _input.ToDictionary(x => x, x => _input.LongCount(y => y == x));
        stones.TryAdd(1, 0);

        for (var i = 0; i < 25; i++)
        {
            var modifications = new Dictionary<long, long> { { 1, 0 } };
            foreach (var stone in stones.Where(stone => stone.Value != 0))
            {
                if (stone.Key == 0)
                    modifications[1] = modifications.TryGetValue(1, out var value) ? value + stone.Value : stone.Value;
                else if (stone.Key.ToString().Length % 2 == 0)
                {
                    var stoneString = stone.Key.ToString();
                    var leftStone = int.Parse(stoneString[..(stoneString.Length / 2)]);
                    var rightStone = int.Parse(stoneString[(stoneString.Length / 2)..]);

                    modifications[leftStone] = modifications.TryGetValue(leftStone, out var value) ? value + stone.Value : stone.Value;
                    modifications[rightStone] = modifications.TryGetValue(rightStone, out value) ? value + stone.Value : stone.Value;
                }
                else
                    modifications[stone.Key * 2024] = modifications.TryGetValue(stone.Key * 2024, out var value) ? value + stone.Value : stone.Value;

                stones[stone.Key] = 0;
            }

            foreach (var modification in modifications)
                stones[modification.Key] = modification.Value;

            modifications.Clear();
        }

        return new(stones.Values.Sum().ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var stones = _input.ToDictionary(x => x, x => _input.LongCount(y => y == x));
        stones.TryAdd(1, 0);

        for (var i = 0; i < 75; i++)
        {
            var modifications = new Dictionary<long, long> { { 1, 0 } };
            foreach (var stone in stones.Where(stone => stone.Value != 0))
            {
                if (stone.Key == 0)
                    modifications[1] = modifications.TryGetValue(1, out var value) ? value + stone.Value : stone.Value;
                else if (stone.Key.ToString().Length % 2 == 0)
                {
                    var stoneString = stone.Key.ToString();
                    var leftStone = int.Parse(stoneString[..(stoneString.Length / 2)]);
                    var rightStone = int.Parse(stoneString[(stoneString.Length / 2)..]);

                    modifications[leftStone] = modifications.TryGetValue(leftStone, out var value) ? value + stone.Value : stone.Value;
                    modifications[rightStone] = modifications.TryGetValue(rightStone, out value) ? value + stone.Value : stone.Value;
                }
                else
                    modifications[stone.Key * 2024] = modifications.TryGetValue(stone.Key * 2024, out var value) ? value + stone.Value : stone.Value;

                stones[stone.Key] = 0;
            }

            foreach (var modification in modifications)
                stones[modification.Key] = modification.Value;

            modifications.Clear();
        }

        return new(stones.Values.Sum().ToString());
    }
}
