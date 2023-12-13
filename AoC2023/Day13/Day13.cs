namespace AoC2023.Day13;

public class Day13
{
    [TestCase("Day13/example.txt", 405)]
    [TestCase("Day13/input.txt", 30518)]
    public void Part1(string path, int expected)
    {
        var input = File.ReadLines(path).ToArray();
        var patterns = new List<List<string>>();
        var currentPattern = new List<string>();
        var i = 0;
        do
        {
            if (string.IsNullOrEmpty(input[i]))
            {
                patterns.Add(currentPattern);
                currentPattern = new List<string>();
            }
            else
            {
                currentPattern.Add(input[i]);
            }

            i++;
        } while (i < input.Length);

        patterns.Add(currentPattern);

        patterns.Select(Score).Sum().Should().Be(expected);
    }

    private static int Score(List<string> pattern)
    {
        var vertical = FindSymmetry(pattern);
        if (vertical >= 0)
            return vertical;
        var horizontal = FindSymmetry(Rotate(pattern).ToList());
        return 100 * horizontal;
    }

    private static IEnumerable<string> Rotate(IReadOnlyList<string> pattern)
    {
        for (var i = 0; i < pattern[0].Length; i++)
            yield return new string(pattern.Select(l => l[i]).ToArray());
    }

    private static int FindSymmetry(IReadOnlyList<string> pattern)
    {
        var width = pattern[0].Length;
        for (var i = 1; i < width; i++)
        {
            var palindrome = true;
            for (var j = 0; j < i; j++)
            {
                var left = j;
                var right = i + i - j - 1;
                var symmetric = right >= width || pattern.All(l => l[left] == l[right]);

                if (!symmetric)
                {
                    palindrome = false;
                    break;
                }
            }

            if (palindrome)
                return i;
        }

        return -1;
    }
}