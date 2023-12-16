namespace AoC2023.Day16;

public class Day16
{
    [TestCase("Day16/example.txt", 46)]
    [TestCase("Day16/input.txt", 8116)]
    public void Part1(string path, int expected)
    {
        var input = File.ReadLines(path).ToArray();

        CountEnergy(
                (0, 0, (0, 1)),
                input)
            .Should()
            .Be(expected);
    }

    [TestCase("Day16/example.txt", 51)]
    [TestCase("Day16/input.txt", 8383)]
    public void Part2(string path, int expected)
    {
        var input = File.ReadLines(path).ToArray();

        var width = input[0].Length;
        var height = input.Length;
        Array.Empty<(int, int, (int, int))>()
            .Concat(Enumerable.Range(0, width).Select(i => (0, i, (1, 0))))
            .Concat(Enumerable.Range(0, width).Select(i => (height - 1, i, (-1, 0))))
            .Concat(Enumerable.Range(0, height).Select(i => (i, 0, (0, 1))))
            .Concat(Enumerable.Range(0, height).Select(i => (i, width - 1, (0, -1))))
            .Select(s => CountEnergy(s, input))
            .Max()
            .Should()
            .Be(expected);
    }

    private static int CountEnergy((int, int, (int, int)) start, string[] input)
    {
        var beams = new List<(int, int, (int, int))> { start };
        var energy = Enumerable
            .Range(0, input.Length)
            .Select(_ => new Direction[input[0].Length])
            .ToArray();
        while (beams.Any())
        {
            var nextBeams = new List<(int, int, (int, int))>();
            foreach (var beam in beams)
            {
                var currentDirection = GetDirection(beam.Item3);
                var tile = energy[beam.Item1][beam.Item2];
                if (tile.HasFlag(currentDirection))
                    continue;

                energy[beam.Item1][beam.Item2] |= currentDirection;

                var directions = input[beam.Item1][beam.Item2] switch
                {
                    '.' => new[] { beam.Item3 },
                    '\\' => new[] { (beam.Item3.Item2, beam.Item3.Item1) },
                    '/' => new[] { (beam.Item3.Item2 * -1, beam.Item3.Item1 * -1) },
                    '-' => beam.Item3 switch
                    {
                        (0, 1) => new[] { beam.Item3 },
                        (0, -1) => new[] { beam.Item3 },
                        (1, 0) => new[] { (0, -1), (0, 1) },
                        (-1, 0) => new[] { (0, -1), (0, 1) },
                    },
                    '|' => beam.Item3 switch
                    {
                        (0, 1) => new[] { (-1, 0), (1, 0) },
                        (0, -1) => new[] { (-1, 0), (1, 0) },
                        (1, 0) => new[] { beam.Item3 },
                        (-1, 0) => new[] { beam.Item3 },
                    },
                };
                nextBeams
                    .AddRange(directions
                        .Select(d => (beam.Item1 + d.Item1, beam.Item2 + d.Item2, d))
                        .Where(b => b.Item1 >= 0
                                    && b.Item1 < input.Length
                                    && b.Item2 >= 0
                                    && b.Item2 < input[0].Length));
            }

            beams = nextBeams;
        }

        var totalEnergy = energy
            .Sum(r => r.Count(c => c != Direction.None));
        return totalEnergy;
    }

    private static Direction GetDirection((int, int) vector)
    {
        return vector switch
        {
            (0, -1) => Direction.Left,
            (-1, 0) => Direction.Up,
            (0, 1) => Direction.Right,
            (1, 0) => Direction.Down,
        };
    }

    [Flags]
    private enum Direction
    {
        None = 0,
        Left = 1 << 0,
        Up = 1 << 1,
        Right = 1 << 2,
        Down = 1 << 3,
    }
}