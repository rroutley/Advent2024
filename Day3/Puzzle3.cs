// #define Sample
using System.Text.RegularExpressions;

public partial class Puzzle3 : IPuzzle
{

    [GeneratedRegex(@"mul\((\d{1,3}),(\d{1,3})\)")]
    public partial Regex MulRegex { get; }

    public void Excute(FileInfo input)
    {
#if Sample
        var line = sample;
#else
        var line = File.ReadAllText(input.FullName);
#endif

        long result;
        result = SumMuls(line);

        System.Console.WriteLine("Part 1 = {0}", result);


        result = 0;
        var ranges = SplitIntoDos(line);

        foreach (var range in ranges)
        {
            result += SumMuls(line[range]);
        }

        System.Console.WriteLine("Part 2 = {0}", result);
    }

    private long SumMuls(string line)
    {
        long result = 0;
        var matches = MulRegex.Matches(line);
        foreach (Match match in matches)
        {
            result += int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value);
        }
        return result;
    }

    private static IEnumerable<Range> SplitIntoDos(string line)
    {
        bool inDo = true;
        int start = 0;

        while (start >= 0)
        {
            if (inDo)
            {
                var end = line.IndexOf("don't()", start);

                if (end < 0)
                    end = line.Length;

                yield return new Range(start, end);
                inDo = false;
                start = end;
            }
            else
            {
                start = line.IndexOf("do()", start);
                inDo = true;
            }
        }
    }

    private string sample = """
xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))
""";
}