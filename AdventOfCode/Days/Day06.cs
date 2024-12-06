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
    private readonly (int, int) _nonExistantBlockade = (int.MaxValue, int.MaxValue);
    private (int, int) _currentPosition;
    private char _direction;
    private char[][] _grid;
    private Dictionary<char, List<(int, int)>> _knownTurnPoints;

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
        _knownTurnPoints = new()
        {
            {'^', [] },
            {'>', [] },
            {'v', [] },
            {'<', [] },
        };

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
        var validPosition = true;
        var potentialBlockageCoordinates = new List<(int, int)>();
        var count = 0;
        _grid = _input.Select(x => x.ToArray()).ToArray();
        _direction = '^';
        _currentPosition = _startingPosition;
        _knownTurnPoints = new()
        {
            {'^', [] },
            {'>', [] },
            {'v', [] },
            {'<', [] },
        };

        while (validPosition)
        {
            var directionToMove = _direction is '^' or '<' ? -1 : 1;

            switch (_direction)
            {
                case '^' or 'v':
                    {
                        validPosition = IsValidPosition(directionToMove, 0);

                        var blockadeCoordinates = BlockadeExists(0, _direction is 'v' ? -1 : 1);
                        if (blockadeCoordinates != _nonExistantBlockade)
                        {
                            if (_knownTurnPoints[_positionRotation[_direction]].Contains(blockadeCoordinates))
                                potentialBlockageCoordinates.Add((_currentPosition.Item1 + directionToMove, _currentPosition.Item2));
                            else
                            {

                            }
                        }

                        //ArrayHelper.ArrayPrinter(_grid);

                        break;
                    }
                case '>' or '<':
                    {
                        validPosition = IsValidPosition(0, directionToMove);

                        var blockadeCoordinates = BlockadeExists(_direction is '<' ? -1 : 1, 0);
                        if (blockadeCoordinates != _nonExistantBlockade && _knownTurnPoints[_positionRotation[_direction]].Contains(blockadeCoordinates))
                        {
                            potentialBlockageCoordinates.Add((_currentPosition.Item1, _currentPosition.Item2 + directionToMove));
                        }

                        break;
                    }
            }

            if (!validPosition || _grid[_currentPosition.Item1][_currentPosition.Item2] == 'x') continue;

            _grid[_currentPosition.Item1][_currentPosition.Item2] = 'x';
            count++;

            //Console.Clear();
            //ArrayHelper.ArrayPrinter(_grid);
        }

        return new(potentialBlockageCoordinates.Count.ToString());
    }

    private bool IsValidPosition(int iDirection, int jDirection)
    {
        if (ArrayHelper.IsValidCoordinate(_currentPosition.Item1 + iDirection, _currentPosition.Item2 + jDirection, _grid))
        {
            _currentPosition.Item1 += iDirection;
            _currentPosition.Item2 += jDirection;

            if (_grid[_currentPosition.Item1][_currentPosition.Item2] == '#')
            {
                _currentPosition = (_currentPosition.Item1 - iDirection, _currentPosition.Item2 - jDirection);

                _knownTurnPoints[_direction].Add(_currentPosition);

                _direction = _positionRotation[_direction];

            }

            return true;
        }
        else
            return false;
    }

    private (int, int) BlockadeExists(int iDirection, int jDirection)
    {
        var nextI = _currentPosition.Item1 + iDirection;
        var nextJ = _currentPosition.Item2 + jDirection;

        while (ArrayHelper.IsValidCoordinate(nextI, nextJ, _grid))
        {
            if (_grid[nextI][nextJ] == '#')
                return (nextI - iDirection, nextJ - jDirection);

            nextI += iDirection;
            nextJ += jDirection;
        }

        return _nonExistantBlockade;
    }

}