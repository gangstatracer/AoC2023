namespace AoC2023.Day3;

public class Day3
{
    [TestCase("Day3/input.txt", 551094)]
    [TestCase("Day3/example.txt", 4361)]
    [TestCase("Day3/test.txt", 2)]
    public void Part1(string path, int expected)
    {
        var input = File.ReadLines(path).Select(l => $".{l}.").ToArray();
        var width = input.First().Length;
        var padString = string.Join("", Enumerable.Repeat('.', width));
        input = input.Prepend(padString).Append(padString).ToArray();

        var sum = 0;
        for (var i = 1; i < input.Length - 1; i++)
        {
            for (var j = 1; j < input[i].Length - 1; j++)
            {
                if (char.IsDigit(input[i][j]))
                {
                    var k = j;
                    while (k < input[i].Length - 1 && char.IsDigit(input[i][k]))
                        k++;

                    var isPartNumber = false;
                    for (var offset = -1; offset < 2; offset++)
                    for (var jj = j - 1; jj <= k; jj++)
                        if (input[i + offset][jj] != '.' && !char.IsDigit(input[i + offset][jj]))
                            isPartNumber = true;

                    if (isPartNumber)
                    {
                        var number = int.Parse(input[i].Substring(j, k - j));
                        Console.WriteLine($"Part number detected: {number}");
                        sum += number;
                    }

                    j = k;
                }
            }
        }

        sum.Should().Be(expected);
    }

    [TestCase("Day3/example.txt", 467835)]
    [TestCase("Day3/input.txt", 80179647)]
    public void Part2(string path, int expected)
    {
        var input = File.ReadLines(path).Select(l => $".{l}.").ToArray();
        var width = input.First().Length;
        var padString = string.Join("", Enumerable.Repeat('.', width));
        input = input.Prepend(padString).Append(padString).ToArray();
        var gears = new Dictionary<int, Dictionary<int, (int Count, int Product)>>();

        for (var i = 1; i < input.Length - 1; i++)
        {
            for (var j = 1; j < input[i].Length - 1; j++)
            {
                if (char.IsDigit(input[i][j]))
                {
                    var k = j;
                    while (k < input[i].Length - 1 && char.IsDigit(input[i][k]))
                        k++;

                    var number = int.Parse(input[i].Substring(j, k - j));
                    for (var offset = -1; offset < 2; offset++)
                    for (var jj = j - 1; jj <= k; jj++)
                        if (input[i + offset][jj] == '*')
                        {
                            if (!gears.ContainsKey(i + offset))
                                gears[i + offset] = new Dictionary<int, (int Count, int Product)>();

                            if (!gears[i + offset].ContainsKey(jj))
                                gears[i + offset][jj] = (Count: 0, Product: 1);

                            var (count, product) = gears[i + offset][jj];
                            gears[i + offset][jj] = (count + 1, product * number);
                        }

                    j = k;
                }
            }
        }

        var trueGears = gears
            .SelectMany(l => l.Value)
            .Select(kvp => kvp.Value)
            .Where(l => l.Count == 2)
            .Select(l => l.Product)
            .ToArray();

        var sum = trueGears
            .Sum();

        sum.Should().Be(expected);
    }
}