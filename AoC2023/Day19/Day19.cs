namespace AoC2023.Day19;

public class Day19
{
    [TestCase("Day19/example.txt", 19114)]
    [TestCase("Day19/input.txt", 432434)]
    public void Part1(string path, int expected)
    {
        var input = File.ReadLines(path).ToArray();
        var workflows = new Dictionary<string, List<Instruction>>();
        var i = 0;
        while (!string.IsNullOrEmpty(input[i]))
        {
            var (id, rest) = input[i].Halve("{");

            var instructions = rest[..^1]
                .Split(',')
                .ToArray();
            workflows[id] = instructions
                .Take(instructions.Length - 1)
                .Select(s =>
                {
                    var (condition, workflow) = s.Halve(":");
                    Func<Gear, int> propertyGetter = condition[0] switch
                    {
                        'x' => g => g.X,
                        'm' => g => g.M,
                        'a' => g => g.A,
                        's' => g => g.S,
                    };
                    Func<int, int, bool> comparision = condition[1] switch
                    {
                        '<' => (a, b) => a < b,
                        '>' => (a, b) => a > b,
                    };
                    var number = int.Parse(condition[2..]);
                    return new Instruction(
                        g => comparision(
                            propertyGetter(g),
                            number),
                        workflow);
                })
                .Append(new Instruction(_ => true, instructions[^1]))
                .ToList();
            i++;
        }

        i++;
        var gears = input[i..]
            .Select(l =>
            {
                var parts = l[1..^1]
                    .Split(',')
                    .Select(p => p
                        .Halve("=", null, int.Parse)
                        .Item2)
                    .ToArray();
                return new Gear(X: parts[0], M: parts[1], A: parts[2], S: parts[3]);
            })
            .ToArray();

        var sum = 0;
        foreach (var gear in gears)
        {
            var workflowId = "in";
            var instructionIndex = 0;
            do
            {
                var instruction = workflows[workflowId][instructionIndex];
                if (instruction.Predicate(gear))
                {
                    workflowId = instruction.Result;
                    instructionIndex = 0;
                }
                else
                {
                    instructionIndex++;
                }
            } while (workflowId != "R" && workflowId != "A");

            if (workflowId == "A")
            {
                var (x, m, a, s) = gear;
                sum += x + m + a + s;
            }
        }

        sum.Should().Be(expected);
    }

    private record Instruction(Func<Gear, bool> Predicate, string Result);

    private record Gear(int X, int M, int A, int S);

    [TestCase("Day19/example.txt", 167409079868000Lu)]
    [TestCase("Day19/input.txt", 132557544578569Lu)]
    public void Part2(string path, ulong expected)
    {
        var input = File.ReadLines(path).ToArray();
        var workflows = new Dictionary<string, List<(string, string)>>();
        var i = 0;
        while (!string.IsNullOrEmpty(input[i]))
        {
            var (id, rest) = input[i].Halve("{");

            var instructions = rest[..^1]
                .Split(',')
                .ToArray();
            workflows[id] = instructions
                .Take(instructions.Length - 1)
                .Select(s =>
                {
                    var (condition, workflow) = s.Halve(":");

                    return (condition, workflow);
                })
                .Append(("", instructions[^1]))
                .ToList();
            i++;
        }

        var paths = new List<(string, List<string>)> { ("in", new List<string>()) };
        while (paths.Any(t => t.Item1 != "A"))
        {
            var nextPaths = paths.Where(t => t.Item1 == "A").ToList();
            foreach (var p in paths.Where(t => t.Item1 != "A"))
            {
                var previousNegated = new List<string>();
                foreach (var instruction in workflows[p.Item1])
                {
                    var conditions = new List<string>(
                        p
                            .Item2
                            .Concat(previousNegated)
                            .Append(instruction.Item1));
                    nextPaths.Add((instruction.Item2, conditions));
                    previousNegated.Add(Negate(instruction.Item1));
                }
            }

            paths = nextPaths
                .Where(t => t.Item1 != "R")
                .ToList();
        }

        var combinations = 0LU;
        foreach (var p in paths)
        {
            var limits = new Dictionary<char, (int Min, int Max)>
            {
                ['x'] = (0, 4001),
                ['m'] = (0, 4001),
                ['a'] = (0, 4001),
                ['s'] = (0, 4001),
            };
            var conditions = p
                .Item2
                .Where(c => !string.IsNullOrEmpty(c))
                .ToList();
            foreach (var c in conditions)
            {
                var (min, max) = limits[c[0]];
                var number = int.Parse(c[2..]);
                switch (c[1])
                {
                    case '<':
                        max = number < max ? number : max;
                        break;
                    case '>':
                        min = number > min ? number : min;
                        break;
                }

                limits[c[0]] = (min, max);
            }

            combinations += limits.Aggregate(
                1Lu,
                (c, t) =>
                {
                    var range = t.Value.Max - t.Value.Min - 1;
                    if (range < 0)
                        return 0;
                    return c * Convert.ToUInt64(range);
                });
        }

        combinations.Should().Be(expected);
    }

    private static string Negate(string condition)
    {
        if (string.IsNullOrEmpty(condition))
            return condition;

        var number = int.Parse(condition[2..]);
        return condition[1] switch
        {
            '<' => "" + condition[0] + '>' + (number - 1),
            '>' => "" + condition[0] + '<' + (number + 1),
        };
    }
}