// #define Sample


public class Puzzle11 : IPuzzle
{

    public void Excute(FileInfo input)
    {
#if Sample
        var lines = sample.Split("\r\n");
#else
        var lines = File.ReadAllLines(input.FullName);
#endif

        var arrangement = lines[0].Split(' ').Select(long.Parse);

        Render(arrangement);

        for (int i = 0; i < 25; i++)
        {
            Console.Write($"Blink {i + 1}: ");
            arrangement = Blink(arrangement).ToList();
            if (i < 6)
            {
                Render(arrangement);
            }
            Console.WriteLine($". {arrangement.Count()} stones");
        }

        long result = arrangement.Count();

        Console.WriteLine("Part 1 = {0}", result);


        var freqs = arrangement.GroupBy(s => s).Select(s => new Freq(s.Key, s.Count()));

        for (int i = 25; i < 75; i++)
        {
            Console.Write($"Blink {i + 1}: ");
            freqs = Blink(freqs).GroupBy(s => s.Value).Select(s => new Freq(s.Key, s.Select(c => c.Count).Sum()));

            Console.WriteLine($". {freqs.Select(x => x.Count).Sum()} stones");
        }


        Console.WriteLine("Part 2 = {0}", freqs.Select(x => x.Count).Sum());

    }

    private static void Render(IEnumerable<long> arrangement)
    {
        foreach (var stone in arrangement)
        {
            Console.Write(stone);
            Console.Write(' ');
        }
        Console.WriteLine();
    }

    private IEnumerable<long> Blink(IEnumerable<long> arrangement)
    {
        foreach (var stone in arrangement)
        {
            if (stone == 0)
            {
                yield return 1;
            }
            else
            {
                var text = stone.ToString().AsSpan();
                if (text.Length % 2 == 0)
                {
                    var half = text.Length / 2;
                    long v1 = long.Parse(text[..half]);
                    long v2 = long.Parse(text[half..]);
                    yield return v1;
                    yield return v2;

                }
                else
                {
                    yield return stone * 2024;
                }
            }
        }
    }

    private IEnumerable<Freq> Blink(IEnumerable<Freq> arrangement)
    {
        foreach (var stone in arrangement)
        {
            if (stone.Value == 0)
            {
                yield return new Freq(1, stone.Count);
            }
            else
            {
                var text = stone.Value.ToString().AsSpan();
                if (text.Length % 2 == 0)
                {
                    var half = text.Length / 2;
                    long v1 = long.Parse(text[..half]);
                    long v2 = long.Parse(text[half..]);
                    yield return new Freq(v1, stone.Count);
                    yield return new Freq(v2, stone.Count);

                }
                else
                {
                    yield return new Freq(stone.Value * 2024, stone.Count);
                }
            }
        }
    }

    private string sample = """
125 17
""";

    private record class Freq(long Value, long Count);
}