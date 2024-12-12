namespace AdventOfCode.Days;

public class Day12 : BaseDay
{
    private readonly char[][] _input;
    private readonly List<(int, int)> _regionsStarts;
    private List<HashSet<(int, int)>> _regions;

    public Day12()
    {
        _input = File.ReadAllLines(InputFilePath).Select(x => x.ToCharArray()).ToArray();
        _regionsStarts = [];
        _regions = [];

        var doneRegions = new List<char>();
        for (int i = 0; i < _input.Length; i++)
        {
            for (int j = 0; j < _input[0].Length; j++)
            {
                if (!doneRegions.Contains(_input[i][j]))
                {
                    doneRegions.Add(_input[i][j]);
                    _regionsStarts.Add((i, j));

                }
            }
        }
    }

    public override ValueTask<string> Solve_1()
    {
        var totalFencingPrice = 0;
        var areasExplored = new List<(int, int)>();

        for (int i = 0; i < _input.Length; i++)
        {
            for (int j = 0; j < _input[0].Length; j++)
            {
                if (areasExplored.Contains((i, j))) continue;

                var regionArea = new HashSet<(int, int)>();
                var perimeter = GetPerimeter((i, j), regionArea, 0);
                areasExplored.AddRange(regionArea);

                _regions.AddRange(regionArea);

                totalFencingPrice += perimeter * regionArea.Count;
            }
        }

        return new(totalFencingPrice.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var totalFencingPrice = 0;

        foreach (var region in _regions)
        {
            var sides = GetSidesCount(region);

            totalFencingPrice += sides * region.Count;
        }

        return new(totalFencingPrice.ToString());
    }

    private int GetPerimeter((int, int) start, HashSet<(int, int)> area, int perimeter)
    {
        area.Add(start);

        var personalPerimeter = 4;

        if (start.Item1 == 0)
        {
            perimeter++;
            personalPerimeter--;
        }

        if (start.Item2 == 0)
        {
            perimeter++;
            personalPerimeter--;
        }

        var surroundingValues = _input.GetSurroundingCompassValues(start).Where(x => _input[x.Item1][x.Item2] == _input[start.Item1][start.Item2]);
        personalPerimeter -= surroundingValues.Count();
        perimeter += personalPerimeter;

        foreach (var value in surroundingValues)
        {
            if (area.Contains(value)) continue;

            perimeter = GetPerimeter(value, area, perimeter);
        }

        return perimeter;
    }

    private static int GetSidesCount(HashSet<(int, int)> region)
    {
        var sides = 0;
        var topHorizontal = new HashSet<(int, int)>();
        var bottomHorizontal = new HashSet<(int, int)>();
        var leftVertical = new HashSet<(int, int)>();
        var rightVertical = new HashSet<(int, int)>();

        var horizontalSides = region.OrderBy(x => x.Item2).GroupBy(x => x.Item1);

        foreach (var horizontalSide in horizontalSides)
        {
            var side = horizontalSide.ToList();
            for (var i = 0; i < side.Count; i++)
            {
                var regionArea = side[i];
                var length = 0;
                var horizontalIndex = regionArea.Item2;

                while (!region.Contains((regionArea.Item1 - 1, horizontalIndex)) && side.Contains((regionArea.Item1, horizontalIndex)) && !topHorizontal.Contains((regionArea.Item1, horizontalIndex)))
                {
                    topHorizontal.Add((regionArea.Item1, horizontalIndex));
                    horizontalIndex++;
                    length++;
                }

                if (length > 0)
                    sides++;

                length = 0;
                horizontalIndex = regionArea.Item2;

                while (!region.Contains((regionArea.Item1 + 1, horizontalIndex)) && side.Contains((regionArea.Item1, horizontalIndex)) && !bottomHorizontal.Contains((regionArea.Item1, horizontalIndex)))
                {
                    bottomHorizontal.Add((regionArea.Item1, horizontalIndex));
                    horizontalIndex++;
                    length++;
                }

                if (length > 0)
                    sides++;
            }
        }

        var verticalSides = region.OrderBy(x => x.Item1).GroupBy(x => x.Item2);

        foreach (var verticalSide in verticalSides)
        {
            var side = verticalSide.ToList();
            for (var i = 0; i < side.Count; i++)
            {
                var regionArea = side[i];
                var length = 0;
                var verticalIndex = regionArea.Item1;

                while (!region.Contains((verticalIndex, regionArea.Item2 - 1)) && side.Contains((verticalIndex, regionArea.Item2)) && !leftVertical.Contains((verticalIndex, regionArea.Item2)))
                {
                    leftVertical.Add((verticalIndex, regionArea.Item2));
                    verticalIndex++;
                    length++;
                }

                if (length > 0)
                    sides++;

                length = 0;
                verticalIndex = regionArea.Item1;

                while (!region.Contains((verticalIndex, regionArea.Item2 + 1)) && side.Contains((verticalIndex, regionArea.Item2)) && !rightVertical.Contains((verticalIndex, regionArea.Item2)))
                {
                    rightVertical.Add((verticalIndex, regionArea.Item2));
                    verticalIndex++;
                    length++;
                }

                if (length > 0)
                    sides++;
            }
        }

        return sides;
    }
}
