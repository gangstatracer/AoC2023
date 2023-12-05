namespace AoC2023.Day5;

public class Day5
{
    [TestCase("Day5/example.txt", 35)]
    [TestCase("Day5/input.txt", 31599214)]
    public void Part1(string path, long expected)
    {
        var input = File
            .ReadLines(path)
            .Where(l => !string.IsNullOrEmpty(l))
            .ToArray();

        var seeds = input
            .First()
            .Halve(": ")
            .Item2
            .Split(' ')
            .Select(long.Parse)
            .ToArray();

        var maps = new List<List<(long, long, long)>>();
        List<(long, long, long)>? currentMap = null;
        foreach (var line in input.Skip(1).Append("#"))
        {
            if (!char.IsDigit(line[0]))
            {
                if (currentMap != null)
                    maps.Add(currentMap);
                currentMap = new List<(long, long, long)>();
            }
            else
            {
                var parts = line.Split(' ').Select(long.Parse).ToArray();
                currentMap!.Add((parts[0], parts[1], parts[2]));
            }
        }

        foreach (var map in maps)
        {
            for (var i = 0; i < seeds.Length; i++)
            {
                foreach (var (destination, source, length) in map)
                {
                    if (seeds[i] >= source && seeds[i] < source + length)
                    {
                        seeds[i] = destination + seeds[i] - source;
                        break;
                    }
                }
            }
        }

        seeds.Min().Should().Be(expected);
    }

    [TestCase("Day5/example.txt", 46)]
    [TestCase("Day5/input.txt", 20358599)]
    public void Part2(string path, long expected)
    {
        var input = File
            .ReadLines(path)
            .Where(l => !string.IsNullOrEmpty(l))
            .ToArray();

        var seedData = input
            .First()
            .Halve(": ")
            .Item2
            .Split(' ')
            .Select(long.Parse)
            .ToArray();

        var seedRanges = new List<(long Start, long Length)>();
        for (var i = 0; i < seedData.Length; i += 2)
        {
            seedRanges.Add((seedData[i], seedData[i + 1]));
        }

        var maps = new List<List<(long Destination, long Source, long Length)>>();
        List<(long, long, long)>? currentMap = null;
        foreach (var line in input.Skip(1).Append("#"))
        {
            if (!char.IsDigit(line[0]))
            {
                if (currentMap != null)
                    maps.Add(currentMap);
                currentMap = new List<(long, long, long)>();
            }
            else
            {
                var parts = line.Split(' ').Select(long.Parse).ToArray();
                currentMap!.Add((parts[0], parts[1], parts[2]));
            }
        }

        foreach (var map in maps)
        {
            var split = new List<(long, long)>();
            var splitPoints = map
                .SelectMany(m => new[]
                {
                    (m.Source, 0),
                    (m.Source + m.Length - 1, 1),
                })
                .ToArray();

            Console.WriteLine($"Split points: {string.Join(" ", splitPoints)}");

            foreach (var (seedStart, seedLength) in seedRanges)
            {
                var points = splitPoints
                    .Where(p => p.Item1 >= seedStart
                                && p.Item1 < seedStart + seedLength)
                    .OrderBy(p => p)
                    .Prepend((seedStart, 0))
                    .Append((seedStart + seedLength - 1, 1))
                    .ToArray();

                for (var i = 0; i < points.Length - 1; i++)
                    split.Add(
                        (
                            points[i].Item1,
                            points[i + 1].Item1 - points[i].Item1 + points[i + 1].Item2
                        ));
            }

            var next = new List<(long, long)>();
            foreach (var (seedStart, seedLength) in split)
            {
                var mapped = false;
                foreach (var (destination, source, length) in map)
                {
                    if (seedStart >= source
                        && seedStart + seedLength <= source + length)
                    {
                        next.Add((seedStart + destination - source, seedLength));
                        mapped = true;
                    }
                }

                if (!mapped)
                    next.Add((seedStart, seedLength));
            }

            seedRanges = next;
        }

        seedRanges
            .Select(r => r.Start)
            .Min()
            .Should()
            .Be(expected);
    }
}