using FluentAssertions;
using NUnit.Framework;

namespace AoC2023.Day1;

public class Day1
{
    [TestCase("Day1/input.txt", 53334)]
    [TestCase("Day1/example.txt", 142)]
    public void Part1(string path, int expected)
    {
        var input = File.ReadLines(path);
        var sum = 0;
        foreach (var line in input)
        {
            int firstDigit = -1, lastDigit = 0;
            foreach (var c in line)
            {
                if (char.IsDigit(c))
                {
                    var digit = c - '0';
                    if (firstDigit == -1)
                        firstDigit = digit;
                    lastDigit = digit;
                }
            }

            sum += firstDigit * 10 + lastDigit;
        }

        sum.Should().Be(expected);
    }

    [TestCase("Day1/input.txt", 52834)]
    [TestCase("Day1/example2.txt", 281)]
    public void Part2(string path, int expected)
    {
        var input = File.ReadLines(path);
        var sum = 0;
        var digitWords = new List<string>
        {
            "one",
            "two",
            "three",
            "four",
            "five",
            "six",
            "seven",
            "eight",
            "nine",
        };

        foreach (var line in input)
        {
            int firstDigit = -1, lastDigit = 0;
            for (var index = 0; index < line.Length; index++)
            {
                var c = line[index];
                int digit;
                if (char.IsDigit(c))
                    digit = c - '0';
                else
                {
                    var digitWordIndex = digitWords.FindIndex(w =>
                        index + w.Length <= line.Length
                        && line.Substring(index, w.Length) == w);

                    if (digitWordIndex != -1)
                        digit = digitWordIndex + 1;
                    else continue;
                }

                if (firstDigit == -1)
                    firstDigit = digit;
                lastDigit = digit;
            }

            sum += firstDigit * 10 + lastDigit;
            Console.WriteLine(sum);
        }

        sum.Should().Be(expected);
    }
}