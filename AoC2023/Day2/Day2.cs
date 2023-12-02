namespace AoC2023.Day2;

public class Day2
{
    [TestCase("Day2/input.txt", 2632)]
    [TestCase("Day2/example.txt", 8)]
    public void Part1(string path, int expected)
    {
        var input = File.ReadLines(path);
        var limits = new Dictionary<string, int>
        {
            ["red"] = 12,
            ["green"] = 13,
            ["blue"] = 14,
        };
        var sum = 0;
        foreach (var line in input)
        {
            var gamePossible = true;
            var (gameIdPart, rest) = line.Halve(": ");
            var turns = rest.Split("; ");
            foreach (var turn in turns)
            {
                var balls = turn.Split(", ");
                foreach (var ball in balls)
                {
                    var (count, color) = ball.Halve(" ", int.Parse);
                    if (limits[color] < count)
                        gamePossible = false;
                }
            }

            if (gamePossible)
                sum += gameIdPart.Halve(" ", null, int.Parse).Item2;
        }

        sum.Should().Be(expected);
    }

    [TestCase("Day2/input.txt", 69629)]
    [TestCase("Day2/example.txt", 2286)]
    public void Part2(string path, int expected)
    {
        var input = File.ReadLines(path);
        var sum = 0;
        foreach (var line in input)
        {
            var power = new Dictionary<string, int>
            {
                ["red"] = 0,
                ["green"] = 0,
                ["blue"] = 0,
            };

            var (_, rest) = line.Halve(": ");
            var turns = rest.Split("; ");
            foreach (var turn in turns)
            {
                var balls = turn.Split(", ");
                foreach (var ball in balls)
                {
                    var (count, color) = ball.Halve(" ", int.Parse);
                    if (power[color] < count)
                        power[color] = count;
                }
            }

            sum += power.Aggregate(1, (i, kvp) => i * kvp.Value);
        }

        sum.Should().Be(expected);
    }
}