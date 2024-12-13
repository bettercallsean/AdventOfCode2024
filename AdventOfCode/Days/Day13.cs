namespace AdventOfCode.Days;

public class Day13 : BaseDay
{
    private readonly List<Grabber> _input;

    public Day13()
    {
        _input = File.ReadAllText(InputFilePath)
            .Split($"{Environment.NewLine}{Environment.NewLine}")
            .Select(x => x.Split(Environment.NewLine).ToList())
            .Select(x => new
            {
                ButtonA = (int.Parse(x[0][12..14]), int.Parse(x[0][18..20])),
                ButtonB = (int.Parse(x[1][12..14]), int.Parse(x[1][18..20])),
                Prize = x[2].Split("Prize: X=")[1].Split(", Y=").Select(long.Parse).ToList(),
            })
            .Select(x => new Grabber
            {
                ButtonA = x.ButtonA,
                ButtonB = x.ButtonB,
                Prize = (x.Prize[0], x.Prize[1])
            })
            .ToList();
    }

    public override ValueTask<string> Solve_1()
    {
        var total = 0L;

        foreach (var grabber in _input)
        {
            var presses = GetMinNumberOfButtonPresses(grabber.Prize, grabber.ButtonA, grabber.ButtonB);

            if (presses.Item1 * grabber.ButtonA.X + presses.Item2 * grabber.ButtonB.X != grabber.Prize.X
                || presses.Item1 * grabber.ButtonA.Y + presses.Item2 * grabber.ButtonB.Y != grabber.Prize.Y)
                continue;

            total += presses.Item1 * 3 + presses.Item2;
        }

        return new(total.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var total = 0L;

        foreach (var grabber in _input)
        {
            var presses = GetMinNumberOfButtonPresses((grabber.Prize.X + 10000000000000, grabber.Prize.Y + 10000000000000), grabber.ButtonA, grabber.ButtonB);

            if (presses.Item1 * grabber.ButtonA.X + presses.Item2 * grabber.ButtonB.X != grabber.Prize.X + 10000000000000
                || presses.Item1 * grabber.ButtonA.Y + presses.Item2 * grabber.ButtonB.Y != grabber.Prize.Y + 10000000000000)
                continue;

            total += presses.Item1 * 3 + presses.Item2;
        }

        return new(total.ToString());
    }

    private static (long, long) GetMinNumberOfButtonPresses((long X, long Y) prize, (int X, int Y) buttonA, (int X, int Y) buttonB)
    {
        var presses = (0L, 0L);

        presses.Item1 = (prize.X * buttonB.Y - prize.Y * buttonB.X) / (buttonA.X * buttonB.Y - buttonA.Y * buttonB.X);
        presses.Item2 = (buttonA.X * prize.Y - buttonA.Y * prize.X) / (buttonA.X * buttonB.Y - buttonA.Y * buttonB.X);

        return presses;
    }
}

internal class Grabber
{
    public (int X, int Y) ButtonA { get; set; }
    public (int X, int Y) ButtonB { get; set; }
    public (long X, long Y) Prize { get; set; }
}