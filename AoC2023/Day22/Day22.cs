namespace AoC2023.Day22;

public class Day22
{
    [TestCase("Day22/example.txt", 5)]
    [TestCase("Day22/input.txt", 459)]
    public void Part1(string path, int expected)
    {
        var fallen = GetFallen(path);

        var result = 0;
        foreach (var b in fallen)
        {
            var liesOnB = Intersection(fallen.Where(bb => bb != b), b.MoveZ(1));
            var safe = liesOnB.All(l => Intersection(fallen.Where(bb => bb != l), l.MoveZ(-1)).Take(2).Count() > 1);
            if (safe)
                result++;
        }

        result.Should().Be(expected);
    }
    
    [TestCase("Day22/example.txt", 7)]
    [TestCase("Day22/input.txt", 459)]
    public void Part2(string path, int expected)
    {
        var fallen = GetFallen(path);

        var result = 0;
        foreach (var b in fallen)
        {
            var liesOnB = Intersection(fallen.Where(bb => bb != b), b.MoveZ(1));
            var safe = liesOnB.All(l => Intersection(fallen.Where(bb => bb != l), l.MoveZ(-1)).Take(2).Count() > 1);
            if (safe)
                result++;
        }

        result.Should().Be(expected);
    }

    private static List<Brick> GetFallen(string path)
    {
        var input = File
            .ReadLines(path)
            .Select(l => l.Halve("~", ToCube, ToCube))
            .Select(t => new Brick(t.Item1, t.Item2))
            .OrderBy(b => b.LowerPoint)
            .ToArray();
        var fallen = new List<Brick>();
        foreach (var brick in input)
        {
            var fallenBrick = brick;
            do
            {
                fallenBrick = fallenBrick.MoveZ(-1);
            } while (fallenBrick.LowerPoint > 0 && !Intersection(fallen, fallenBrick).Any());

            fallen.Add(fallenBrick.MoveZ(1));
        }

        ToString(fallen);
        return fallen;
    }

    private static IEnumerable<Brick> Intersection(IEnumerable<Brick> bricks, Brick brick)
    {
        return bricks.Where(b =>
            Intersects((b.Start.X, b.End.X), (brick.Start.X, brick.End.X))
            && Intersects((b.Start.Y, b.End.Y), (brick.Start.Y, brick.End.Y))
            && Intersects((b.Start.Z, b.End.Z), (brick.Start.Z, brick.End.Z)));
    }

    private static bool Intersects(
        (int X, int Y) first,
        (int X, int Y) second)
    {
        return first.X <= second.Y && first.Y >= second.X;
    }

    private static Cube ToCube(string s)
    {
        var parts = s.Split(",")
            .Select(int.Parse)
            .ToArray();
        return new Cube(parts[0], parts[1], parts[2]);
    }

    private record Cube(int X, int Y, int Z)
    {
        public Cube MoveZ(int offset) => this with { Z = Z + offset };
    }

    private record Brick(Cube Start, Cube End)
    {
        public int LowerPoint => Math.Min(Start.Z, End.Z);

        public Brick MoveZ(int offset) => new(
            Start.MoveZ(offset),
            End.MoveZ(offset));
    }

    private static void ToString(List<Brick> bricks)
    {
        var xRange = bricks.Select(b => b.End.X).Max();
        var yRange = bricks.Select(b => b.End.Y).Max();
        var zRange = bricks.Select(b => b.End.Z).Max();
        var space = new int[xRange + 1, yRange + 1, zRange + 1];
        foreach (var b in bricks)
        {
            for (var i = b.Start.X; i <= b.End.X; i++)
            for (var j = b.Start.Y; j <= b.End.Y; j++)
            for (var k = b.Start.Z; k <= b.End.Z; k++)
                space[i, j, k] += 1;
        }

        for (var z = 0; z < space.GetLength(2); z++)
        {
            for (var x = 0; x < space.GetLength(0); x++)
            {
                for (var y = 0; y < space.GetLength(1); y++)
                    Console.Write(space[x, y, z]);
                Console.WriteLine();
            }

            Console.WriteLine();
        }
    }
}