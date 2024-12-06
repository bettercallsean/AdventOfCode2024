using AdventOfCode.Utilities.Helpers;

namespace AdventOfCode.Days;

public class Day06 : BaseDay
{
    private readonly char[][] _input;
    private readonly Dictionary<char, char> _positionRotation = new()
    {
        { '^', '>' },
        { '>', 'v' },
        { 'v', '<' },
        { '<', '^' }
    };
    private readonly (int, int) _startingPosition;
    private (int, int) _currentPosition;
    private char _direction;
    private char[][] _grid;

    public Day06()
    {
        _input = File.ReadAllLines(InputFilePath).Select(line => line.ToCharArray()).ToArray();

        for (var i = 0; i < _input.Length; i++)
        {
            for (var j = 0; j < _input[0].Length; j++)
            {
                if (_input[i][j] != '^') continue;

                _startingPosition = (i, j);
                break;
            }
        }
    }

    public override ValueTask<string> Solve_1()
    {
        var validPosition = true;
        var count = 0;
        _direction = '^';
        _grid = _input.Select(x => x.ToArray()).ToArray();
        _currentPosition = _startingPosition;

        while (validPosition)
        {
            var directionToMove = _direction is '^' or '<' ? -1 : 1;

            switch (_direction)
            {
                case '^' or 'v':
                    {
                        validPosition = IsValidPosition(directionToMove, 0);

                        break;
                    }
                case '>' or '<':
                    {
                        validPosition = IsValidPosition(0, directionToMove);

                        break;
                    }
            }

            if (!validPosition || _grid[_currentPosition.Item1][_currentPosition.Item2] == 'x') continue;

            _grid[_currentPosition.Item1][_currentPosition.Item2] = 'x';
            count++;
        }

        return new(count.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
    }

    private bool IsValidPosition(int iDirection, int jDirection)
    {
        if (ArrayHelper.IsValidCoordinate(_currentPosition.Item1 + iDirection, _currentPosition.Item2 + jDirection, _grid))
        {
            _currentPosition.Item1 += iDirection;
            _currentPosition.Item2 += jDirection;

            if (_grid[_currentPosition.Item1][_currentPosition.Item2] == '#')
            {
                _direction = _positionRotation[_direction];
                _currentPosition = (_currentPosition.Item1 - iDirection, _currentPosition.Item2 - jDirection);
            }

            return true;
        }
        else
            return false;
    }

    private bool BlockadeExists(int iDirection, int jDirection)
    {
        var nextI = _currentPosition.Item1 + iDirection;
        var nextJ = _currentPosition.Item2 + jDirection;

        while (ArrayHelper.IsValidCoordinate(nextI, nextJ, _grid))
        {
            if (_grid[nextI][nextJ] == '#')
                return true;

            nextI = nextI + iDirection;
            nextJ = nextJ + jDirection;
        }

        return false;
    }
}