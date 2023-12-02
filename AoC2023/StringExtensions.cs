namespace AoC2023;

public static class StringExtensions
{
    public static (string, string) Halve(this string value, string separator)
    {
        var parts = value.Split(separator);
        if (parts.Length != 2)
            throw new Exception($"Parts lenght is {parts.Length}");
        return (parts[0], parts[1]);
    }

    public static (T, string) Halve<T>(this string value, string separator, Func<string, T> part0Parser)
    {
        var (part0, part1) = value.Halve(separator);
        return (part0Parser(part0), part1);
    }

    public static (string, T) Halve<T>(
        this string value,
        string separator,
        Func<string, string>? part0Parser,
        Func<string, T> part1Parser)
    {
        var (part0, part1) = value.Halve(separator);
        return (part0Parser == null ? part0 : part0Parser(part0), part1Parser(part1));
    }
}