namespace AoC2023.Day4;

public class Day4
{
    [TestCase("Day4/example.txt", 13)]
    [TestCase("Day4/input.txt", 21558)]
    public void Part1(string path, int expected)
    {
        var input = File.ReadLines(path);
        var sum = 0;
        foreach (var line in input)
        {
            var (_, card) = line.Halve(":");
            var (winningString, havingString) = card.Halve("|");
            var winningNumbers = winningString
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToHashSet();
            var havingNumbers = havingString
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToHashSet();
            winningNumbers.IntersectWith(havingNumbers);
            if (winningNumbers.Count > 0)
                sum += (int)Math.Pow(2, winningNumbers.Count - 1);
        }

        sum.Should().Be(expected);
    }

    [TestCase("Day4/example.txt", 30)]
    [TestCase("Day4/input.txt", 10425665)]
    public void Part2(string path, int expected)
    {
        var input = File.ReadLines(path);
        var cards = new Dictionary<int, int>();

        foreach (var line in input)
        {
            var (id, card) = line.Halve(":");
            var (winningString, havingString) = card.Halve("|");
            var winningNumbers = winningString
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToHashSet();
            var havingNumbers = havingString
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToHashSet();
            winningNumbers.IntersectWith(havingNumbers);
            var cardNumber = int.Parse(id.Split(' ').Last());
            cards[cardNumber] = winningNumbers.Count;
        }

        var pile = new List<int>();
        pile.AddRange(cards.Keys);

        for (var i = 0; i < pile.Count; i++)
        {
            var cardNumber = pile[i];
            var numberOfCopies = cards[cardNumber];
            if (numberOfCopies > 0)
            {
                for (var j = cardNumber + 1; j <= cardNumber + numberOfCopies; j++)
                    pile.Add(j);
            }
        }

        pile
            .Count
            .Should()
            .Be(expected);
    }
}