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


        var fileSystem = LoadFiles(line, [], []);

        Render(fileSystem);

        Defragement(fileSystem);

        Render(fileSystem);

        long result = Checksum(fileSystem);

        Console.WriteLine("Part 1 = {0}", result);



        List<Block> files = new();
        List<Block> freeList = new();
        fileSystem = LoadFiles(line, files, freeList);

        Render(fileSystem);

        DefragementFiles(fileSystem, files, freeList);

        Render(fileSystem);

        result = Checksum(fileSystem);

        Console.WriteLine("Part 2 = {0}", result);

    }

    private void DefragementFiles(List<int> fileSystem, List<Block> files, List<Block> freeList)
    {
        for (int f = files.Count - 1; f >= 0; f--)
        {
            var file = files[f];
            var freeBlock = FindFreeBlock(file);
            if (freeBlock != null)
            {
                // Move the File
                for (int i = 0; i < file.Length; i++)
                {
                    fileSystem[freeBlock.Start + i] = file.File;
                    fileSystem[file.Start + i] = SPARE;
                }
                files[f] = new Block(freeBlock.Start, file.Length, file.File);

                // Shrink the Free block for the destination
                var remainingSpace = freeBlock.Length - file.Length;
                var fi = freeList.IndexOf(freeBlock);

                if (remainingSpace == 0)
                {
                    // entire block was used
                    freeList.RemoveAt(fi);
                }
                else if (remainingSpace > 0)
                {
                    freeList[fi] = new Block(freeBlock.Start + file.Length, remainingSpace, SPARE);
                }


                // Add a Free Block where the file used to be
                var start = file.Start;
                var length = file.Length;
                // Consider Free Blocks either side of where the file used to be and merge if possible
                var adjLeft = freeList.FindIndex(b => b.Start + b.Length == file.Start);
                var adjRight = freeList.FindIndex(b => b.Start == file.Start + file.Length);
                if (adjLeft >= 0 && adjRight >= 0)
                {
                    // Enlarge the block to the left and remove the block on the right
                    freeList[adjLeft] = new Block(freeList[adjLeft].Start, freeList[adjLeft].Length + length + freeList[adjRight].Length, SPARE);
                    freeList.RemoveAt(adjRight);
                }
                else if (adjRight >= 0)
                {
                    // Enlarge the block to the right
                    freeList[adjRight] = new Block(start, length + freeList[adjRight].Length, SPARE);
                }
                else if (adjLeft >= 0)
                {
                    // Enlarge the block to the left
                    freeList[adjLeft] = new Block(freeList[adjLeft].Start, freeList[adjLeft].Length + length, SPARE);
                }
                else
                {
                    // Insert a new Free block where the file was.
                    var i = freeList.FindIndex(b => b.Start > start + length);
                    var item = new Block(start, length, SPARE);
                    if (i >= 0)
                        freeList.Insert(i, item);
                    else
                        freeList.Add(item);
                }


                Render(fileSystem);
            }

        }

        Block FindFreeBlock(Block file)
        {
            for (int b = 0; b < freeList.Count; b++)
            {
                // Only interested in free blocks to the left of the file
                if (freeList[b].Start > file.Start)
                    break;

                if (file.Length <= freeList[b].Length)
                {
                    var freeBlock = freeList[b];
                    return freeBlock;

                }
            }
            return null;
        }
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

    private static List<int> LoadFiles(string line, List<Block> files, List<Block> free)
    {
        Console.WriteLine(line);

        var fileSystem = new List<int>();
        int fileNum = 0;

        var nums = line.Select(c => c - '0').GetEnumerator();
        int current;

        while (nums.MoveNext())
        {
            int fileSize = nums.Current;

            current = fileSystem.Count;
            fileSystem.AddRange(Enumerable.Repeat(fileNum, fileSize));
            files.Add(new Block(current, fileSize, fileNum));

            if (nums.MoveNext())
            {
                var padding = nums.Current;
                if (padding > 0)
                {
                    current = fileSystem.Count;
                    fileSystem.AddRange(Enumerable.Repeat(SPARE, padding));
                    free.Add(new Block(current, padding, SPARE));
                }
            }

            fileNum++;
        }

        return fileSystem;
    }


    record Block(int Start, int Length, int File);

    private string sample = """
2333133121414131402
""";
}