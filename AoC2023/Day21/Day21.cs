namespace AoC2023.Day21;

public class Day21
{
    [TestCase("Day21/example.txt", 5, 5, 6, 16)]
    [TestCase("Day21/input.txt", 65, 65, 64, 432434)]
    public void Part1(string path, int x, int y, int limit, int expected)
    {
        var input = File
            .ReadLines(path)
            .Select(l => l.ToCharArray())
            .ToArray();

        var queue = new Queue<(int, int, int)>();
        queue.Enqueue((x, y, 0));
        while (queue.Any(i => i.Item3 != limit))
        {
            var (i, j, count) = queue.Dequeue();
            foreach (var (ii, jj) in new[] { (0, 1), (0, -1), (1, 0), (-1, 0) }
                         .Select(t => (i + t.Item1, j + t.Item2)))
                if (!queue.Any(t => t.Item1 == ii && t.Item2 == jj) && input[ii][jj] != '#')
                    queue.Enqueue((ii, jj, count + 1));
        }

        queue
            .Count
            .Should()
            .Be(expected);
    }
}