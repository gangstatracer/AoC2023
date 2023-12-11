namespace AoC2023.Day10;

public class Day10
{
    [TestCase("Day10/example.txt", 1, 1, 4)]
    [TestCase("Day10/example2.txt", 2, 0, 8)]
    [TestCase("Day10/input.txt", 23, 114, 6923)]
    public void Part1(string path, int x, int y, int expected)
    {
        var map = File
            .ReadLines(path)
            .ToArray();

        var length = 0;
        WalkLoop(x, y, (_, _, _) => length++, map);
        (length / 2).Should().Be(expected);
    }

    [TestCase("Day10/input.txt", 23, 114, 6923)]
    [TestCase("Day10/example3.txt", 4, 12, 8)]
    [TestCase("Day10/example4.txt", 0, 4, 10)]
    public void Part2(string path, int x, int y, int expected)
    {
        var map = File
            .ReadLines(path)
            .ToArray();
        var marks = new int[map.Length, map[0].Length];

        WalkLoop(x, y, (c, _, _) => marks[c.x, c.y] = 1, map);

        WalkLoop(x, y, (c, p, n) =>
        {
            var leftSide = (p, n) switch
            {
                (Direction.Down, Direction.Up) => (0, -1),
                (Direction.Down, Direction.Left) => (1, -1),
                (Direction.Down, Direction.Right) => (0, -1),
                (Direction.Left, Direction.Up) => (-1, -1),
                (Direction.Left, Direction.Right) => (-1, 0),
                (Direction.Left, Direction.Down) => (-1, 0),
                (Direction.Up, Direction.Right) => (-1, 1),
                (Direction.Up, Direction.Down) => (0, 1),
                (Direction.Up, Direction.Left) => (0, 1),
                (Direction.Right, Direction.Down) => (1, 1),
                (Direction.Right, Direction.Left) => (1, 0),
                (Direction.Right, Direction.Up) => (1, 0),
            };
            var start = (c.x + leftSide.Item1, c.y + leftSide.Item2);
            if (start.Item1 > 0
                && start.Item1 < marks.GetLength(0)
                && start.Item2 > 0
                && start.Item2 < marks.GetLength(1)
                && marks[start.Item1, start.Item2] != 1)
                DFS(marks, start.Item1, start.Item2, 2);
        }, map);

        int leftCount = 0, rightCount = 0;

        for (var i = 0; i < marks.GetLength(0); i++)
        {
            for (var j = 0; j < marks.GetLength(1); j++)
            {
                if (marks[i, j] == 0)
                    leftCount++;
                if (marks[i, j] == 2)
                    rightCount++;
                Console.Write($"{marks[i, j]}");
            }

            Console.WriteLine();
        }

        new []{leftCount, rightCount}.Should().Contain(expected);
    }

    private static void WalkLoop(
        int x,
        int y,
        Action<(int x, int y), Direction, Direction> action,
        string[] map)
    {
        var start = (x, y);
        var current = start;
        var previous = Direction.Left;
        do
        {
            var (d1, d2) = GetDirections(map[current.x][current.y]);
            var next = previous == d1 ? d2 : d1;
            previous = ConvertToFrom(next);

            action(current, previous, next);

            var offset = GetOffset(next);
            current = (current.x + offset.Item1, current.y + offset.Item2);
        } while (current != start);
    }

    private static void DFS(int[,] a, int x, int y, int areaNumber)
    {
        var width = a.GetLength(0);
        var height = a.GetLength(1);

        if (a[x, y] == 0)
            a[x, y] = areaNumber;
        else
            return;

        for (var i = -1; i < 2; i++)
        for (var j = -1; j < 2; j++)
        {
            if (i == 0 && j == 0)
                continue;
            if (x + i >= 0
                && x + i < width
                && y + j >= 0
                && y + j < height)
                DFS(a, x + i, y + j, areaNumber);
        }
    }

    private static Direction ConvertToFrom(Direction d)
    {
        return d switch
        {
            Direction.Up => Direction.Down,
            Direction.Down => Direction.Up,
            Direction.Left => Direction.Right,
            Direction.Right => Direction.Left,
        };
    }

    private static (int, int) GetOffset(Direction d)
    {
        return d switch
        {
            Direction.Up => (-1, 0),
            Direction.Left => (0, -1),
            Direction.Down => (1, 0),
            Direction.Right => (0, 1),
        };
    }

    private static (Direction, Direction) GetDirections(char c)
    {
        return c switch
        {
            '|' => (Direction.Down, Direction.Up),
            '-' => (Direction.Left, Direction.Right),
            'L' => (Direction.Up, Direction.Right),
            'J' => (Direction.Up, Direction.Left),
            '7' => (Direction.Left, Direction.Down),
            'F' => (Direction.Right, Direction.Down),
            '.' => throw new Exception(),
        };
    }

    private enum Direction
    {
        Up,
        Left,
        Down,
        Right,
    }
}