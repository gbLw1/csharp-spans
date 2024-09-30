using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Spans;

public class Program
{
    static void Main(string[] args)
    {
        BenchmarkRunner.Run<Benchy>();
    }
}

[MemoryDiagnoser]
public class Benchy
{
    public static string _dateAsText = "30 09 2024";

    [Benchmark]
    public (int day, int month, int year) DateWithStringAndSubstring()
    {
        var dayAsText = _dateAsText.Substring(0, 2);
        var monthAsText = _dateAsText.Substring(3, 2);
        var yearAsText = _dateAsText.Substring(6);

        var day = int.Parse(dayAsText);
        var month = int.Parse(monthAsText);
        var year = int.Parse(yearAsText);

        return (day, month, year);
    }

    [Benchmark]
    public (int day, int month, int year) DateWithStringAndSpan()
    {
        ReadOnlySpan<char> dateAsSpan = _dateAsText;

        var dayAsText = dateAsSpan.Slice(0, 2);
        var monthAsText = dateAsSpan.Slice(3, 2);
        var yearAsText = dateAsSpan.Slice(6);

        var day = int.Parse(dayAsText);
        var month = int.Parse(monthAsText);
        var year = int.Parse(yearAsText);

        return (day, month, year);
    }
}