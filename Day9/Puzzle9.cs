// #define Sample

using System.Diagnostics;

public class Puzzle9 : IPuzzle
{
    const int SPARE = int.MinValue;

    public void Excute(FileInfo input)
    {
#if Sample
        var lines = sample.Split("\r\n");
#else
        var lines = File.ReadAllLines(input.FullName);
#endif
        var line = lines[0];


        var fileSystem = LoadFiles(line);

        Render(fileSystem);

        Defragement(fileSystem);

        Render(fileSystem);





        long result = Checksum(fileSystem);

        Console.WriteLine("Part 1 = {0}", result);



        Console.WriteLine("Part 2 = {0}", result);

    }

    private long Checksum(List<int> fileSystem)
    {
        return fileSystem.Select((a, i) => a == SPARE ? 0 : (long)a * i).Sum();
    }

    private void Defragement(List<int> fileSystem)
    {
        var end = fileSystem.Count - 1;

        for (int i = 0; i < fileSystem.Count; i++)
        {
            if (end <= i)
                break;

            if (fileSystem[i] == SPARE)
            {
                (fileSystem[i], fileSystem[end]) = (fileSystem[end], fileSystem[i]);

                while (fileSystem[end] == SPARE)
                    end--;
            }

            Render(fileSystem);
        }
    }

    [Conditional("Sample")]
    private static void Render(List<int> fileSystem)
    {
        foreach (var f in fileSystem)
        {
            if (f == SPARE)
                Console.Write('.');
            else
                Console.Write((char)(f + 48));
        }

        Console.WriteLine();
    }

    private static List<int> LoadFiles(string line)
    {
        Console.WriteLine(line);

        var fileSystem = new List<int>();
        int fileNum = 0;

        var nums = line.Select(c => c - '0').GetEnumerator();

        while (nums.MoveNext())
        {
            int fileSize = nums.Current;

            fileSystem.AddRange(Enumerable.Repeat(fileNum, fileSize));

            if (nums.MoveNext())
            {
                var padding = nums.Current;
                fileSystem.AddRange(Enumerable.Repeat(SPARE, padding));
            }

            fileNum++;
        }

        return fileSystem;
    }

    private string sample = """
2333133121414131402
""";
}