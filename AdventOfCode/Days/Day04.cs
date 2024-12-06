using AdventOfCode.Utilities.Helpers;

namespace AdventOfCode.Days;
public class Day04 : BaseDay
{
    private readonly char[][] _input;

    public Day04()
    {
        _input = [.. File.ReadAllLines(InputFilePath).Select(x => x.ToCharArray())];
    }

    public override ValueTask<string> Solve_1()
    {
        var count = 0;

        for (var y = 0; y < _input.GetLength(0); y++)
        {
            var line = _input[y];

            for (var x = 0; x < line.Length; x++)
            {
                if (line[x] != 'X') continue;
                
                // Right
                if (ArrayHelper.IsValidCoordinate(x + 3, line) && new string(line[x..(x + 4)]) == "XMAS")
                    count++;

                // Left
                if (ArrayHelper.IsValidCoordinate(x - 3, line) && new string(line[(x - 3)..(x + 1)]) == "SAMX")
                    count++;

                // Up
                if (ArrayHelper.IsValidCoordinate(x, y - 3, _input) && new string(ArrayHelper.GetVerticalSlice(x, y, y - 3, _input)) == "XMAS")
                    count++;

                // Down
                if (ArrayHelper.IsValidCoordinate(x, y + 3, _input) && new string(ArrayHelper.GetVerticalSlice(x, y, y + 3, _input)) == "XMAS")
                    count++;

                for (var i = -3; i <= 3; i += 6)
                {
                    // Top Left && Bottom Right
                    if (ArrayHelper.IsValidCoordinate(x + i, y + i, _input) && new string(ArrayHelper.GetDiagonalSlice(x, y, x + i, y + i, _input)) == "XMAS")
                        count++;

                    // Top Right && Bottom Left
                    if (ArrayHelper.IsValidCoordinate(x + i, (y + i * -1), _input) && new string(ArrayHelper.GetDiagonalSlice(x, y, x + i, y + i * -1, _input)) == "XMAS")
                        count++;
                }
            }
        }

        return new(count.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var count = 0;

        for (var y = 0; y < _input.GetLength(0); y++)
        {
            var line = _input[y];

            for (var x = 0; x < line.Length; x++)
            {
                if (line[x] != 'A') continue;
                
                var surroundingCharacterCoordinates = ArrayHelper.GetSurroundingDiagonalValues(x, y, _input).ToList();

                if (surroundingCharacterCoordinates.Count != 4)
                    continue;

                var masCount = surroundingCharacterCoordinates.Where(coordinate => _input[coordinate.Item2][coordinate.Item1] == 'M')
                    .Select(coordinate =>
                    {
                        var xDirection = x - coordinate.Item1;
                        var yDirection = y - coordinate.Item2;
                        return ArrayHelper.GetDiagonalSlice(coordinate.Item1, coordinate.Item2, coordinate.Item1 + xDirection * 2, coordinate.Item2 + yDirection * 2, _input);
                    })
                    .Count(diagonalSlice => new string(diagonalSlice) == "MAS" || new string(diagonalSlice) == "SAM");

                if (masCount == 2)
                    count++;
            }
        }

        return new(count.ToString());
    }
}
