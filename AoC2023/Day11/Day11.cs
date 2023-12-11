namespace AoC2023.Day11;

public class Day11
{
    [TestCase("Day11/example.txt", 374)]
    [TestCase("Day11/input.txt", 9742154)]
    public void Part1(string path, int expected)
    {
        var map = File
            .ReadLines(path)
            .Select(l => l.ToList())
            .ToList();

        var columnsToExpand = Enumerable
            .Repeat(true, map.First().Count)
            .ToArray();

        for (var i = 0; i < map.Count; i++)
        {
            var line = map[i];
            var expand = true;
            for (var j = 0; j < map[i].Count; j++)
                if (map[i][j] != '.')
                {
                    columnsToExpand[j] = false;
                    expand = false;
                }

            if (expand)
            {
                i++;
                map.Insert(
                    i,
                    Enumerable
                        .Repeat('.', line.Count)
                        .ToList());
            }
        }

        foreach (var (i, o) in columnsToExpand
                     .Select((c, i) => (c, i))
                     .Where(t => t.c)
                     .Select((t, o) => (t.i, o)))
        foreach (var line in map)
            line.Insert(i + o, '.');

        var galaxies = new List<(int, int)>();
        for (var i = 0; i < map.Count; i++)
        for (var j = 0; j < map[i].Count; j++)
        {
            if (map[i][j] == '#')
                galaxies.Add((i, j));
        }

        var sum = 0;
        for (var i = 0; i < galaxies.Count; i++)
        for (var j = i + 1; j < galaxies.Count; j++)
            sum += Math.Abs(galaxies[i].Item1 - galaxies[j].Item1)
                   + Math.Abs(galaxies[i].Item2 - galaxies[j].Item2);

        sum.Should().Be(expected);
    }

    [TestCase("Day11/example.txt", 2, 374)]
    [TestCase("Day11/input.txt", 1_000_000, 411142919886L)]
    public void Part2(string path, int expansionMultiplier, long expected)
    {
        var map = File
            .ReadLines(path)
            .Select(l => l.ToList())
            .ToList();

        var columnsToExpand = Enumerable
            .Range(0, map.First().Count)
            .ToHashSet();

        var rowsToExpand = Enumerable
            .Range(0, map.Count)
            .ToHashSet();

        for (var i = 0; i < map.Count; i++)
        {
            for (var j = 0; j < map[i].Count; j++)
                if (map[i][j] != '.')
                {
                    columnsToExpand.RemoveWhere(e => e == j);
                    rowsToExpand.RemoveWhere(r => r == i);
                }
        }

        var galaxies = new List<(int, int)>();
        for (var i = 0; i < map.Count; i++)
        for (var j = 0; j < map[i].Count; j++)
        {
            if (map[i][j] == '#')
                galaxies.Add((i, j));
        }

        int GetActualX(int x) => x + rowsToExpand.Count(r => r < x) * (expansionMultiplier - 1);
        int GetActualY(int y) => y + columnsToExpand.Count(c => c < y) * (expansionMultiplier - 1);

        var sum = 0L;
        for (var i = 0; i < galaxies.Count; i++)
        for (var j = i + 1; j < galaxies.Count; j++)
        {
            var xi = GetActualX(galaxies[i].Item1);
            var xj = GetActualX(galaxies[j].Item1);
            var xOffset = Math.Abs(xi - xj);
            var yi = GetActualY(galaxies[i].Item2);
            var yj = GetActualY(galaxies[j].Item2);
            var yOffset = Math.Abs(yi - yj);
            var distance = xOffset + yOffset;
            sum += distance;
        }

        sum.Should().Be(expected);
    }
}