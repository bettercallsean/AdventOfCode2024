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
    private readonly HashSet<(int, int)> _traversedNodes;
    private (int, int) _currentPosition;
    private char _direction;

    public Day06()
    {
        _input = File.ReadAllLines(InputFilePath).Select(line => line.ToCharArray()).ToArray();

        for (var i = 0; i < _input.Length; i++)
        {
            for (var j = 0; j < _input[0].Length; j++)
            {
                if (_input[i][j] != '^') continue;

                _startingPosition = (i, j);
                _input[i][j] = '.';
                break;
            }
        }

        _traversedNodes = new HashSet<(int, int)>();
    }

    public override ValueTask<string> Solve_1()
    {
        var validPosition = true;
        _direction = '^';
        _currentPosition = _startingPosition;

        while (validPosition)
        {
            var directionToMove = _direction is '^' or '<' ? -1 : 1;

            validPosition = _direction switch
            {
                '^' or 'v' => IsValidPosition(directionToMove, 0),
                '>' or '<' => IsValidPosition(0, directionToMove),
                _ => true
            };

            if (!validPosition || _currentPosition == _startingPosition) continue;

            _traversedNodes.Add(_currentPosition);
        }

        return new(_traversedNodes.Count.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var count = 0;
        foreach (var position in _traversedNodes)
        {
            _direction = '^';
            _currentPosition = _startingPosition;
            _input[position.Item1][position.Item2] = '#';

            if (CausesLoop())
                count++;
            
            _input[position.Item1][position.Item2] = '.';
        }

        return new(count.ToString());
    }

    private bool CausesLoop()
    {
        var validPosition = true;
        var traversedNodes =  new Dictionary<(int, int), HashSet<char>>();
        while (validPosition)
        {
            var directionToMove = _direction is '^' or '<' ? -1 : 1;

            validPosition = _direction switch
            {
                '^' or 'v' => IsValidPosition(directionToMove, 0),
                '>' or '<' => IsValidPosition(0, directionToMove),
                _ => true
            };
            
            if(!validPosition) continue;

            if (traversedNodes.TryGetValue(_currentPosition, out HashSet<char> value))
            {
                    if (traversedNodes[_currentPosition].Contains(_direction)) return true;
                    
                value.Add(_direction);
            }
            else
                traversedNodes.Add(_currentPosition, [_direction]);

        }
        
        return false;
    }

    private bool IsValidPosition(int iDirection, int jDirection)
    {
        if (!_input.IsValidCoordinate(_currentPosition.Item1 + iDirection, _currentPosition.Item2 + jDirection)) return false;
        
        _currentPosition.Item1 += iDirection;
        _currentPosition.Item2 += jDirection;

        if (_input[_currentPosition.Item1][_currentPosition.Item2] != '#') return true;
            
        _currentPosition = (_currentPosition.Item1 - iDirection, _currentPosition.Item2 - jDirection);
        _direction = _positionRotation[_direction];

        return true;
    }
}