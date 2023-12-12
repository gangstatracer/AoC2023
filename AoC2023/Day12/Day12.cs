namespace AoC2023.Day12;

public class Day12
{
    [TestCase("Day12/example.txt", 21)]
    [TestCase("Day12/input.txt", 6827)]
    public void Part1(string path, int expected)
    {
        File
            .ReadLines(path)
            .Select(l => l
                .Halve(" ", null, s => s
                    .Split(',')
                    .Select(int.Parse)
                    .ToArray()))
            .Select(t => CountPossibleArrangements(t.Item1, t.Item2))
            .Sum()
            .Should()
            .Be(expected);
    }

    [TestCase("Day12/example.txt", 525152)]
    [TestCase("Day12/input.txt", 6827)]
    public void Part2(string path, long expected)
    {
        File
            .ReadLines(path)
            .Select(l => l
                .Halve(" ",
                    s => string.Join('?', Enumerable.Repeat(s, 5)),
                    s => string.Join(',', Enumerable.Repeat(s, 5))
                        .Split(',')
                        .Select(int.Parse)
                        .ToArray()))
            .Select((t, i) =>
            {
                Console.WriteLine($"Processing row {i}");
                return CountPossibleArrangements(t.Item1, t.Item2);
            })
            .Sum()
            .Should()
            .Be(expected);
    }

    private static long CountPossibleArrangements(string condition, int[] damagedGroups)
    {
        var binary = condition
            .Select(c => c switch
            {
                '.' => '0',
                '#' => '1',
                _ => c
            })
            .ToArray();

        var wildcards = binary
            .Select((c, i) => (c, i))
            .Where(t => t.c == '?')
            .Select(t => t.i)
            .ToArray();

        var count = 0L;

        var combinations = GenerateCombinations(
            wildcards.Length,
            damagedGroups.Sum() - binary.Count(c => c == '1'));

        foreach (var combination in combinations)
        {
            for (var i = 0; i < wildcards.Length; i++)
            {
                binary[wildcards[i]] = combination[i];
            }

            if (Matches(binary, damagedGroups))
                count++;
        }

        return count;
    }

    private static IEnumerable<string> GenerateCombinations(int length, int limit)
    {
        for (var i = 0; i < Math.Pow(2, length); i++)
        {
            var count = 0;
            var a = i;
            while (a > 0)
            {
                if (a % 2 == 1) count++;
                a /= 2;
            }

            if (count == limit)
                yield return Convert.ToString(i, 2).PadLeft(length, '0');
        }
    }

    private static bool Matches(char[] condition, int[] damagedGroups)
    {
        var currentGroup = 0;
        for (var i = 0; i < condition.Length; i++)
            if (condition[i] == '1')
            {
                var groupLength = 0;
                while (i < condition.Length && condition[i] == '1')
                {
                    groupLength++;
                    i++;
                }

                if (currentGroup >= damagedGroups.Length
                    || damagedGroups[currentGroup] != groupLength)
                    return false;

                currentGroup++;
            }

        return currentGroup == damagedGroups.Length;
    }
}