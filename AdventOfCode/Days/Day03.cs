using System.Text.RegularExpressions;

namespace AdventOfCode.Days;
public partial class Day03 : BaseDay
{
    private readonly string _input;

    public Day03()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        var functions = FunctionRegex().Matches(_input);

        var result = functions.ToList().Sum(function => int.Parse(function.Groups[1].Value) * int.Parse(function.Groups[2].Value));

        return new(result.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        const string DoFunction = "do()";

        var functions = FunctionRegex().Matches(_input).ToList();
        var doDonts = DoDontRegex().Matches(_input).ToList();

        var doDontIndex = 0;
        var result = 0;
        var doFunction = true;

        foreach (var function in functions)
        {
            if (function.Index >= doDonts[doDontIndex].Index)
            {
                doFunction = doDonts[doDontIndex].Value == DoFunction;
                if (doDontIndex < doDonts.Count - 1)
                    doDontIndex++;
            }

            if (doFunction)
                result += int.Parse(function.Groups[1].Value) * int.Parse(function.Groups[2].Value);
        }

        return new(result.ToString());
    }

    [GeneratedRegex(@"mul\((\d{1,3}),(\d{1,3})\)")]
    private static partial Regex FunctionRegex();

    [GeneratedRegex(@"do\(\)|don't\(\)")]
    private static partial Regex DoDontRegex();
}
