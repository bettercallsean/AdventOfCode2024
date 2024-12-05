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
                if (line[x] == 'X')
                {
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

                    for (int i = -3; i <= 3; i += 6)
                    {
                        // Top Left && Bottom Right
                        if (ArrayHelper.IsValidCoordinate(x + i, y + i, _input) && new string(ArrayHelper.GetDiaganolSlice(x, y, x + i, y + i, _input)) == "XMAS")
                            count++;

                        // Top Right && Bottom Left
                        if (ArrayHelper.IsValidCoordinate(x + i, (y + i * -1), _input) && new string(ArrayHelper.GetDiaganolSlice(x, y, x + i, y + i * -1, _input)) == "XMAS")
                            count++;
                    }
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
                if (line[x] == 'A')
                {
                    var surroundingCharacterCoordinates = ArrayHelper.GetSurroundingDiaganolValues(y, x, _input);

                    if (surroundingCharacterCoordinates.Count() != 4)
                        continue;

                    var masCount = 0;
                    foreach (var coordinate in surroundingCharacterCoordinates.Where(x => _input[x.Item1][x.Item2] == 'M'))
                    {
                        var xDirection = x - coordinate.Item1;
                        var yDirection = y - coordinate.Item2;
                        var diaganolSlice = ArrayHelper.GetDiaganolSlice(coordinate.Item1, coordinate.Item2, coordinate.Item1 + (xDirection * 2), coordinate.Item2 + (yDirection * 2), _input);

                        if (new string(diaganolSlice) == "MAS" || new string(diaganolSlice) == "SAM")
                            masCount++;
                    }

                    if (masCount == 2)
                        count++;
                }
            }
        }

        return new(count.ToString());
    }
}
