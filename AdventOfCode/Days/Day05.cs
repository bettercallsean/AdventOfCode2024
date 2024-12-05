namespace AdventOfCode.Days;
public class Day05 : BaseDay
{
    private readonly Dictionary<int, List<int>> _order;
    private readonly List<List<int>> _updates;
    private readonly List<List<int>> _invalidUpdate;

    public Day05()
    {
        var input = File.ReadAllText(InputFilePath).Split($"{Environment.NewLine}{Environment.NewLine}");

        _order = input[0].Split(Environment.NewLine)
            .Select(x => x.Split('|').Select(int.Parse).ToList())
            .Select(x => (x[0], x[1]))
            .GroupBy(x => x.Item1)
            .ToDictionary(x => x.Key, x => x.Select(x => x.Item2).Order().ToList());

        _updates = [.. input[1].TrimEnd()
            .Split(Environment.NewLine)
            .Select(x => x.Split(',').Select(int.Parse)
            .ToList())];

        _invalidUpdate = [];
    }

    public override ValueTask<string> Solve_1()
    {
        var result = 0;

        foreach (var update in _updates)
        {
            var validUpdate = IsValidUpdate(update);

            if (!validUpdate)
            {
                _invalidUpdate.Add(update);
                continue; ;
            }

            result += update[update.Count / 2];
        }

        return new(result.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var result = 0;

        foreach (var update in _invalidUpdate)
        {
            while (!IsValidUpdate(update))
            {
                for (var i = 0; i < update.Count; i++)
                {
                    var page = update[i];

                    for (var j = i + 1; j < update.Count; j++)
                    {
                        var page2 = update[j];

                        if (_order.TryGetValue(page2, out var pagesToComeAfter) && pagesToComeAfter.Contains(page))
                        {
                            update[i] = page2;
                            update[j] = page;
                            break;
                        }
                    }
                }
            }

            result += update[update.Count / 2];
        }

        return new(result.ToString());
    }

    private bool IsValidUpdate(List<int> update)
    {
        for (var i = 0; i < update.Count; i++)
        {
            var page = update[i];
            var updateOutOfOrder = false;

            for (var j = i + 1; j < update.Count; j++)
            {
                var item2 = update[j];

                if (page == item2)
                    continue;

                if (_order.TryGetValue(item2, out var pagesToComeAfter) && pagesToComeAfter.Contains(page))
                {
                    updateOutOfOrder = true;
                    break;
                }
            }

            if (updateOutOfOrder)
                return false;
        }

        return true;
    }
}
