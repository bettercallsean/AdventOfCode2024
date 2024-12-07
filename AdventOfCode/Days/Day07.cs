using AdventOfCode.Utilities.ExtensionMethods;

namespace AdventOfCode.Days;

public class Day07 : BaseDay
{
    private readonly List<Equation> _input;
    private readonly Dictionary<int, char> _numberToOperator = new()
    {
        { 1, '+' },
        { 2, '*' },
        { 3, '|' }
    };
    private readonly Dictionary<int, HashSet<List<int>>> _operatorCombinations;
    private long _part1Answer;

    public Day07()
    {
        _input = File.ReadAllLines(InputFilePath)
            .Select(x => x.Split(": "))
            .Select(x => new Equation
            {
                Answer = long.Parse(x[0]),
                Values = x[1].Split(' ').Select(int.Parse).ToList()
            })
            .ToList();

        _operatorCombinations = [];
    }
    
    public override ValueTask<string> Solve_1()
    {
        var result = 0L;

        for (var i = 0; i < _input.Count; i++)
        {
            var equation = _input[i];
            foreach (var combination in GetOperatorCombinations(equation.Values.Count - 1))
            {
                long total = equation.Values[0];
                for (var j = 0; j < combination.Count; j++)
                {
                    var nextValue = equation.Values[j + 1];
                    switch (_numberToOperator[combination[j]])
                    {
                        case '+':
                            total += nextValue;
                            break;
                        case '*':
                            total *= nextValue;
                            break;
                    }
                }

                if (total != equation.Answer) continue;

                result += equation.Answer;
                
                _input.Remove(equation);
                i--;
                
                break;
            }
        }

        _part1Answer = result;

        return new(_part1Answer.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var result = 0L;
        
        foreach (var equation in _input)
        {
            foreach (var combination in GetOperatorCombinations(equation.Values.Count - 1))
            {
                long total = equation.Values[0];
                for (var j = 0; j < combination.Count; j++)
                {
                    var nextValue = equation.Values[j + 1];
                    switch (_numberToOperator[combination[j]])
                    {
                        case '+':
                            total += nextValue;
                            break;
                        case '*':
                            total *= nextValue;
                            break;
                        default:
                            total = long.Parse($"{total}{nextValue}");
                            break;
                    }
                }

                if (total != equation.Answer) continue;

                result += equation.Answer;

                break;
            }
        }

        return new((result + _part1Answer).ToString());
    }

    private HashSet<List<int>> GetOperatorCombinations(int numberOfOperators)
    {
        if(_operatorCombinations.TryGetValue(numberOfOperators, out var combinations))
           return combinations;
        
        if (numberOfOperators == 1)
        {
            _operatorCombinations.Add(numberOfOperators, [
                new() { 1 },
                new() { 2 },
                new() { 3 }
            ]); }
        else
            _operatorCombinations.Add(numberOfOperators, Enumerable.Range(1, 3).Permute(numberOfOperators).Select(x => x.ToList()).ToHashSet());
        
        return _operatorCombinations[numberOfOperators];
    }
}

internal class Equation
{
    public long Answer { get; init; }
    public List<int> Values { get; init; }
}