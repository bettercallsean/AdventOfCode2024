namespace AdventOfCode.Days;
public class Day05 : BaseDay
{
    private readonly Dictionary<int, List<int>> _order;
    private readonly List<List<int>> _pages;
    private readonly List<List<int>> _invalidPages;

    public Day05()
    {
        var input = File.ReadAllText(InputFilePath).Split($"{Environment.NewLine}{Environment.NewLine}");

        _order = input[0].Split(Environment.NewLine)
            .Select(x => x.Split('|').Select(int.Parse).ToList())
            .Select(x => (x[0], x[1]))
            .GroupBy(x => x.Item1)
            .ToDictionary(x => x.Key, x => x.Select(x => x.Item2).Order().ToList());

        _pages = [.. input[1].TrimEnd()
            .Split(Environment.NewLine)
            .Select(x => x.Split(',').Select(int.Parse)
            .ToList())];

        _invalidPages = [];
    }

    public override ValueTask<string> Solve_1()
    {
        var result = 0;

        foreach (var page in _pages)
        {
            var validPage = IsValidPage(page);

            if (!validPage)
            {
                _invalidPages.Add(page);
                continue; ;
            }

            result += page[page.Count / 2];
        }

        return new(result.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var result = 0;

        foreach (var page in _invalidPages)
        {
            while (!IsValidPage(page))
            {
                for (var i = 0; i < page.Count; i++)
                {
                    var item = page[i];

                    for (var j = i + 1; j < page.Count; j++)
                    {
                        var item2 = page[j];

                        if (_order.TryGetValue(item2, out var value) && value.Contains(item))
                        {
                            page[i] = item2;
                            page[j] = item;
                            break;
                        }
                    }
                }
            }

            result += page[page.Count / 2];
        }

        return new(result.ToString());
    }

    private bool IsValidPage(List<int> page)
    {
        for (var i = 0; i < page.Count; i++)
        {
            var item = page[i];
            var itemOutOfOrder = false;

            for (var j = i + 1; j < page.Count; j++)
            {
                var item2 = page[j];

                if (item == item2)
                    continue;

                if (_order.TryGetValue(item2, out var value) && value.Contains(item))
                {
                    itemOutOfOrder = true;
                    break;
                }
            }

            if (itemOutOfOrder)
                return false;
        }

        return true;
    }
}
