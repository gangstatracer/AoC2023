namespace AoC2023.Day9;

public class Day9
{
    [TestCase("Day9/example.txt", 114)]
    [TestCase("Day9/input.txt", 1972648895L)]
    public void Part1(string path, long expected)
    {
        var sum = File.ReadLines(path)
            .Select(l => l.Split(" "))
            .Select(a => a
                .Select(long.Parse)
                .ToArray())
            .Select(GetNext)
            .Sum();

        sum
            .Should()
            .Be(expected);
    }

    private static long GetNext(long[] sequence)
    {
        var height = 1;
        var layers = new List<long[]> { sequence };
        bool hasNonZero;
        do
        {
            hasNonZero = false;
            layers.Add(new long[sequence.Length]);

            for (var i = 0; i < sequence.Length - height; i++)
            {
                layers[height][i] = layers[height - 1][i + 1] - layers[height - 1][i];
                if (!hasNonZero && layers[height][i] != 0)
                    hasNonZero = true;
            }

            height++;
        } while (hasNonZero);

        height--;
        for (var i = height; i > 0; i--)
        {
            var previous = layers[i - 1][^i];
            var step = layers[i][^i];
            var next = previous + step;
            if (i - 1 == 0)
                return next;
            layers[i - 1][^(i - 1)] = next;
        }

        return 0;
    }
    
    [TestCase("Day9/example.txt", 2)]
    [TestCase("Day9/input.txt", 919)]
    public void Part2(string path, long expected)
    {
        var sum = File.ReadLines(path)
            .Select(l => l.Split(" "))
            .Select(a => a
                .Select(long.Parse)
                .ToArray())
            .Select(GetPrevious)
            .Sum();

        sum
            .Should()
            .Be(expected);
    }

    private static long GetPrevious(long[] sequence)
    {
        var height = 1;
        var layers = new List<long[]> { sequence.Reverse().ToArray() };
        bool hasNonZero;
        do
        {
            hasNonZero = false;
            layers.Add(new long[sequence.Length]);

            for (var i = 0; i < sequence.Length - height; i++)
            {
                layers[height][i] = layers[height - 1][i] - layers[height - 1][i + 1];
                if (!hasNonZero && layers[height][i] != 0)
                    hasNonZero = true;
            }

            height++;
        } while (hasNonZero);

        height--;
        for (var i = height; i > 0; i--)
        {
            var previous = layers[i - 1][^i];
            var step = layers[i][^i];
            var next = previous - step;
            if (i - 1 == 0)
                return next;
            layers[i - 1][^(i - 1)] = next;
        }

        return 0;
    }
}