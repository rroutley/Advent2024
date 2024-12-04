// #define Sample
using System.Collections.Immutable;

public class Puzzle2 : IPuzzle
{

    const int Increasing = 1;
    const int Decreasing = -1;

    public void Excute(FileInfo input)
    {
#if Sample
        var lines = sample.Split("\r\n");
#else
        var lines = File.ReadAllLines(input.FullName);
#endif


        var result = 0;
        foreach (var report in lines)
        {
            var levels = report.Split(' ').Select(int.Parse);

            if (IsSafe(levels, Increasing) || IsSafe(levels, Decreasing))
            {
                System.Console.WriteLine(report);
                result++;
            }
        }

        System.Console.WriteLine("Part 1 = {0}", result);

        result = 0;
        foreach (var report in lines)
        {
            var levels = report.Split(' ').Select(int.Parse).ToImmutableArray();

            if (IsSafe(levels, Increasing) || IsSafe(levels, Decreasing))
            {
                result++;
                continue;
            }

            if (IsSafeByRemovingOne(levels))
            {
                result++;
                System.Console.WriteLine(report);

            }

        }
        System.Console.WriteLine("Part 2 = {0}", result);
    }

    private bool IsSafeByRemovingOne(ImmutableArray<int> levels)
    {
        for (int i = 0; i < levels.Length; i++)
        {
            var newLevels = levels.RemoveAt(i);

            if (IsSafe(newLevels, Increasing) || IsSafe(newLevels, Decreasing))
            {
                return true;
            }

        }
        return false;
    }

    private bool IsSafe(IEnumerable<int> levels, int direction)
    {
        int last = levels.First();
        foreach (var level in levels.Skip(1))
        {
            var diff = direction * (level - last);
            if (0 >= diff || diff > 3)
            {
                return false;
            }

            last = level;
        }

        return true;
    }

    private string sample = """
7 6 4 2 1
1 2 7 8 9
9 7 6 2 1
1 3 2 4 5
8 6 4 4 1
1 3 6 7 9
""";
}