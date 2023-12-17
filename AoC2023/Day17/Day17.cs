namespace AoC2023.Day17;

public class Day17
{
    [TestCase("Day17/example.txt", 102)]
    [TestCase("Day17/input.txt", 8116)]
    public void Part1(string path, int expected)
    {
        var input = File
            .ReadLines(path)
            .Select(l => l
                .Select(c => c - '0')
                .ToArray())
            .ToArray();
        var losses = new int[input.Length, input[0].Length];
        Search(input, losses, 0, 0, 0, 0, (0, 1));
        losses[input.Length - 1, input[0].Length - 1]
            .Should()
            .Be(expected);
    }

    private static void Search(
        int[][] a,
        int[,] b,
        int x,
        int y,
        int straightLength,
        int previousLoss,
        (int, int) direction)
    {
        if (!(x >= 0 && x < a.Length && y >= 0 && y < a[0].Length))
            return;

        var loss = a[x][y] + previousLoss;
        if (loss < b[x, y] || b[x, y] == 0)
            b[x, y] = loss;
        if (straightLength < 3)
            Search(a, b, x + direction.Item1, y + direction.Item2, straightLength + 1, b[x, y], direction);
        var right = (direction.Item2, direction.Item1);
        Search(a, b, x + right.Item1, y + right.Item2, straightLength: 0, b[x, y], right);
        var left = (direction.Item2 * -1, direction.Item1 * -1);
        Search(a, b, x + left.Item1, y + left.Item2, straightLength: 0, b[x, y], left);
    }
}