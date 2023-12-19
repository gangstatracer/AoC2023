namespace AoC2023.Day14;

public class Day14
{
    [TestCase("Day14/example.txt", 136)]
    [TestCase("Day14/input.txt", 113078)]
    public void Part1(string path, int expected)
    {
        var input = File.ReadLines(path).ToArray();
        var weight = 0;
        for (var j = 0; j < input[0].Length; j++)
        {
            var roundCount = 0;
            var boundary = -1;
            for (var i = 0; i < input.Length; i++)
            {
                if (input[i][j] == 'O')
                    roundCount++;
                if (input[i][j] == '#'
                    || i == input.Length - 1)
                {
                    weight += (2 * (input.Length - (boundary + 1)) + -1 * (roundCount - 1)) * roundCount / 2;
                    boundary = i;
                    roundCount = 0;
                }
            }
        }

        weight.Should().Be(expected);
    }


    // [TestCase("Day14/example.txt", 64)]
    // [TestCase("Day14/input.txt", 30518)]
    public void Part2(string path, int expected)
    {
        var input = File
            .ReadLines(path)
            .Select(l => l.ToCharArray())
            .ToArray();

        var width = input[0].Length;
        var height = input.Length;

        for (var cycle = 0; cycle < 1_000_000_000; cycle++)
        {
            Tilt(
                startingColumn: 0,
                columnsEnd: i => i < width,
                columnStep: i => i + 1,
                startingBoundary: -1,
                rowStep: i => i + 1,
                rowsEnd: i => i < height,
                getElement: (i, j) => input[i][j],
                setElement: (i, j, c) => input[i][j] = c);

            Tilt(
                startingColumn: 0,
                columnsEnd: i => i < height,
                columnStep: i => i + 1,
                startingBoundary: -1,
                rowStep: i => i + 1,
                rowsEnd: i => i < width,
                getElement: (i, j) => input[j][i],
                setElement: (i, j, c) => input[j][i] = c);

            Tilt(
                startingColumn: 0,
                columnsEnd: i => i < width,
                columnStep: i => i + 1,
                startingBoundary: height,
                rowStep: i => i - 1,
                rowsEnd: i => i >= 0,
                getElement: (i, j) => input[i][j],
                setElement: (i, j, c) => input[i][j] = c);

            Tilt(
                startingColumn: 0,
                columnsEnd: i => i < height,
                columnStep: i => i + 1,
                startingBoundary: width,
                rowStep: i => i - 1,
                rowsEnd: i => i >= 0,
                getElement: (i, j) => input[j][i],
                setElement: (i, j, c) => input[j][i] = c);
        }

        input
            .Select((l, i) => l.Count(c => c == 'O') * (input.Length - i + 1))
            .Sum()
            .Should()
            .Be(expected);
    }

    private static void Tilt(
        int startingColumn,
        Func<int, bool> columnsEnd,
        Func<int, int> columnStep,
        int startingBoundary,
        Func<int, int> rowStep,
        Func<int, bool> rowsEnd,
        Func<int, int, char> getElement,
        Action<int, int, char> setElement)
    {
        void Compact(int boundary, int roundCount, int j, int i)
        {
            var writtenCount = 0;
            for (var k = rowStep(boundary); k != i; k = rowStep(k))
                if (writtenCount < roundCount)
                {
                    setElement(k, j, 'O');
                    writtenCount++;
                }
                else
                    setElement(k, j, '.');
        }

        for (var j = startingColumn; columnsEnd(j); j = columnStep(j))
        {
            var roundCount = 0;
            var boundary = startingBoundary;
            int i;
            for (i = rowStep(startingBoundary); rowsEnd(i); i = rowStep(i))
            {
                if (getElement(i, j) == 'O')
                    roundCount++;

                if (getElement(i, j) == '#')
                {
                    Compact(boundary, roundCount, j, i);

                    boundary = i;
                    roundCount = 0;
                }
            }

            Compact(boundary, roundCount, j, i);
        }
    }
}
