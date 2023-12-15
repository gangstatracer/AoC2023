namespace AoC2023.Day15;

public class Day15
{
    [TestCase("Day15/example.txt", 1320)]
    [TestCase("Day15/input.txt", 521341)]
    public void Part1(string path, int expected)
    {
        var input = File.ReadLines(path).ToArray();
        input
            .Single()
            .Split(',')
            .Select(Hash)
            .Sum()
            .Should()
            .Be(expected);
    }

    [TestCase("Day15/example.txt", 145)]
    [TestCase("Day15/input.txt", 252782)]
    public void Part2(string path, int expected)
    {
        var input = File.ReadLines(path).ToArray();
        var boxes = Enumerable.Range(0, 256).Select(_ => new List<(string, int)>()).ToArray();
        var instructions = input
            .Single()
            .Split(',')
            .Select(i => i.EndsWith('-')
                ? (i[..^1], -1)
                : i.Halve("=", null, int.Parse));

        foreach (var instruction in instructions)
        {
            var box = Hash(instruction.Item1);
            var index = boxes[box].FindIndex(t => t.Item1 == instruction.Item1);
            if (index < 0)
            {
                if (instruction.Item2 > 0)
                    boxes[box].Add(instruction);
            }
            else
            {
                if (instruction.Item2 > 0)
                    boxes[box][index] = instruction;
                else
                    boxes[box].RemoveAt(index);
            }
        }

        boxes
            .SelectMany((b, i) => b.Select((t, j) => (i + 1) * (j + 1) * t.Item2))
            .Sum()
            .Should().Be(expected);
    }

    private static int Hash(string s)
    {
        var hash = 0;
        foreach (var c in s)
        {
            hash += c;
            hash *= 17;
            hash %= 256;
        }

        return hash;
    }
}