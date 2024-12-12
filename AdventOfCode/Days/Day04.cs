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
                if (line.IsValidCoordinate(x + 3) && new string(line[x..(x + 4)]) == "XMAS")
                    count++;

                // Left
                if (line.IsValidCoordinate(x - 3) && new string(line[(x - 3)..(x + 1)]) == "SAMX")
                    count++;

                // Up
                if (_input.IsValidCoordinate(x, y - 3) && new string(_input.GetVerticalSlice(x, y, y - 3)) == "XMAS")
                    count++;

                // Down
                if (_input.IsValidCoordinate(x, y + 3) && new string(_input.GetVerticalSlice(x, y, y + 3)) == "XMAS")
                    count++;

                for (var i = -3; i <= 3; i += 6)
                {
                    // Top Left && Bottom Right
                    if (_input.IsValidCoordinate(x + i, y + i) && new string(_input.GetDiagonalSlice(x, y, x + i, y + i)) == "XMAS")
                        count++;

                    // Top Right && Bottom Left
                    if (_input.IsValidCoordinate(x + i, y + i * -1) && new string(_input.GetDiagonalSlice(x, y, x + i, y + i * -1)) == "XMAS")
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
                
                var surroundingCharacterCoordinates = _input.GetSurroundingDiagonalValues(x, y).ToList();

                if (surroundingCharacterCoordinates.Count != 4)
                    continue;

                var masCount = surroundingCharacterCoordinates.Where(coordinate => _input[coordinate.Item2][coordinate.Item1] == 'M')
                    .Select(coordinate =>
                    {
                        var xDirection = x - coordinate.Item1;
                        var yDirection = y - coordinate.Item2;
                        return _input.GetDiagonalSlice(coordinate.Item1, coordinate.Item2, coordinate.Item1 + xDirection * 2, coordinate.Item2 + yDirection * 2);
                    })
                    .Count(diagonalSlice => new string(diagonalSlice) == "MAS" || new string(diagonalSlice) == "SAM");

                if (masCount == 2)
                    count++;
            }
        }

        return new(count.ToString());
    }
}
