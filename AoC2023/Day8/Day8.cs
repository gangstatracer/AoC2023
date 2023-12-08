namespace AoC2023.Day8;

public class Day8
{
    [TestCase("Day8/example.txt", 2)]
    [TestCase("Day8/example2.txt", 6)]
    [TestCase("Day8/input.txt", 13771)]
    public void Part1(string path, int expected)
    {
        var input = File.ReadLines(path).ToArray();
        var instructions = input[0]
            .Select(c => c == 'L' ? 0 : 1)
            .ToArray();
        var map = new Dictionary<string, string[]>();
        foreach (var line in input.Skip(2))
        {
            var (node, next) = line.Halve(" = ");
            var (left, right) = next.Substring(1, 8).Halve(", ");
            map.Add(node, new[] { left, right });
        }

        var location = "AAA";
        var current = 0;
        var steps = 0;
        while (location != "ZZZ")
        {
            location = map[location][instructions[current]];
            current++;
            current %= instructions.Length;
            steps++;
        }

        steps.Should().Be(expected);
    }


    [TestCase("Day8/input.txt", 13129439557681L)]
    [TestCase("Day8/example3.txt", 6)]
    public void Part2(string path, long expected)
    {
        var input = File.ReadLines(path).ToArray();
        var instructions = input[0]
            .Select(c => c == 'L' ? 0 : 1)
            .ToArray();
        var map = new Dictionary<string, string[]>();
        foreach (var line in input.Skip(2))
        {
            var (node, next) = line.Halve(" = ");
            var (left, right) = next.Substring(1, 8).Halve(", ");
            map.Add(node, new[] { left, right });
        }

        var locations = map
            .Keys
            .Where(k => k.EndsWith('A'))
            .ToDictionary(
                l => l,
                _ => (FromStart: 0, LoopLength: 0));

        foreach (var l in locations.Keys)
        {
            var location = l;
            var current = 0;
            var steps = 0;
            while (!location.EndsWith('Z'))
            {
                location = map[location][instructions[current]];
                current++;
                current %= instructions.Length;
                steps++;
            }

            var stepsToReachZ = steps;
            steps = 0;
            do
            {
                location = map[location][instructions[current]];
                current++;
                current %= instructions.Length;
                steps++;
            } while (!location.EndsWith('Z'));

            locations[l] = (FromStart: stepsToReachZ, LoopLength: steps);
        }

        locations
            .Values
            .Select(t => t.LoopLength)
            .SelectMany(GetFactors)
            .GroupBy(
                t => t.Number,
                (number, powers) => (long)Math
                    .Pow(
                        number,
                        powers
                            .Select(tt => tt.Power)
                            .Max()))
            .Aggregate(1L, (k, l) => k * l)
            .Should()
            .Be(expected);
    }

    private static IEnumerable<(int Number, int Power)> GetFactors(int number)
    {
        for (var i = 2; number > 1; i++)
            if (number % i == 0)
            {
                var x = 0;
                while (number % i == 0)
                {
                    number /= i;
                    x++;
                }

                yield return (i, x);
            }
    }
}