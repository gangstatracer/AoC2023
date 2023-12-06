namespace AoC2023.Day6;

public class Day6
{
    [TestCase("Day6/example.txt", 288)]
    [TestCase("Day6/input.txt", 252000)]
    public void Part1(string path, int expected)
    {
        var input = File
            .ReadLines(path)
            .ToArray();

        var time = input[0]
            .Halve(":")
            .Item2
            .Split(" ", StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToArray();

        var distance = input[1]
            .Halve(":")
            .Item2
            .Split(" ", StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToArray();

        var product = 1;
        for (var i = 0; i < time.Length; i++)
        {
            var wins = 0;
            for (var j = 1; j < time[i]; j++)
            {
                if (j * (time[i] - j) > distance[i])
                    wins++;
            }

            product *= wins;
        }

        product.Should().Be(expected);
    }
    
    [TestCase("Day6/example.txt", 71503)]
    [TestCase("Day6/input.txt", 36992486)]
    public void Part2(string path, int expected)
    {
        var input = File
            .ReadLines(path)
            .ToArray();

        var time = input[0]
            .Replace(" ","")
            .Halve(":")
            .Item2
            .Split(" ", StringSplitOptions.RemoveEmptyEntries)
            .Select(long.Parse)
            .ToArray();

        var distance = input[1]
            .Replace(" ", "")
            .Halve(":")
            .Item2
            .Split(" ", StringSplitOptions.RemoveEmptyEntries)
            .Select(long.Parse)
            .ToArray();

        var product = 1L;
        for (var i = 0; i < time.Length; i++)
        {
            var wins = 0L;
            for (var j = 1L; j < time[i]; j++)
            {
                if (j * (time[i] - j) > distance[i])
                    wins++;
            }

            product *= wins;
        }

        product.Should().Be(expected);
    }
}