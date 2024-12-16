namespace AdventOfCode.Days;

public class Day15 : BaseDay
{
    private readonly char[] _moves;
    private readonly Dictionary<char, (int i, int j)> _movementDirection = new()
    {
        {'<', (0, -1) },
        {'^', (-1, 0) },
        {'>', (0, 1) },
        {'v', (1, 0) },
    };

    private char[][] _input;
    private (int i, int j) _start;

    public Day15()
    {
        var input = File.ReadAllText(InputFilePath).Split($"{Environment.NewLine}{Environment.NewLine}").ToList();

        _input = input[0].Split(Environment.NewLine).Select(x => x.ToCharArray()).ToArray();
        _moves = input[1].Split(Environment.NewLine).SelectMany(x => x.ToCharArray()).ToArray();

        for (int i = 0; i < _input.Length; i++)
        {
            for (int j = 0; j < _input[0].Length; j++)
            {
                if (_input[i][j] == '@')
                {
                    _start = (i, j);
                    break;
                }
            }
        }
    }

    public override ValueTask<string> Solve_1()
    {
        var robotLocation = _start;
        foreach (var move in _moves)
        {
            var movementDirection = _movementDirection[move];
            var nextSpot = (i: robotLocation.i + movementDirection.i, j: robotLocation.j + movementDirection.j);
            if (_input[nextSpot.i][nextSpot.j] == '#') continue;

            if (_input[nextSpot.i][nextSpot.j] == 'O')
            {
                var nextBox = GetLocationOfLastMoveableBox((robotLocation.i + movementDirection.i, robotLocation.j + movementDirection.j), movementDirection);

                if (nextBox == (int.MaxValue, int.MaxValue)) continue;

                while (nextSpot != nextBox)
                {
                    _input[nextBox.i][nextBox.j] = 'O';
                    _input[nextBox.i - movementDirection.i][nextBox.j - movementDirection.j] = '.';
                    nextBox.i -= movementDirection.i;
                    nextBox.j -= movementDirection.j;
                }
            }

            _input[robotLocation.i][robotLocation.j] = '.';

            robotLocation.i += movementDirection.i;
            robotLocation.j += movementDirection.j;

            _input[robotLocation.i][robotLocation.j] = '@';
        }

        var gpsSum = 0;
        for (int i = 0; i < _input.Length; i++)
        {
            for (int j = 0; j < _input[0].Length; j++)
            {
                if (_input[i][j] == 'O')
                    gpsSum += 100 * i + j;
            }
        }

        return new(gpsSum.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
    }

    private (int i, int j) GetLocationOfLastMoveableBox((int i, int j) firstBox, (int i, int j) direction)
    {
        var nextBox = firstBox;

        while (_input[nextBox.i][nextBox.j] != '.' && _input.IsValidCoordinate(nextBox.i + direction.i, nextBox.j + direction.j) && _input[nextBox.i][nextBox.j] != '#')
        {
            nextBox.i += direction.i;
            nextBox.j += direction.j;
        }

        return _input[nextBox.i][nextBox.j] switch
        {
            '#' when _input[nextBox.i - direction.i][nextBox.j - direction.j] != '.' => (int.MaxValue, int.MaxValue),
            '#' => (nextBox.i - direction.i, nextBox.j - direction.j),
            _ => nextBox,
        };
    }

    private void GenerateNewMap()
    {
        var input = File.ReadAllText(InputFilePath).Split($"{Environment.NewLine}{Environment.NewLine}").ToList();
        var oldMap = input[0].Split(Environment.NewLine).Select(x => x.ToCharArray()).ToArray();
        var newMap = new char[_input.Length][];

        for (var i = 0; i < oldMap.Length; i++)
        {
            var level = oldMap[i];
            newMap[i] = new char[oldMap[0].Length * 2];
            var offset = 0;
            for (var j = 0; j < level.Length; j++)
            {
                if (level[j] == '#')
                {
                    newMap[i][j + offset] = '#';
                    newMap[i][j + offset + 1] = '#';
                }
                else if (level[j] == 'O')
                {
                    newMap[i][j + offset] = '[';
                    newMap[i][j + offset + 1] = ']';
                }
                else if (level[j] == '@')
                {
                    newMap[i][j + offset] = '@';
                    newMap[i][j + offset + 1] = '.';

                    _start = (i, j + offset);
                }
                else
                {
                    newMap[i][j + offset] = '.';
                    newMap[i][j + offset + 1] = '.';
                }

                offset++;
            }
        }

        _input = newMap;
    }
}
