using System.Security.Principal;
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
    private (int, int) _position;
    
    public Day06()
    {
        _input = File.ReadAllLines(InputFilePath).Select(line => line.ToCharArray()).ToArray();

        for (var i = 0; i < _input.Length; i++)
        {
            for (var j = 0; j < _input[0].Length; j++)
            {
                if (_input[i][j] != '^') continue;
                
                _position = (i, j);
                break;
            }
        }
    }
    
    public override ValueTask<string> Solve_1()
    {
        var outOfBounds = false;
        var direction = '^';
        var count = 0;
        
        while (!outOfBounds)
        {
            var directionToMove = direction is '^' or '<' ? -1 : 1;

            switch (direction)
            {
                case '^' or 'v':
                {
                    if (ArrayHelper.IsValidCoordinate(_position.Item1 + directionToMove, _position.Item2, _input))
                    {
                        _position.Item1 += directionToMove;

                        if (_input[_position.Item1][_position.Item2] == '#')
                        {
                            _position = (_position.Item1 - directionToMove, _position.Item2);
                            direction = _positionRotation[direction];
                        }
                    }
                    else
                        outOfBounds = true;

                    break;
                }
                case '>' or '<':
                {
                    if (ArrayHelper.IsValidCoordinate(_position.Item1, _position.Item2 + directionToMove, _input))
                    {
                        _position.Item2 += directionToMove;

                        if (_input[_position.Item1][_position.Item2] == '#')
                        {
                            _position = (_position.Item1, _position.Item2 - directionToMove);
                            direction = _positionRotation[direction];
                        }
                    }
                    else
                        outOfBounds = true;

                    break;
                }
            }
            
            if (_input[_position.Item1][_position.Item2] == 'x') continue;
                        
            _input[_position.Item1][_position.Item2] = 'x';
            count++;
        }

        return new(count.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        throw new NotImplementedException();
    }
}