namespace AdventOfCode.Days;

public class Day14 : BaseDay
{
    private const int GridWidth = 101;
    private const int GridHeight = 103;
    
    private List<Robot> _input;

    public Day14()
    {
        _input = GetRobotsList();
    }
    
    public override ValueTask<string> Solve_1()
    {
        for (var timer = 0; timer < 100; timer++)
        {
            foreach (var robot in _input)
            {
                robot.Position = UpdateRobotPosition(robot);
            }
        }
        
        var safetyScore = GetSafetyScore(_input);

        return new(safetyScore.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        _input = GetRobotsList();
        
        var grid = new char[GridHeight][];
        for (var i = 0; i < GridHeight; i++)
        {
            grid[i] = new char[GridWidth];
            Array.Fill(grid[i], '.');
        }

        var iteration = 0;
        var scores = new Dictionary<int, int>();
        while (iteration < 15000)
        {
            foreach (var robot in _input)
            {
                grid[robot.Position.Y][robot.Position.X] = '.';
                robot.Position = UpdateRobotPosition(robot);
                grid[robot.Position.Y][robot.Position.X] = '#';
            }
            
            iteration++;
            
            var safetyScore = GetSafetyScore(_input);

            scores.Add(iteration, safetyScore);
        }
        
        scores = scores.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

        return new(scores.Select(x => x.Key).FirstOrDefault().ToString());
    }

    private static (int, int) UpdateRobotPosition(Robot robot)
    {
        var newPosition = robot.Position;

        newPosition.X += robot.Velocity.X;
        newPosition.Y += robot.Velocity.Y;

        if (newPosition.Y is < 0 or >= GridHeight)
        {
            if (newPosition.Y < 0)
                newPosition.Y = GridHeight - newPosition.Y * -1;
            else
                newPosition.Y = 0 + newPosition.Y - GridHeight;
        }

        if (newPosition.X is < 0 or >= GridWidth)
        {
            if (newPosition.X < 0)
                newPosition.X = GridWidth - newPosition.X * -1;
            else
                newPosition.X = 0 + newPosition.X - GridWidth;
        }
                
        return newPosition;
    }

    private List<Robot> GetRobotsList()
    {
        return  File.ReadAllLines(InputFilePath)
            .Select(x => x.Split(' ').Select(x => x.Split('=')).ToList())
            .Select(x => new
            {
                Position = x[0][1].Split(','),
                Velocity = x[1][1].Split(',')
            })
            .Select(x => new Robot
            {
                Position = (int.Parse(x.Position[0]), int.Parse(x.Position[1])),
                Velocity = (int.Parse(x.Velocity[0]), int.Parse(x.Velocity[1])),
            })
            .ToList();
    }

    private int GetSafetyScore(List<Robot> robots)
    {
        var quadrants = new int[4];
        var robotPositions = _input.Select(x => x.Position).ToList();
        foreach (var robot in robotPositions)
        {
            switch (robot.X)
            {
                case >= 0 and < GridWidth / 2 when robot.Y is >= 0 and < GridHeight / 2:
                    quadrants[0]++;
                    break;
                case > GridWidth / 2 and < GridWidth when robot.Y is >= 0 and < GridHeight / 2:
                    quadrants[1]++;
                    break;
                case >= 0 and < GridWidth / 2 when robot.Y is > GridHeight / 2 and < GridHeight:
                    quadrants[2]++;
                    break;
                case > GridWidth / 2 and < GridWidth when robot.Y is > GridHeight / 2 and < GridHeight:
                    quadrants[3]++;
                    break;
            }
        }
        
        return quadrants.Aggregate((x, y) => x * y);
    }
}

internal class Robot
{
    public (int X, int Y) Position { get; set; }
    public (int X, int Y) Velocity { get; set; }
}