using System.Text;

namespace AoC2023.Day18;

public class Day18
{
    [TestCase("Day18/example.txt", 62)]
    [TestCase("Day18/input.txt", 8116)]
    public void Part1(string path, int expected)
    {
        var input = File
            .ReadLines(path)
            .Select(l =>
            {
                var parts = l.Split(" ");
                return (parts[0][0] switch
                    {
                        'R' => (0, 1),
                        'U' => (-1, 0),
                        'L' => (0, -1),
                        'D' => (1, 0),
                    },
                    int.Parse(parts[1]),
                    parts[2].Substring(2, 6));
            })
            .ToArray();

        var x = 0;
        var y = 0;
        var map = new Dictionary<(int, int), string?> { [(0, 0)] = "000000" };
        foreach (var (direction, length, color) in input)
        {
            for (var i = 0; i < length; i++)
            {
                x += direction.Item1;
                y += direction.Item2;
                map[(x, y)] = color;
            }
        }

        var loopCount = map.Keys.Count;

        var maxX = map.Keys.Select(k => k.Item1).Max();
        var minX = map.Keys.Select(k => k.Item1).Min();
        var maxY = map.Keys.Select(k => k.Item2).Max();
        var minY = map.Keys.Select(k => k.Item2).Min();

        BFS(
            minX, maxX,
            minY, maxY,
            map,
            minX + 2,
            minY + 153,
            "000000");

        var innerCount = 0;
        var builder = new StringBuilder();
        for (var i = minX; i <= maxX; i++)
        {
            for (var j = minY; j <= maxY; j++)
            {
                if (!map.TryGetValue((i, j), out var color))
                {
                    innerCount++;
                    builder.Append('O');
                }
                else
                {
                    builder.Append(color == "000000" ? '.' : '#');
                }
            }

            builder.Append(Environment.NewLine);
        }

        File.WriteAllText("Day18/output.txt", builder.ToString());

        (loopCount + innerCount)
            .Should()
            .Be(expected);
    }

    private static void BFS(
        int minX,
        int maxX,
        int minY,
        int maxY,
        IDictionary<(int, int), string?> map,
        int startX,
        int startY,
        string color)
    {
        var queue = new Queue<(int, int)>();
        queue.Enqueue((startX, startY));
        while (queue.Count > 0)
        {
            var (x, y) = queue.Dequeue();
            map[(x, y)] = color;
            for (var i = -1; i < 2; i++)
            for (var j = -1; j < 2; j++)
                if (x + i >= minX - 1
                    && x + i <= maxX + 1
                    && y + j >= minY - 1
                    && y + j <= maxY + 1
                    && !map.ContainsKey((x + i, y + j)))
                    queue.Enqueue((x + i, y + j));
        }
    }
}