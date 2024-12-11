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

        var arrangement = lines[0].Split(' ').Select(long.Parse).ToList();

        Render(arrangement);

        for (int i = 0; i < 25; i++)
        {
            Console.Write($"Blink {i+1}");
            arrangement = Blink(arrangement).ToList();
            if (i < 6)
            {
                Render(arrangement);
            }
            Console.WriteLine($". {arrangement.Count} stones");
        }

        long result = arrangement.Count;

        Console.WriteLine("Part 1 = {0}", result);




        Console.WriteLine("Part 2 = {0}", result);

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

    private IEnumerable<long> Blink(IList<long> arrangement)
    {
        foreach (var stone in arrangement)
        {
            if (stone == 0)
            {
                yield return 1;
            }
            else if (stone.ToString().Length % 2 == 0)
            {
                var text = stone.ToString();
                var half = text.Length / 2;
                yield return long.Parse(text[..half]);
                yield return long.Parse(text[half..]);

            }
            else
            {
                yield return stone * 2024;
            }
        }
    }

    private string sample = """
125 17
""";
}