namespace AoC2023.Day25;

public class Day25
{
    [TestCase("Day25/example.txt", 54)]
    [TestCase("Day25/input.txt", 562772)]
    public void Part1(string path, int expected)
    {
        var edges = File
            .ReadAllLines(path)
            .Select(l => l.Halve(": ", null, s => s.Split(" ")))
            .SelectMany(t => t.Item2.Select(p => (first: t.Item1, second: p)))
            .ToArray();

        var vertices = edges
            .SelectMany(t => new[] { t.first, t.second })
            .Distinct()
            .ToHashSet();

        var outgoing = edges
            .SelectMany(e => new[] { (e.first, e.second), (e.second, e.first) })
            .ToDictionary(t => t, _ => 1);

        var n = vertices.Count;
        var bestCost = int.MaxValue;
        var bestCut = new HashSet<string>();
        var removed = new HashSet<string>();
        var source = vertices.ToDictionary(v => v, v => new HashSet<string> { v });
        for (var ph = 0; ph < n - 1; ph++)
        {
            var a = new HashSet<string>();
            var w = vertices.ToDictionary(v => v, _ => 0);
            string previous = null!;
            for (var it = 0; it < n - ph; it++)
            {
                string sel = null!;
                foreach (var v in vertices)
                    if (!removed.Contains(v) && !a.Contains(v) && (sel == null || w[v] > w[sel]))
                        sel = v;
                if (it == n - ph - 1)
                {
                    if (w[sel] < bestCost)
                    {
                        bestCost = w[sel];
                        bestCut = source[sel].ToArray().ToHashSet();
                    }

                    source[previous].UnionWith(source[sel]);
                    foreach (var v in vertices)
                    {
                        if (!outgoing.ContainsKey((v, previous)))
                            outgoing[(v, previous)] = 0;
                        outgoing[(v, previous)] += outgoing.GetValueOrDefault((sel, v), 0);
                        outgoing[(previous, v)] = outgoing[(v, previous)];
                    }

                    removed.Add(sel);
                }
                else
                {
                    a.Add(sel);
                    foreach (var v in vertices)
                        w[v] += outgoing.GetValueOrDefault((sel, v));
                    previous = sel;
                }
            }
        }

        (bestCut.Count * (vertices.Count - bestCut.Count)).Should().Be(expected);
    }
}