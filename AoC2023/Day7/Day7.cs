namespace AoC2023.Day7;

public class Day7
{
    [TestCase("Day7/example.txt", 6440)]
    [TestCase("Day7/input.txt", 251029473)]
    public void Part1(string path, int expected)
    {
        File
            .ReadLines(path)
            .Select(l =>
            {
                var (hand, bid) = l.Halve(" ", null, int.Parse);
                return (Hand: new Hand1(hand), Bid: bid);
            })
            .OrderBy(h => h.Hand)
            .Select((t, i) => t.Bid * (i + 1))
            .Sum()
            .Should()
            .Be(expected);
    }

    private class Hand1 : IComparable<Hand1>
    {
        private static readonly Dictionary<char, int> Labels = "AKQJT98765432"
            .Select((c, i) => (c, i))
            .ToDictionary(t => t.c, t => 13 - t.i);

        private readonly string _value;

        private readonly HandType _type;

        public Hand1(string value)
        {
            _value = value;
            var stats = value
                .GroupBy(c => c)
                .ToDictionary(g => g.Key, g => g.Count());

            var highest = stats
                .Select(kvp => kvp.Value)
                .OrderByDescending(c => c)
                .ToArray();

            var first = highest.FirstOrDefault();
            var second = highest.Skip(1).FirstOrDefault();
            _type = (first, second) switch
            {
                (5, _) => HandType.FiveOfAKind,
                (4, _) => HandType.FourOfAKind,
                (3, 2) => HandType.FullHouse,
                (3, _) => HandType.ThreeOfAKind,
                (2, 2) => HandType.TwoPair,
                (2, _) => HandType.OnePair,
                (1, _) => HandType.HighCard,
            };
        }

        public int CompareTo(Hand1? other)
        {
            if (_type != other?._type)
                return _type.CompareTo(other?._type);

            for (var i = 0; i < 5; i++)
                if (_value[i] != other._value[i])
                    return Labels[_value[i]].CompareTo(Labels[other._value[i]]);

            return _type.CompareTo(other?._type);
        }
    }

    [TestCase("Day7/example.txt", 5905)]
    [TestCase("Day7/input.txt", 251003917)]
    public void Part2(string path, int expected)
    {
        File
            .ReadLines(path)
            .Select(l =>
            {
                var (hand, bid) = l.Halve(" ", null, int.Parse);
                return (Hand: new Hand2(hand), Bid: bid);
            })
            .OrderBy(h => h.Hand)
            .Select((t, i) => t.Bid * (i + 1))
            .Sum()
            .Should()
            .Be(expected);
    }

    private class Hand2 : IComparable<Hand2>
    {
        private static readonly Dictionary<char, int> Labels = "AKQT98765432J"
            .Select((c, i) => (c, i))
            .ToDictionary(t => t.c, t => 13 - t.i);

        private readonly string _value;

        private readonly HandType _type;

        public Hand2(string value)
        {
            _value = value;
            var stats = value
                .GroupBy(c => c)
                .ToDictionary(g => g.Key, g => g.Count());

            stats.TryGetValue('J', out var jCount);
            var highest = stats.Where(kvp => kvp.Key != 'J')
                .Select(kvp => kvp.Value)
                .OrderByDescending(c => c)
                .ToArray();
            var first = highest.FirstOrDefault();
            var second = highest.Skip(1).FirstOrDefault();
            _type = (first + jCount, second) switch
            {
                (5, _) => HandType.FiveOfAKind,
                (4, _) => HandType.FourOfAKind,
                (3, 2) => HandType.FullHouse,
                (3, _) => HandType.ThreeOfAKind,
                (2, 2) => HandType.TwoPair,
                (2, _) => HandType.OnePair,
                (1, _) => HandType.HighCard,
            };
        }

        public int CompareTo(Hand2? other)
        {
            if (_type != other?._type)
                return _type.CompareTo(other?._type);

            for (var i = 0; i < 5; i++)
                if (_value[i] != other._value[i])
                    return Labels[_value[i]].CompareTo(Labels[other._value[i]]);

            return _type.CompareTo(other?._type);
        }
    }

    private enum HandType
    {
        FiveOfAKind = 7,
        FourOfAKind = 6,
        FullHouse = 5,
        ThreeOfAKind = 4,
        TwoPair = 3,
        OnePair = 2,
        HighCard = 1,
    }
}