// #define Sample


public class Puzzle19 : IPuzzle
{

    public void Excute(FileInfo input)
    {
#if Sample
        var lines = sample.Split("\r\n");
#else
        var lines = File.ReadAllLines(input.FullName);
#endif

        var towels = lines[0].Split(',').Select(s => s.Trim());


        long result = 0;
        for (int i = 2; i < lines.Length; i++)
        {
            var line = lines[i];

            if (IsCovering(line, towels))
            {
                result++;
                //System.Console.WriteLine(line);
            }
        }


        System.Console.WriteLine("Part 1 = {0}", result);

        var t2 = towels.GroupBy(t => t[0]).ToDictionary(s => s.Key, v => v.AsEnumerable());

        result = 0;
        for (int i = 2; i < lines.Length; i++)
        {
            var line = lines[i];

            result += Covering(line, t2);

            System.Console.Write('.');
        }
        System.Console.WriteLine("Part 2 = {0}", result);

    }

    private bool IsCovering(ReadOnlySpan<char> line, IEnumerable<string> towels)
    {
        foreach (var t in towels)
        {
            if (line.StartsWith(t.AsSpan()))
            {
                if (t.Length == line.Length)
                    return true;

                var yes = IsCovering(line[t.Length..], towels);
                if (yes)
                    return true;
            }
        }

        return false;
    }


    private int Covering(ReadOnlySpan<char> line, IReadOnlyDictionary<char, IEnumerable<string>> towels)
    {
        if (line.Length == 0)
        {
            return 1;
        }

        int c = 0;
        foreach (var t in towels[line[0]])
        {
            if (line.StartsWith(t.AsSpan()))
            {
                c += Covering(line[t.Length..], towels);
            }
        }

        return c;
    }
    private string sample = """
r, wr, b, g, bwu, rb, gb, br

brwrr
bggr
gbbr
rrbgbr
ubwu
bwurrg
brgr
bbrgwb   
""";
}